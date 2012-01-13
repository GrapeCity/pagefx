#if DEBUG
using System;
using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.CFG
{
    internal class NameService
    {
        private char letter = 'a';
        private int index;
        private int seqIndex;
        private int graphIndex;
        private int whileIndex;
        private int doWhileIndex;
        private int loopIndex;
        private int ifIndex;
        private int swIndex;
        private int tryIndex;
        private int handlerIndex;

        public void SetNames<T>(IEnumerable<T> list) 
            where T : Node
        {
            foreach (var node in list)
                node.IsVisited = false;

            foreach (var node in list)
                SetNames(node);
        }

        private void SetNames(Node node)
        {
            if (node.IsVisited) return;
            node.IsVisited = true;
            SetName(node);
            switch (node.NodeType)
            {
                case NodeType.Sequence:
                    {
                        var seq = (SequenceNode)node;
                        foreach (var kid in seq.Kids)
                        {
                            SetNames(kid);
                        }
                    }
                    break;

                case NodeType.Loop:
                    {
                        var loop = (LoopNode)node;
                        if (loop.Condition != null)
                            SetNames(loop.Condition);
                        foreach (var kid in loop.Body)
                        {
                            SetNames(kid);
                        }
                    }
                    break;

                case NodeType.If:
                    {
                        var ifNode = (IfNode)node;
                        SetNames(ifNode.Condition);
                        SetNames(ifNode.True);
                        if (ifNode.False != null)
                            SetNames(ifNode.False);
                    }
                    break;

                case NodeType.Switch:
                    {
                        var sw = (SwitchNode)node;
                        SetNames(sw.CaseNodes);
                    }
                    break;

                case NodeType.Try:
                    {
                        var t = (TryNode)node;
                        SetNames(t.Body);
                        SetNames(t.Handlers);
                    }
                    break;

            }
            foreach (var suc in node.Successors)
            {
                if (!suc.IsVisited)
                    SetNames(suc);
            }
        }

        private static void SetName(Node node, string prefix, ref int i)
        {
            node.Name = prefix + (i++);
        }

        public void SetName(Node node)
        {
            if (!string.IsNullOrEmpty(node.Name))
                return;

            switch (node.NodeType)
            {
                case NodeType.BasicBlock:
                    SetName(node, "B", ref index);
                    break;

                case NodeType.Graph:
                    SetName(node, "G", ref graphIndex);
                    break;

                case NodeType.Sequence:
                    SetName(node, "SEQ", ref seqIndex);
                    break;

                case NodeType.Loop:
                    {
                        var loop = (LoopNode)node;
                        var loopType = loop.LoopType;
                        if (loopType == LoopType.PreTested)
                            SetName(node, "While", ref whileIndex);
                        else if (loopType == LoopType.PostTested)
                            SetName(node, "DoWhile", ref doWhileIndex);
                        else
                            SetName(node, "Endless", ref loopIndex);
                    }
                    break;

                case NodeType.If:
                    SetName(node, "IF", ref ifIndex);
                    break;

                case NodeType.Switch:
                    SetName(node, "SW", ref swIndex);
                    break;

                case NodeType.Try:
                    SetName(node, "Try", ref tryIndex);
                    break;

                case NodeType.Handler:
                    {
                        var h = (HandlerNode)node;
                        SetName(node, h.Block.Type.ToString(), ref handlerIndex);
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
#endif