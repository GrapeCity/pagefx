using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using DataDynamics.PageFX.CLI.IL;
using DataDynamics.PageFX.CodeModel;
using MethodBody=DataDynamics.PageFX.CLI.IL.MethodBody;

namespace DataDynamics.PageFX.CLI.CFG
{
    internal class StructuringEngine
    {
        private MethodBody _body;

        #region Run
        public Node Run(Node entry, MethodBody body)
        {
            _body = body;
            CollapseBlocks(ref entry, body);
            CollapseGraph(ref entry);
            return entry;
        }
        #endregion

        #region CollapseBlocks
        private void CollapseBlocks(ref Node entry, MethodBody body)
        {
            if (!body.HasProtectedBlocks) return;

            foreach (var block in body.ProtectedBlocks)
            {
                CollapseBlock(ref entry, block);
            }
        }

        private void CollapseBlock(ref Node entry, Block block)
        {
            foreach (var kid in block.Kids)
            {
                CollapseBlock(ref entry, kid);
                kid.Node.OwnerBlock = block;
            }

            var pb = block as ProtectedBlock;
            if (pb != null)
            {
                foreach (var h in pb.Handlers)
                {
                    CollapseBlock(ref entry, h);
                }
            }

            var normalEntries = new List<Edge>(); //normal entries to block
            var normalExits = new List<Edge>(); //normal entries from block

            var blockEntry = block.GetEntryNode();
            var blockExit = block.GetExitNode();

            Debug.Assert(blockEntry != null);
            Debug.Assert(pb == null || blockExit != null);

            bool forceReplaceEntry = blockEntry == entry;
            if (blockEntry.IsEntry)
            {
                if (entry != blockEntry)
                    blockEntry.RemoveEntry();
            }
            blockEntry.IsEntry = true;
            
            //Remove subgraph
            var nodes = block.GetNodes();
            foreach (var node in nodes)
            {
                node.Remove();
            }

            //Link nodes with each other in double linked list
            for (int i = 1; i < nodes.Count; ++i)
            {
                nodes[i - 1].Append(nodes[i]);
            }

            //Detach block entries and exits
            //Resolve gotos to collapsed block
            foreach (var node in nodes)
            {
                foreach (var e in node.InEdgeList)
                {
                    if (e.To == blockEntry)
                    {
                        normalEntries.Add(e);
                        e.Remove();
                    }
                    else if (e.From.OwnerBlock != block)
                    {
                        Goto(e);
                    }
                }
                foreach (var e in node.OutEdgeList)
                {
                    if (e.To == blockExit)
                    {
                        normalExits.Add(e);
                        //NOTE: we will not change the structure of block flow graph.
                        //e.Remove();
                    }
                    else if (e.To.OwnerBlock != block)
                    {
                        Goto(e);
                    }
                }
            }

            //Render(entry);

            CollapseGraph(ref blockEntry);

            if (pb != null)
            {
                var newNode = new TryNode();
                newNode.Block = pb;
                block.Node = newNode;

                newNode.Body.Add(blockEntry);

                foreach (var h in pb.Handlers)
                {
                    newNode.AddHandler(h.Node as HandlerNode);
                    h.Node = newNode;
                }

                ReplaceBlock(ref entry, blockExit, blockEntry, newNode,
                             normalEntries, normalExits, forceReplaceEntry);
            }
            else
            {
                var hb = block as HandlerBlock;
                Debug.Assert(hb != null);

                var newNode = new HandlerNode();
                newNode.Block = hb;
                block.Node = newNode;

                newNode.Body.Add(blockEntry);

                ReplaceBlock(ref entry, blockExit, blockEntry, newNode,
                             normalEntries, normalExits, forceReplaceEntry);
            }
        }

        private void ReplaceBlock(ref Node entry, 
            Node exit, Node oldNode, Node newNode, 
            IEnumerable<Edge> normalEntries, 
            IEnumerable<Edge> normalExits, 
            bool forceReplaceEntry)
        {
            bool isHandler = newNode.NodeType == NodeType.Handler;
            if (!isHandler)
                exit.Prepend(newNode);

            if (isHandler)
            {
                foreach (var e in normalEntries)
                    Goto(e);
            }
            else
            {
                foreach (var e in normalEntries)
                {
                    e.To = newNode;
                    e.From.AppendOut(e);
                }

                foreach (var e in normalExits)
                {
                    e.From = newNode;
                    //NOTE: edge must be exist in list of incoming edges of To node
                    //e.To.AppendIn(e);
                }
            }

            if (isHandler)
            {
                newNode.DetachKids();
                newNode.Detach();
                //newNode.IsEntry = true;
                //Render(newNode);
                //newNode.IsEntry = false;
                //Render(entry);
            }
            else
            {
                ReplaceNode(ref entry, oldNode, newNode, forceReplaceEntry);
            }
        }
        #endregion

        #region CollapseGraph
        private void CollapseGraph(ref Node entry)
        {
            ResolveGotos(ref entry);
            CollapseSequences(ref entry);
            bool change = true;
            while (change)
            {
                change = false;
                foreach (var node in entry.Nodes)
                {
#if DEBUG
                    if (CLIDebug.IsCancel)
                        break;
#endif
                    if (CollapseNode(ref entry, node))
                    {
                        change = true;
                        break;
                    }
                }
            }
        }
        #endregion

        #region CollapseNext
        private Hashtable _excluded;
        private Node _original;

        private bool CollapseNext(ref Node entry, Node node, IEnumerable<Node> toExclude)
        {
            bool result;
            if (_excluded == null)
            {
                _excluded = new Hashtable();
                _original = node;
                if (toExclude != null)
                {
                    foreach (var n in toExclude)
                        _excluded[n] = n;
                }
                result = _CollapseNext(ref entry, node, _excluded);
                _original = null;
                _excluded = null;
            }
            else
            {
                result = _CollapseNext(ref entry, node, _excluded);
            }
            return result;
        }

        private bool _CollapseNext(ref Node entry, Node node, Hashtable excluded)
        {
            foreach (var e in node.OutEdges)
            {
                //if (e.IsBack)
                //{
                //}
                var next = e.To;
                if (excluded.Contains(next)) continue;
                excluded[next] = next;
                if (CollapseNode(ref entry, next))
                    return true;
                if (_CollapseNext(ref entry, next, excluded))
                    return true;
            }
            return false;
        }
        #endregion

        #region CollapseSequences
        private void CollapseSequences(ref Node entry)
        {
            _collapseNewNode = false;
            bool change = true;
            while (change)
            {
                change = false;
                foreach (var node in entry.Nodes)
                {
                    if (CollapseSequence(ref entry, node))
                    {
                        change = true;
                        break;
                    }
                }
            }
            _collapseNewNode = true;
        }
        #endregion

        #region CollapseSequence
        private static bool IsSequence(Node node)
        {
            //if (node.IsLabeled) return false;
            if (node.HasZeroOrOneIn)
            {
                var e = node.FirstOut;
                if (e == null) return true;
                if (e.IsBack) return false;
                return e.NextOut == null;
            }
            return false;
        }

        private static bool IsSequenceEnd(Node node, out bool br)
        {
            br = false;
            //if (node.IsLabeled) return false;
            if (node.HasZeroOrOneIn)
            {
                var e = node.FirstOut;
                if (e == null) return true;
                if (e.NextOut == null)
                {
                    if (e.IsBack)
                    {
                        br = true;
                    }
                    return true;
                }
            }
            return false;
        }

        private bool CollapseSequence(ref Node entry, Node node)
        {
            if (node.NodeType == NodeType.Sequence)
                return false;

            if (!IsSequence(node))
                return false;

            var start = node;
            var end = node;
            while (true)
            {
                node = start.FirstPredecessor;
                if (node == null) break;
                if (!IsSequence(node)) break;
                if (node.OwnerBlock != start.OwnerBlock) break;
                start = node;
            }

            bool br;
            while (true)
            {
                node = end.FirstSuccessor;
                if (node == null) break;
                if (!IsSequenceEnd(node, out br)) break;
                if (node.OwnerBlock != end.OwnerBlock) break;
                end = node;
                if (br) break;
            }

            if (start == end)
                return false;

            var seq = new SequenceNode();
            node = start;
            while (node != null)
            {
                seq.Add(node);
                if (node == end) break;
                node = node.FirstSuccessor;
            }

            ReplaceSuccessor(start, seq, null);
            ReplacePredecessor(end, seq);
            ReplaceNode(ref entry, start, seq);

            return true;
        }
        #endregion

        #region CollapseNode
        private bool CollapseNode(ref Node entry, Node node)
        {
            if (CollapseSwitch(ref entry, node))
                return true;

            if (CollapseTwoWay(ref entry, node))
                return true;

            if (CollapseEndlessLoop(ref entry, node))
                return true;

            if (CollapseSequence(ref entry, node))
                return true;

            return false;
        }
        #endregion

        #region CollapseIf
        private void IF1(ref Node entry, Node C, Node T, Node F, Edge se, bool negate)
        {
            var IF = new IfNode(C, T, F);
            IF.Negate = negate;
            ReplaceSuccessor(C, IF, null);
            if (se != null)
                se.From = IF;
            ReplaceNode(ref entry, C, IF);
        }

        // Single Branch Conditional: 
        //     A
        //    / \
        //   /   \
        //  B  -> C

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="A">A</param>
        /// <param name="AB">A -> B</param>
        /// <param name="AC">A -> C</param>
        /// <param name="BC">B -> C</param>
        private void IF2(ref Node entry, Node A, Edge AB, Edge AC, Edge BC)
        {
            var TN = AB.To;
            var IF = new IfNode(A, TN);
            IF.Negate = AC.Label != BC.Label;
            IF.And = BC.Label == 0;
            ReplaceSuccessor(A, IF, null);
            ReplacePredecessor(TN, IF);
            ReplaceNode(ref entry, A, IF);
        }

        //lsn > 0, rsn == 0
        private void IF3(ref Node entry, Node C, Edge F, Edge T)
        {
            var L = F.To;
            var R = T.To;
            var IF = new IfNode(C, R);
            IF.Negate = false;
            IF.DetachTrue = false;

            ReplaceSuccessor(C, IF, null);

            F.From = IF;
            //F.Remove();
            T.Remove();
            
            ReplaceNode(ref entry, C, IF);
        }

        //    IF
        //    /\
        //   L  R
        //  /    \
        // LFS   RFS
        //  \    /
        //   \  /
        // LOOPHEADER
        private void IF4(ref Node entry, Node C, Edge F, Edge T)
        {
            var L = F.To;
            var R = T.To;
            var LFS = L.FirstSuccessor;
            var RFS = R.FirstSuccessor;
            var lseq = new SequenceNode(L, LFS);
            var rseq = new SequenceNode(R, RFS);

            var IF = new IfNode(C, lseq, rseq);

            ReplaceSuccessor(C, IF, null);

            var e = LFS.FirstOut;
            e.From = IF;
            RFS.FirstOut.Remove();

            ReplaceNode(ref entry, C, IF);
        }

        private void IF5(ref Node entry, Node C, Edge F, Edge T)
        {
            var L = F.To;
            var R = T.To;
            var LFS = L.FirstSuccessor;
            var RFS = R.FirstSuccessor; //LH

            var IF = new IfNode(RFS, C);
            ReplaceSuccessor(RFS, IF, null);

            F.From = IF;
            var e = R.FirstOut;
            e.From = R;
            e.To = IF;

            e = RFS.FirstOut;
            if (e.To == C)
                e = e.NextOut;
            if (e.To == R)
                e.From = IF;

            ReplaceNode(ref entry, RFS, IF);

            CollapseWhile(ref entry, IF, IF.FalseEdge, IF.TrueEdge);
        }

        //Note: works only for two way nodes
        private static Edge GetOppositeEdge(Edge e)
        {
            var e2 = e.NextOut;
            if (e2 != null) return e2;
            return e.PrevOut;
        }

        private void Break(ref Node entry, Edge br, Edge next)
        {
            var T = CreateGoto(br.To);
            var IF = new IfNode(br.From, T);
            ReplaceSuccessor(IF.Condition, IF, null);
            br.Remove();
            next.From = IF;
            ReplaceNode(ref entry, IF.Condition, IF);
        }

        // Single Branch Conditionals
        private bool CollapseSingleBranch(ref Node entry, Node C, Edge F, Edge T, bool breaks)
        {
            var L = F.To;
            var R = T.To;

            var e = L.FindOut(R);
            if (e != null)
            {
                if (L.IsTwoWay)
                {
                    var e2 = GetOppositeEdge(e);
                    if (IsBreakEdge(e2))
                    {
                        Break(ref entry, e2, e);
                        return true;
                    }
                }
                if (breaks && F.IsBack)
                {
                    Break(ref entry, T, F);
                    return true;
                }
                IF2(ref entry, C, F, T, e);
                return true;
            }

            e = R.FindOut(L);
            if (e != null)
            {
                IF2(ref entry, C, T, F, e);
                return true;
            }

            return false;
        }

        private bool CollapseSingleBranch(ref Node entry, Node C, bool breaks)
        {
            if (C.IsTwoWay)
            {
                var F = C.FalseEdge;
                var T = C.TrueEdge;
                return CollapseSingleBranch(ref entry, C, F, T, breaks);
            }
            return false;
        }

        private bool CollapseSimpleIf(ref Node entry, Node C, Edge F, Edge T)
        {
            var L = F.To;
            var R = T.To;
            int lsn = L.OutCount;
            int rsn = R.OutCount;

//#if DEBUG
//            if (F.IsBack || T.IsBack)
//                Debugger.Break();
//#endif

            //   C
            //  / \
            // L   R -> ?
            if (lsn == 0)
            {
                int lin = L.InCount;
                if (rsn == 0)
                {
                    int rin = R.InCount;
                    var tnode = rin == 1 ? R : CreateGoto(R);
                    var fnode = lin == 1 ? L : CreateGoto(L);
                    var IF = new IfNode(C, tnode, fnode);
                    IF.Negate = false;
                    ReplaceSuccessor(C, IF, null);
                    T.Remove();
                    F.Remove();
                    ReplaceNode(ref entry, C, IF);
                    return true;
                }
                else
                {
                    var tnode = lin == 1 ? L : CreateGoto(L);
                    var IF = new IfNode(C, tnode);
                    ReplaceSuccessor(C, IF, null);
                    F.Remove();
                    T.From = IF;
                    ReplaceNode(ref entry, C, IF);
                    return true;
                }
            }

            //   C
            //  / \
            // L   R 
            //  \ /
            //  CS
            if (lsn == 1 && rsn == 1 && L.FirstSuccessor == R.FirstSuccessor)
            {
                //NOTE
                //We can not negate ternary conditional node to avoid problems
                //with decompilation of boolean expressions
                int lin = L.InCount;
                int rin = R.InCount;
                var tnode = rin == 1 ? R : CreateGoto(R);
                var fnode = lin == 1 ? L : CreateGoto(L);
                var IF = new IfNode(C, tnode, fnode);
                IF.Negate = false;
                ReplaceSuccessor(C, IF, null);
                L.FirstOut.From = IF;
                T.Remove();
                F.Remove();
                ReplaceNode(ref entry, C, IF);
                return true;
            }

            // Single Branch Conditionals?
            if (CollapseSingleBranch(ref entry, C, F, T, true))
                return true;

            //    C
            //   / \
            //  L   R
            // / \?
            if (rsn == 0)
            {
                //Special case when R is ended with ret or throw instructions
                //    C
                //   / \
                //  L ->R
                if (lsn == 1 && L.FirstSuccessor == R && R.IsBasicBlock && L.IsBasicBlock)
                {
                    var seq = new SequenceNode(L, R);
                    var IF = new IfNode(C, seq, R);
                    ReplaceSuccessor(C, IF, null);
                    ReplaceNode(ref entry, C, IF);
                    return true;
                }

                //TODO: Optimize this branch, it can be used only for small codes
                if (R.IsBasicBlock)
                {
                    IF3(ref entry, C, F, T);
                    return true;
                }
                if (R.InCount == 1)
                {
                    IF1(ref entry, C, R, null, F, false);
                    return true;
                }
            }

            return false;
        }

        private bool CollapseSimpleIf(ref Node entry, Node C)
        {
            var F = C.FalseEdge;
            var T = C.TrueEdge;
            return CollapseSimpleIf(ref entry, C, F, T);
        }

        private bool CollapseIf(ref Node entry, Node C, Edge F, Edge T)
        {
            if (CollapseSimpleIf(ref entry, C, F, T))
                return true;

            var L = F.To;
            var R = T.To;
            int lsn = L.OutCount;
            int rsn = R.OutCount;

            if (lsn == 2)
            {
                if (CollapseSimpleIf(ref entry, L))
                    return true;
            }

            //if (rsn == 2)
            //{
            //    if (CollapseSimpleIf(ref entry, R))
            //        return true;
            //}

            if (lsn == 1 && rsn == 1)
            {
                var LFS = L.FirstSuccessor;
                var RFS = R.FirstSuccessor;

                //    IF
                //    /\
                //   L  R
                //  /    \
                // LFS   RFS
                //  \    /
                //   \  /
                // LOOPHEADER (LH)
                if (LFS.HasOutBackEdges && RFS.HasOutBackEdges
                    && LFS.OutCount == 1 && RFS.OutCount == 1
                    && LFS.FirstOut.To == RFS.FirstOut.To)
                {
                    IF4(ref entry, C, F, T);
                    return true;
                }

                //    IF
                //    /\
                //   L  R
                //  /    \
                // LFS   LH -> IF
                if (R.HasOutBackEdges && RFS.IsTwoWay && RFS.HasOut(C) && RFS.HasOut(R))
                {
                    IF5(ref entry, C, F, T);
                    return true;
                }

                //if (IsBreakEdge(T))
                //{
                //}

                //    IF
                //    /\
                //   L  R
                //  /    \
                // LFS   RFS

                //if (CollapseNext(ref entry, C))
                //    return true;
                if (CollapseNext(ref entry, L, null))
                    return true;
                if (CollapseNext(ref entry, R, null))
                    return true;

                int rin = R.InCount;
                int lin = L.InCount;
                if (rin == 1 && lin == 1)
                {
                    var IF = new IfNode(C, L, R);
                    ReplaceSuccessor(C, IF, null);
                    Goto(L.FirstOut);
                    //L.FirstOut.From = IF;
                    R.FirstOut.From = IF;
                    ReplaceNode(ref entry, C, IF);
                    return true;
                }

                if (lin == 1)
                {
                    IF1(ref entry, C, L, null, L.FirstOut, true);
                }
                else
                {
                    //Debug.Assert(rin == 1);
                    IF1(ref entry, C, R, null, R.FirstOut, false);
                }

                return true;
            }
            return false;
        }
        #endregion

        #region CollapseTwoWay
        private bool CollapseTwoWay(ref Node entry, Node node)
        {
            if (node.IsTwoWay)
            {
                var F = node.FalseEdge;
                var T = node.TrueEdge;
                
                if (CollapseWhile(ref entry, node, F, T))
                    return true;

                if (CollapseDoWhile(ref entry, node, F, T))
                    return true;

                if (CollapseIf(ref entry, node, F, T))
                    return true;
            }

            return false;
        }
        #endregion

        #region CollapseWhile
        private bool CollapseWhile(ref Node entry, Node C, Edge F, Edge T)
        {
            if (F == null || T == null) return false;
            if (!C.HasInBackEdges) return false;

            var L = F.To;
            var R = T.To;

            if (L.IsTwoWay)
            {
                //    C
                //   / \
                //  L - R is exit from loop
                var e = L.FindOut(R);
                if (e != null && !C.IsReachable(R, true))
                {
                    IF2(ref entry, C, F, T, e);
                    return true;
                }
                if (CollapseSingleBranch(ref entry, L, false))
                    return true;
            }

            if (R.IsOneWay)
            {
                var RFS = R.FirstSuccessor;
                // L <- C -> R -> C
                if (RFS == C && !L.HasOut(R))
                {
#if DEBUG
                    if (CLIDebug.BreakWhileLoops && CLIDebug.Filter)
                        Debugger.Break();
#endif
                    var loop = new LoopNode(LoopType.PreTested, C);
                    loop.Add(R);
                    ReplaceSuccessor(C, loop, R);
                    F.From = loop;
                    ReplaceNode(ref entry, C, loop);
                    return true;
                }

                // L <- C -> R -> RFS -> C
                if (RFS.IsOneWay && RFS.FirstSuccessor == C)
                {
#if DEBUG
                    if (CLIDebug.BreakWhileLoops && CLIDebug.Filter)
                        Debugger.Break();
#endif
                    var loop = new LoopNode(LoopType.PreTested, C);
                    loop.Add(R);
                    loop.Add(RFS);
                    ReplaceSuccessor(C, loop, RFS);
                    F.From = loop;
                    ReplaceNode(ref entry, C, loop);
                    return true;
                }

                //continue?
                // RFS <- L <- C -> R -> RFS -> C
                if (RFS.IsTwoWay && L.IsOneWay)
                {
                    var LFS = L.FirstSuccessor;
                    if (LFS == RFS)
                    {
                        var e = RFS.FindOut(C);
                        if (e != null && e.IsBack)
                        {
                            //TODO: Add continue statement
                            var IF = new IfNode(C, L);
                            C.IsLabeled = true;
                            L.Goto = C;
                            ReplaceSuccessor(C, IF, null);
                            T.From = IF;
                            ReplaceNode(ref entry, C, IF);
                            return true;
                        }
                    }
                }
            }
            else if (L.IsOneWay)
            {
                var LFS = L.FirstSuccessor;
                // C <- L <- C -> R
                if (LFS == C)
                {
#if DEBUG
                    if (CLIDebug.BreakWhileLoops && CLIDebug.Filter)
                        Debugger.Break();
#endif
                    var IF = new IfNode(C, L);
                    var loop = new LoopNode(LoopType.PreTested, IF);
                    ReplaceSuccessor(C, loop, R);
                    T.From = loop;
                    ReplaceNode(ref entry, C, loop);
                    return true;
                }
            }

            if (CollapseDoWhile(ref entry, C, F, T))
                return true;

            if (CollapseNext(ref entry, C, null))
                return true;

            if (CollapseIf(ref entry, C, F, T))
                return true;

            return false;
        }
        #endregion

        #region CollapseDoWhile
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="C">Conditional node</param>
        /// <param name="F">False edge, exit from loop</param>
        /// <param name="T">True edge</param>
        /// <returns></returns>
        private bool CollapseDoWhile(ref Node entry, Node C, Edge F, Edge T)
        {
            if (!T.IsBack) return false;

            var L = F.To; //exit from loop
            var R = T.To;

            int cin = C.InCount;
            if (cin == 2)
            {
                var CI0 = C.FirstIn;
                var CI1 = CI0.NextIn;
                // C <-> C ? self cycle?
                if (CI0.From == C || CI1.From == C)
                {
#if DEBUG
                    if (CLIDebug.BreakDoWhileLoops && CLIDebug.Filter)
                        Debugger.Break();
#endif
                    var loop = new LoopNode(LoopType.PostTested, C);
                    ReplaceSuccessor(R, loop, null);
                    F.From = loop;
                    ReplaceNode(ref entry, C, loop);
                    return true;
                }
                return false;
            }
            if (cin >= 2) return false;

            if (R.IsOneWay)
            {
                var RFS = R.FirstSuccessor;
                // L <- C -> R -> RFS -> C
                if (RFS == C)
                {
#if DEBUG
                    if (CLIDebug.BreakDoWhileLoops && CLIDebug.Filter)
                        Debugger.Break();
#endif
                    var loop = new LoopNode(LoopType.PostTested, C);
                    loop.Add(R);
                    ReplaceSuccessor(R, loop, null);
                    F.From = loop;
                    ReplaceNode(ref entry, C, loop);
                    return true;
                }
                // L <- C -> R -> R.FS -> R.FS.FS -> C
                if (RFS.IsOneWay && RFS.FirstSuccessor == C)
                {
#if DEBUG
                    if (CLIDebug.BreakDoWhileLoops && CLIDebug.Filter)
                        Debugger.Break();
#endif
                    var loop = new LoopNode(LoopType.PostTested, C);
                    loop.Add(R);
                    loop.Add(RFS);
                    ReplaceSuccessor(R, loop, null);
                    F.From = loop;
                    ReplaceNode(ref entry, C, loop);
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region CollapseEndlessLoop
        private void CreateEndlessLoop(ref Node entry, Node node, Node except, params Node[] body)
        {
#if DEBUG
            if (CLIDebug.BreakEndlessLoops && CLIDebug.Filter)
                Debugger.Break();
#endif
            var loop = new LoopNode();
            loop.LoopType = LoopType.Endless;
            ReplaceSuccessor(node, loop, except);
            loop.Add(body);
            ReplaceNode(ref entry, node, loop);
        }

        private bool CollapseEndlessLoop(ref Node entry, Node C)
        {
            if (C.IsOneWay)
            {
                var FS = C.FirstSuccessor;
                if (C == FS)
                {
                    CreateEndlessLoop(ref entry, C, null, C);
                    return true;
                }
                if (FS.IsOneWay)
                {
                    var s2 = FS.FirstSuccessor;
                    if (s2 == C)
                    {
                        CreateEndlessLoop(ref entry, C, null, C, FS);
                        return true;
                    }
                    if (s2.IsOneWay)
                    {
                        var s3 = s2.FirstSuccessor;
                        if (s3 == C && s2.FirstOut.IsBack)
                        {
                            CreateEndlessLoop(ref entry, C, null, C, FS, s2);
                            return true;
                        }
                    }
                }
                else if (FS.FirstOut == null)
                {
                    foreach (var p in C.Predecessors)
                    {
                        if (p.IsOneWay && p.HasOutBackEdges)
                        {
                            CreateEndlessLoop(ref entry, C, p, C, FS, p);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        #endregion

        #region CollapseSwitch
        private readonly Hashtable _stackSwitch = new Hashtable();

        private bool CollapsePredecessors(ref Node entry, Node node)
        {
            foreach (var e in node.InEdges)
            {
                if (CollapseNode(ref entry, e.From))
                    return true;
            }
            return false;
        }

        private bool CollapseSuccessors(ref Node entry, Node node)
        {
            foreach (var e in node.OutEdges)
            {
                if (CollapseNode(ref entry, e.To))
                    return true;
            }
            return false;
        }

        private void CollapseSwitchCases(ref Node entry, Node node)
        {
            _stackSwitch[node] = node;
            var follow = node.FirstOut;
            
            var toExclude = new List<Node>();
            toExclude.Add(node);
            toExclude.Add(follow.To);
            for (var e = follow.NextOut; e != null; e = e.NextOut)
            {
                toExclude.Add(e.To);
            }

            for (var e = follow.NextOut; e != null; e = e.NextOut)
            {
                while (true)
                {
                    var caseNode = e.To;
                    if (CollapseNode(ref entry, caseNode))
                        continue;
                    if (CollapseSuccessors(ref entry, caseNode))
                        continue;
                    if (CollapseNext(ref entry, caseNode, toExclude))
                        continue;
#if DEBUG
                    if (caseNode.OutCount > 1)
                    {
                        if (CollapseNode(ref entry, caseNode))
                            continue;
                        throw new DecompileException();
                    }
#endif
                    break;
                }
            }

            _stackSwitch.Remove(node);
        }

        private bool CollapseSwitch(ref Node entry, Node node)
        {
            if (!node.IsSwitch) return false;
            if (_stackSwitch.Contains(node)) return false;
            
            CollapseSwitchCases(ref entry, node);

            if (node.Parent != null)
                throw new DecompileException();

            var sw = new SwitchNode(node);
            var list = new List<Edge>(node.OutEdges);
            var follow = list[0];
            
            int n = list.Count;
            for (int i = 1; i < n; ++i)
            {
                var e = list[i];
                var caseNode = e.To;

                if (caseNode.IsSwitch)
                    throw new DecompileException();

                if (caseNode == follow.To)
                {
                    e.Remove();
                    continue;
                }

                int j;
                for (j = i; j < n; ++j)
                {
                    e = list[j];
                    var case2 = e.To;
                    if (case2 != caseNode)
                    {
                        --j;
                        break;
                    }
                }
                if (j >= n) 
                    j = n - 1;

                for (int k = i; k <= j; ++k)
                {
                    e = list[k];
                    e.Remove();
                }

                int cin = caseNode.InCount;
                var c = sw.AddCase(caseNode, i - 1, j - 1, cin == 0);

                if (caseNode.IsOneWay) //case break
                {
                    e = caseNode.FirstOut;
                    //TODO: Optimize!!!
                    //bool resolve = true;
                    //Node to = e.To;
                    //int clen = to.CodeLength;
                    //if (to.IsBasicBlock && clen > 0)
                    //{
                    //    if (to.Code[clen - 1].IsReturn)
                    //    {
                    //        int toin = to.InCount;
                    //        to.PreventDetach = toin > 1;
                    //        c.Node = new SequenceNode(c.Node, to);
                    //        if (toin == 1)
                    //            e.Remove();
                    //        resolve = false;
                    //    }
                    //}
                    //if (resolve) Goto(e, cin == 0);
                    Goto(e, cin == 0);
                }
                else
                {
                    //TODO:
                    if (caseNode.FirstOut != null)
                        throw new DecompileException();
                }

                i = j;
            }

            ReplaceSuccessor(node, sw, null);
            follow.From = sw;
            ReplaceNode(ref entry, node, sw);

            return true;
        }
        #endregion

        #region ResolveGotos
        private void ResolveGotos(ref Node entry)
        {
            bool render = false;
            bool change = true;
            while (change)
            {
                change = false;
                foreach (var node in entry.Nodes)
                {
                    if (node.IsSwitch)
                    {
                        if (ResolveSwitchGotos(node))
                        {
                            change = true;
                            render = true;
                        }
                    }
                    else if (ResolveGoto(node))
                    {
                        change = true;
                        render = true;
                        break;
                    }
                }
            }
#if DEBUG
            if (render) Render(entry);
#endif
        }

        private static bool ResolveSwitchGotos(Node sw)
        {
            //Resolve gotos between switch cases
            bool change = false;
            for (var e = sw.FirstOut; e != null; e = e.NextOut)
            {
                var caseNode = e.To;
                foreach (var p in caseNode.InEdgeList)
                {
                    var from = p.From;
                    if (from != sw)
                    {
                        change = true;
                        //simple goto?
                        if (from.IsOneWay)
                        {
                            Goto(p);
                        }
                        else
                        {
                            //NOTE
                            //We can not remove the edge becase it can be a case of other switch
                            //Also for two way nodes we can not to destroy structure of graph for further
                            //normal processing and structure recognition
                            InsertGoto(p);
                        }
                    }
                }
            }
            return change;
        }

        private static bool IsSimpleExit(Node node)
        {
            if (!node.IsBasicBlock) return false;
            if (node.OutCount != 0) return false;
            int n = node.CodeLength;
            if (n == 0) return false;
            var li = node.Code[n - 1];
            if (li.IsReturn || li.IsThrow)
                return true;
            return false;
        }

        private static bool ResolveGoto(Node node)
        {
            if (!node.IsBasicBlock) return false;
            if (!node.IsOneWay) return false;

            var fin = node.FirstIn;
            if (fin == null) return false;

            int n = node.CodeLength;
            if (n == 0) return false;
            var e = node.FirstOut;

            if (node.Code[n - 1].IsUnconditionalBranch)
            {
                //Note: it can be end of loop
                if (e.IsBack) return false;

                var to = e.To;
                if (to.IsSwitchCase)
                {
                    Goto(e);
                    return true;
                }
            }

            //Note: forward edges also used in boolean expressions
            if (e.Type == EdgeType.Forward || e.Type == EdgeType.Cross)
            {
                //if (e.To.InCount > 1 && IsSimpleExit(e.To))
                //{
                //    Goto(e);
                //    return true;
                //}
                if (fin.From.IsSwitch)
                {
                    Goto(e);
                    return true;
                }
                if (fin.From.IsOneWay && InSwitchCase(node))
                {
                    Goto(e);
                    return true;
                }
            }

            //if (e.Type == EdgeType.Cross)
            //{
            //    Goto(e);
            //    return true;
            //}

            //TODO: Determine more cases
            //if (to.HasOutBackEdges)
            //{
            //    Goto(e);
            //    return true;
            //}
                        
            return false;
        }

        private static bool InSwitchCase(Node node)
        {
            var e = node.FirstIn;
            while (e != null)
            {
                if (e.From.IsSwitch)
                    return true;
                e = e.From.FirstIn;
            }
            return false;
        }
        #endregion

        #region Utils
        private static bool IsBreakEdge(Edge e)
        {
            var to = e.To;
            foreach (var i in to.InEdges)
            {
                if (i.From.HasInBackEdges) //Loop Header?
                {
                    if (!i.From.IsReachable(to, true))
                        return true;
                    return false;
                }
            }
            return false;
        }

        private static void Goto(Edge e, bool removeEdge)
        {
            e.To.IsLabeled = true;
            e.From.Goto = e.To;
            if (removeEdge)
                e.Remove();
        }

        private static void Goto(Edge e)
        {
            Goto(e, true);
        }

        private static Node CreateGoto(Node to)
        {
            to.IsLabeled = true;
            var from = new Node();
            from.Goto = to;
            return from;
        }

        private static void InsertGoto(Edge e)
        {
            var go = CreateGoto(e.To);
            e.To = go;
        }

        private static void ReplaceEntry(ref Node entry, Node oldNode, Node newNode, bool forceReplaceEntry)
        {
            if (oldNode == entry || forceReplaceEntry)
            {
                var next = entry.RemoveEntry();
                entry.IsEntry = false;
                entry = newNode;
                newNode.IsEntry = true;
                if (next != null)
                    entry.AppendEntry(next);
            }
        }

        private bool _collapseNewNode = true;

        private void ReplaceNode(ref Node entry, Node oldNode, Node newNode, bool forceReplaceEntry)
        {
            newNode.Index = oldNode.Index;
            newNode.OwnerBlock = oldNode.OwnerBlock;
            
            var type = newNode.NodeType;
            if (!(type == NodeType.Try || type == NodeType.Handler))
            {
                oldNode.Prepend(newNode);    
            }

            ReplaceEntry(ref entry, oldNode, newNode, forceReplaceEntry);
            newNode.DetachKids();

#if DEBUG
            newNode.Phase = phase + 1;
            newNode.IsNew = true;
            Render(entry);
            newNode.IsNew = false;
#endif

            if (type != NodeType.Try || type != NodeType.Handler)
            {
                if (type != NodeType.Sequence)
                {
                    if (CollapseSequence(ref entry, newNode))
                        return;
                }

                if (_collapseNewNode && _excluded == null)
                {
                    if (CollapsePredecessors(ref entry, newNode))
                        return;

                    if (CollapseNode(ref entry, newNode))
                        return;

                    if (CollapseSuccessors(ref entry, newNode))
                        return;
                }
            }
        }

        private void ReplaceNode(ref Node entry, Node oldNode, Node newNode)
        {
            ReplaceNode(ref entry, oldNode, newNode, false);
        }

        private static void ReplaceSuccessor(Node node, Node newNode, Node except)
        {
            var list = new List<Edge>(node.InEdges);
            foreach (var e in list)
            {
                if (e.From == except) continue;
                e.To = newNode;
            }
        }

        private static void ReplacePredecessor(Node node, Node newNode)
        {
            var list = new List<Edge>(node.OutEdges);
            foreach (var e in list)
            {
                e.From = newNode;
            }
        }

#if DEBUG
        internal int phase;
        private bool IsBreak
        {
            get { return CLIDebug.Filter && phase == CLIDebug.Phase; }
        }

        private void Render(Node node)
        {
            ++phase;
            if (IsBreak) Debugger.Break();
            if (CLIDebug.Filter || CLIDebug.VisualizeGraphStructuring)
            {
                CLIDebug.LogInfo("Phase {0}", phase);
                string path = DotService.MakePath(_body, phase.ToString());
                var list = node.GetGraphNodes();
                DotService.Write(list, path, false);
            }
        }
#endif
        #endregion
    }
}