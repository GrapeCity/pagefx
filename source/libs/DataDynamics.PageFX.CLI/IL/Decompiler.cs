using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using DataDynamics.PageFX.CLI.CFG;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.IL
{
    internal class Decompiler
    {
        private MethodBody _body;
        private readonly Stack<IExpression> _stack = new Stack<IExpression>();

        #region Decompile
        public void Decompile(MethodBody body)
        {
            _body = body;

#if DEBUG
            IMethod method = body.Method;
            CLIDebug.LogSeparator();
            CLIDebug.LogInfo("Decompiler started for method: {0}", method);

            CLIDebug.CheckFilter(method);
            if (CLIDebug.IsBreak) Debugger.Break();
            CLIDebug.DoCancel();

            int start = Environment.TickCount;
#endif

            //Build Control Flow Graph
            GraphBuilder builder = new GraphBuilder(body, true);
            Node entry = builder.Build();
            NodeList list = entry.GetGraphNodes();

#if DEBUG
            CLIDebug.VisualizeGraph(body, list);
#endif

            //Apply Structuring for CFG
            list = BuildTree(entry);

#if DEBUG
            CLIDebug.DoCancel();

            if (CLIDebug.Filter || CLIDebug.VisualizeGraphAfter)
                DotService.Write(list, DotService.MakePath(body, "after"), true);
            DotService.NameService = null;

            CLIDebug.LogInfo("Decompiling body...");
            CLIDebug.DoCancel();
#endif

            _Decompile(list);

#if DEBUG
            int end = Environment.TickCount;
            if (CLIDebug.DumpSrc || CLIDebug.Filter)
            {
                string dir = CFG.DirHelper.GetDirectory(body);
                string path = Path.Combine(dir, "src.cs");
                string text = method.ToString("lang = c#; mode = full", null);
                File.WriteAllText(path, text);
                CLIDebug.LogInfo("C# code is dumped");
            }
            CLIDebug.LogInfo("Decompiler succeded for {0}", method);
            CLIDebug.LogInfo("Decompile Time: {0}", (end - start) + "ms");
#endif
        }

        private NodeList BuildTree(Node entry)
        {
            StructuringEngine sng = new StructuringEngine();

#if DEBUG
            CLIDebug.LogInfo("Structuring flow graph...");
            int start = Environment.TickCount;
            try
            {
                if (CLIDebug.IsBreak)
                    Debugger.Break();
                entry = sng.Run(entry, _body);
                int end = Environment.TickCount;
                CLIDebug.LogInfo("Flow graph structured in {0} phases. Ellapsed time: {1}.",
                                 sng.phase,
                                 (end - start) + "ms");
            }
            catch
            {
                CLIDebug.SetLastError(_body);
                throw;
            }
#else
            entry = sng.Run(entry, _body);
#endif
            return entry.GetGraphNodes();
        }

        private void _Decompile(NodeList list)
        {
            StatementCollection block = new StatementCollection();
            //Declare local variables
            if (_body.LocalVariables != null)
            {
                foreach (IVariable v in _body.LocalVariables)
                {
                    block.Add(new VariableDeclarationStatement(v));
                }
            }

#if DEBUG
            try
            {
                if (CLIDebug.IsBreak)
                    Debugger.Break();
                DecompileBlock(block, list);
            }
            catch
            {
                CLIDebug.SetLastError(_body);
                throw;
            }
#else
            DecompileBlock(block, list);
#endif


            _body.Statements = block;
            ResolveGotos();
            RemoveReturn(block);
            ReduceLastIf(block);

            if (CLI.ResolveLabels)
            {
                LabelResolver lr = new LabelResolver(block, _label);
                lr.Run();
            }
        }

        private static ILabeledStatement GetLabel(Node node)
        {
            while (node != null)
            {
                if (node.Label != null)
                    return node.Label;
                node = node.Parent;
            }
            return null;
        }

        private void ResolveGotos()
        {
            //Resolve unresolved goto statements
            foreach (Node node in _gotos)
            {
                ILabeledStatement label = GetLabel(node.Goto);
                if (label == null)
                {
                    Debugger.Break();
                    throw new DecompileException();
                }
                node.GotoStatement.Label = node.Goto.Label;
            }
        }

        private void RemoveReturn(IList<IStatement> block)
        {
            //Remove redundant return statement for void methods
            if (!TypeService.IsVoid(_body.Method)) return;
            int i = block.Count - 1;
            if (i < 0) return;
            if (block[i] is IReturnStatement)
                block.RemoveAt(i);
        }

        private static void ReduceLastIf(IList<IStatement> block)
        {
            int n = block.Count;
            if (n == 0) return;
            IIfStatement st = block[n - 1] as IIfStatement;
            if (st == null) return;
            n = st.Then.Count;
            if (n == 0) return;
            int n2 = st.Else.Count;
            if (n2 == 0) return;
            IStatement ls = st.Then[n - 1];
            if (Equals(ls, st.Else[n2-1]))
            {
                if (ls is IThrowExceptionStatement)
                {
                    st.Then.RemoveAt(n - 1);
                    st.Else.RemoveAt(n2 - 1);
                    block.Add(ls);
                    return;
                }
                IReturnStatement ret = ls as IReturnStatement;
                if (ret != null)
                {
                    st.Then.RemoveAt(n - 1);
                    st.Else.RemoveAt(n2 - 1);
                    if (ret.Expression != null)
                        block.Add(ls);
                    return;
                }
            }
        }
        #endregion

        #region DecompileBlock
        private void DecompileBlock(ICollection<IStatement> block, IEnumerable<Node> nodes)
        {
            foreach (Node node in nodes)
            {
                ICodeNode[] code = DecompileNode(node, null);
                Add(block, code);
            }
        }

        private static void Add(ICollection<IStatement> block, ICodeNode node)
        {
            if (node == null)
                throw new ArgumentNullException();
            IStatement st = node as IStatement;
            if (st == null)
            {
                Debugger.Break();
                throw new InvalidOperationException();
            }
            block.Add(st);
        }

        private static void Add(ICollection<IStatement> block, IEnumerable<ICodeNode> code)
        {
            if (code != null)
            {
                foreach (ICodeNode node in code)
                {
                    Add(block, node);
                }
            }
        }
        #endregion

        #region DecompileNode
        private ICodeNode[] DecompileNode(Node node, Node parent)
        {
#if DEBUG
            CLIDebug.DoCancel();
#endif
            switch (node.NodeType)
            {
                case NodeType.BasicBlock:
                    return DecompileBasicBlock(node, parent);

                case NodeType.Sequence:
                    return DecompileSequence((SequenceNode)node);

                case NodeType.If:
                    {
                        ICodeNode[] code = DecompileIf((IfNode)node);
                        return LabelGoto(node, code);
                    }

                case NodeType.Loop:
                    {
                        IStatement st = DecompileLoop((LoopNode)node);
                        return Label(node, st);
                    }

                case NodeType.Switch:
                    {
                        ICodeNode[] code = DecompileSwitch((SwitchNode)node);
                        CheckLabel(node, code);
                        return code;
                    }

                case NodeType.Try:
                    {
                       IStatement st = DecompileTry((TryNode)node);
                       return Label(node, st);
                    }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion

        #region DecompileBasicBlock
        private ICodeNode[] DecompileBasicBlock(Node node, Node parent)
        {
            ICodeNode[] code = _DecompileBasicBlock(node, parent);
            return LabelGoto(node, code);
        }

        private InstructionDecompiler _idec;

        private void InitInstructionDecompiler(Node node, Node parent)
        {
            if (_idec == null)
                _idec = new InstructionDecompiler(_body, _stack);
            _idec.NegateBranches = IsNegateBranches(node, parent);
            _idec.CatchClause = _catchClause;
        }

        private ICodeNode[] _DecompileBasicBlock(Node node, Node parent)
        {
            InitInstructionDecompiler(node, parent);
            List<ICodeNode> code = new List<ICodeNode>();
            foreach (Instruction instruction in node.Code)
            {
                if (instruction.IsUnconditionalBranch)
                    break;

                ICodeNode codeNode = _idec.Decompile(instruction);
                if (codeNode == null) continue;

                IExpression e = codeNode as IExpression;
                if (e != null)
                {
                    _stack.Push(e);
                    continue;
                }

                IStatement st = (IStatement)codeNode;

                //optimize assinments (to use temporary variables generated by compiler)
                if (code.Count > 0)
                {
                    IStatement last = code[code.Count - 1] as IStatement;
                    if (last != null)
                    {
                        last = OptimizeAssignment(last, st);
                        if (last != null)
                        {
                            code.Add(last);
                            continue;
                        }
                    }
                }

                code.Add(st);
            }
            PopStack(code);
            return code.ToArray();
        }

        private static IBinaryExpression ToAssign(IStatement st)
        {
            IExpressionStatement est = st as IExpressionStatement;
            if (est == null) return null;
            IBinaryExpression be = est.Expression as IBinaryExpression;
            if (be == null) return null;
            if (be.Operator != BinaryOperator.Assign) return null;
            return be;
        }

        private static IStatement OptimizeAssignment(IStatement prev, IStatement cur)
        {
            IBinaryExpression a1 = ToAssign(prev);
            if (a1 == null) return null;
            IBinaryExpression a2 = ToAssign(cur);
            if (a2 == null) return null;
            IVariableReferenceExpression v = a1.Left as IVariableReferenceExpression;
            if (v == null) return null;
            if (!Equals(a1.Right, a2.Right)) return null;
            if (v.ResultType != a2.Right.ResultType) return null;
            a2 = new BinaryExpression(a2.Left, v, BinaryOperator.Assign);
            return new ExpressionStatement(a2);
        }

        //private static bool HasStatements(IEnumerable<ICodeNode> code)
        //{
        //    return CollectionUtils.Contains(code,
        //                                    delegate(ICodeNode node)
        //                                        {
        //                                            return node is IStatement;
        //                                        });
        //}

        private void PopStack(ICollection<ICodeNode> code)
        {
            if (_stack.Count > 0)
            {
                //bool st = HasStatements(code);
                IExpression[] arr = _stack.ToArray();
                for (int i = arr.Length - 1; i >= 0; --i)
                {
                    //IExpression e = arr[i];
                    //if (st && ExpressionService.CanStatement(e))
                    //{
                    //    code.Add(new ExpressionStatement(e));
                    //}
                    //else
                    //{
                    //    code.Add(e);
                    //}
                    code.Add(arr[i]);
                }
                _stack.Clear();
            }
        }
        #endregion

        #region DecompileSequence
        private ICodeNode[] DecompileSequence(SequenceNode node)
        {
            ICodeNode[] code = DecompileSequence(node.Parent, node.Kids);
            CheckLabel(node, code);
            IGotoStatement go = Goto(node);
            return Combine(code, go);
        }

        private ICodeNode[] DecompileSequence(Node parent, IEnumerable<Node> nodes)
        {
            List<ICodeNode> list = new List<ICodeNode>();
            foreach (Node node in nodes)
            {
                ICodeNode[] code = DecompileNode(node, parent);
                int n = code.Length;
                for (int i = 0; i < n; ++i)
                {
                    IExpression e = code[i] as IExpression;
                    if (e != null)
                    {
                        _stack.Push(e);
                    }
                    else
                    {
                        list.Add(code[i]);
                    }
                }
            }
            PopStack(list);
            return list.ToArray();
        }
        #endregion

        #region DecompileIf
        private ICodeNode[] DecompileIf(IfNode node)
        {
            ICodeNode[] condCode = DecompileNode(node.Condition, node);
            int cn = condCode.Length;
            if (cn == 0) throw new DecompileException();
            Debug.Assert(_stack.Count == 0);

            ICodeNode[] trueCode = DecompileNode(node.True, node);
            //Note: True block can be empty
            int tn = trueCode.Length;
            Debug.Assert(_stack.Count == 0);

            ICodeNode[] falseCode = null;
            if (node.False != null)
            {
                falseCode = DecompileNode(node.False, node);
                Debug.Assert(_stack.Count == 0);
            }

            //TODO: Check boolean expressions
            if (tn > 0)
            {
                ICodeNode[] code = TryTernary(node, condCode, trueCode, falseCode);
                if (code != null) return code;

                code = TryBooleanExpression(node, condCode, trueCode, falseCode);
                if (code != null) return code;
            }

            IIfStatement If = condCode[0] as IIfStatement;
            if (If != null)
            {
                int n = If.Then.Count;
                IExpressionStatement s = If.Then[n - 1] as IExpressionStatement;
                if (s != null && BooleanAlgebra.IsBooleanExpression(s.Expression))
                {
                    IfStatement if2 = new IfStatement();
                    if2.Condition = s.Expression;
                    Add(if2.Then, trueCode);
                    If.Then[n - 1] = if2;
                    Add(If.Else, falseCode);
                    return new ICodeNode[] { If };
                }
            }

            return CreateIf(node, condCode, trueCode, falseCode);
        }

        private static void Add<T>(ICollection<T> collection, T[] arr, int offset, int count)
        {
            int i = offset;
            while (count > 0)
            {
                collection.Add(arr[i]);
                ++i;
                --count;
            }
        }

        private static void Add<T>(ICollection<T> collection, T[] arr, int count)
        {
            Add(collection, arr, 0, count);
        }

        private ICodeNode[] CreateIf(IfNode node, ICodeNode[] condCode, ICodeNode[] trueCode, ICodeNode[] falseCode, IExpression e)
        {
            int cn = condCode.Length;
            List<ICodeNode> code = new List<ICodeNode>();
            Add(code, condCode, cn - 1);
            if (trueCode != null && trueCode.Length > 1)
                Add(code, trueCode, trueCode.Length - 1);
            if (falseCode != null && falseCode.Length > 1)
                Add(code, falseCode, falseCode.Length - 1);
            code.Add(e);

            Node cond = node.Condition;
            if ((cond.IsLabeled && cond.Label == null)
                || (node.IsLabeled && node.Label == null))
            {
                IStatement fs = code[0] as IStatement;
                if (fs != null)
                {
                    cond.Label = Label(fs);
                    code[0] = cond.Label;
                }
                else
                {
                    //Delegate labeling to parent node
                    Node parent = node.Parent;
                    if (parent != null)
                    {
                        parent.IsLabeled = true;
                    }
                    else
                    {
                        Debugger.Break();
                        throw new InvalidOperationException();
                    }
                }
            }

            return code.ToArray();
        }

        private ICodeNode[] CreateIf(IfNode node, ICodeNode[] condCode, ICodeNode[] trueCode, ICodeNode[] falseCode)
        {
            //Note: Gotos already must be created
            //T = LabelGoto(node.True, T);
            //F = LabelGoto(node.False, F);

            int cn = condCode.Length;
            List<ICodeNode> result = new List<ICodeNode>();
            Add(result, condCode, cn - 1);
            
            IfStatement st = new IfStatement();
            st.Condition = BooleanAlgebra.ToBool(condCode[cn - 1] as IExpression);

            Node cond = node.Condition;
            if ((cond.IsLabeled && cond.Label == null)
                || (node.IsLabeled && node.Label == null))
            {
                if (result.Count > 0)
                {
                    IStatement fs = result[0] as IStatement;
                    if (fs != null)
                    {
                        cond.Label = Label(fs);
                        result[0] = cond.Label;
                    }
                    else
                    {
                        cond.Label = Label(st);
                        node.Label = cond.Label;
                    }
                }
                else
                {
                    cond.Label = Label(st);
                    node.Label = cond.Label;
                }
            }

            Add(st.Then, trueCode);
            Add(st.Else, falseCode);

            MergeIfs(st);
            InvertIf(st);
            
            if (node.Label != null)
            {
                result.Add(node.Label);
            }
            else
            {
                result.Add(st);
            }
            return result.ToArray();
        }

        //Converts of if(a) { if(b){} } to if (a && b) {}
        private static void MergeIfs(IIfStatement st)
        {
            if (st.Then.Count == 1 && st.Else.Count == 0)
            {
                IIfStatement if2 = st.Then[0] as IIfStatement;
                if (if2 != null && if2.Else.Count == 0)
                {
                    st.Condition = BooleanAlgebra.And(st.Condition, if2.Condition);
                    st.Then.Clear();
                    st.Then.AddRange(if2.Then);
                }
            }
        }

        private static void InvertIf(IIfStatement st)
        {
            if (st.Else.Count > 0)
            {
                IUnaryExpression ue = st.Condition as IUnaryExpression;
                if (ue != null && ue.Operator == UnaryOperator.BooleanNot)
                {
                    st.Condition = ue.Expression;
                    IStatementCollection temp = st.Then;
                    st.Then = st.Else;
                    st.Else = temp;
                }
            }
        }

        private static bool IsNegateBranches(Node node, Node parent)
        {
            IfNode p1 = parent as IfNode;
            if (p1 != null)
            {
                if (node == p1.Condition)
                    return p1.Negate;
                return false;
            }
            return false;
        }

        private ICodeNode[] TryTernary(IfNode node, ICodeNode[] condCode, ICodeNode[] trueCode, ICodeNode[] falseCode)
        {
            if (falseCode == null) return null;
            if (falseCode.Length == 0) return null;

            IExpression trueExp = trueCode[trueCode.Length - 1] as IExpression;
            if (trueExp == null) return null;

            IExpression condition = condCode[condCode.Length - 1] as IExpression;
            if (condition != null)
            {
                IExpression falseExp = falseCode[falseCode.Length - 1] as IExpression;
                if (falseExp == null)
                {
                    Debugger.Break();
                    throw new DecompileException();
                }
                IExpression e = CreateTernaryExpression(condition, trueExp, falseExp);
                return CreateIf(node, condCode, trueCode, falseCode, e);
            }
            return null;
        }

        private static IExpression CreateTernaryExpression(IExpression a, IExpression b, IExpression c)
        {
            ConditionExpression e = new ConditionExpression();
            e.Condition = a;
            //e.TrueExpression = BooleanAlgebra.ToBool(b);
            //e.FalseExpression = BooleanAlgebra.ToBool(c);
            e.TrueExpression = b;
            e.FalseExpression = c;
            IExpression result = BooleanAlgebra.Simplify(e);
            return result;
        }

        private ICodeNode[] TryBooleanExpression(IfNode node, ICodeNode[] condCode, ICodeNode[] trueCode, ICodeNode[] falseCode)
        {
            IExpression then = trueCode[trueCode.Length - 1] as IExpression;
            if (then == null) return null;

            IIfStatement f = condCode[0] as IIfStatement;
            if (f != null)
            {
                IExpressionStatement s = f.Then[f.Then.Count - 1] as IExpressionStatement;
                if (s == null)
                    throw new DecompileException();
                s.Expression = CreateBooleanExpression(node, s.Expression, then);
                return new ICodeNode[] {f};
            }
            else
            {
                IExpression condition = condCode[condCode.Length - 1] as IExpression;
                if (condition == null)
                    throw new DecompileException();
                IExpression e = CreateBooleanExpression(node, condition, then);
                return CreateIf(node, condCode, trueCode, falseCode, e);
            }
        }

        private static IExpression CreateBooleanExpression(IfNode node, IExpression left, IExpression right)
        {
            left = BooleanAlgebra.ToBool(left);
            right = BooleanAlgebra.ToBool(right);

            BinaryOperator op = node.And ?  BinaryOperator.BooleanAnd : BinaryOperator.BooleanOr;

            IExpression e = new BinaryExpression(left, right, op);

            IfNode parent = node.Parent as IfNode;
            if (parent != null && !parent.IsTernary 
                && parent.Condition == node && parent.Negate)
            {
                e = BooleanAlgebra.Not(e);
            }

            e = BooleanAlgebra.Simplify(e);

            return e;
        }
        #endregion

        #region DecompileLoop
        private IStatement DecompileLoop(LoopNode loop)
        {
            LoopType type = loop.LoopType;
            if (loop.Condition != null)
            {
                Debug.Assert(type == LoopType.PreTested || type == LoopType.PostTested);
                LoopStatement st = new LoopStatement();
                st.LoopType = type;

                IStatement result = st;
                if (loop.IsLabeled || loop.Condition.IsLabeled)
                {
                    ILabeledStatement label = Label(st);
                    loop.Condition.Label = label;
                    result = label;
                }

                ICodeNode[] code = DecompileNode(loop.Condition, loop);
                int n = code.Length;

                IIfStatement ifst = code[n - 1] as IIfStatement;
                if (ifst != null)
                {
                    if (ifst.Else.Count > 0)
                        throw new InvalidOperationException();
                    st.Condition = ifst.Condition;
                    st.Body.AddRange(ifst.Then);
                }
                else
                {
                    IExpression e = code[n - 1] as IExpression;
                    if (e == null)
                        throw new InvalidOperationException();
                    st.Condition = BooleanAlgebra.ToBool(e);
                }

                ICodeNode[] body = DecompileSequence(loop, loop.Body);
                Add(st.Body, body);

                for (int i = 0; i < n - 1; ++i)
                    Add(st.Body, code[i]);

                return result;
            }
            else
            {
                LoopStatement st = new LoopStatement();
                st.LoopType = type;

                IStatement result = st;
                if (loop.IsLabeled)
                {
                    ILabeledStatement label = Label(st);
                    loop.Condition.Label = label;
                    result = label;
                }

                ICodeNode[] body = DecompileSequence(loop, loop.Body);
                Add(st.Body, body);
                return result;
            }
        }
        #endregion

        #region DecompileSwitch
        private ICodeNode[] DecompileSwitch(SwitchNode node)
        {
            SwitchStatement st = new SwitchStatement();
            ICodeNode[] C = DecompileNode(node.Condition, node);
            _stack.Clear();

            int n = C.Length;
            ICodeNode[] code = new ICodeNode[n];
            for (int i = 0; i < n - 1; ++i)
            {
                code[i] = C[i];
            }
            st.Expression = C[n - 1] as IExpression;
            code[n - 1] = st;

            n = node.Cases.Count;
            for (int i = 0; i < n; ++i)
            {
                SwitchNode.Case c = node.Cases[i];
                ICodeNode[] kidCode = DecompileNode(c.Node, node);
                CheckLabel(c.Node, kidCode);

#if DEBUG
                if (kidCode.Length > 1 && kidCode[kidCode.Length - 1] is IGotoStatement
                    && kidCode[kidCode.Length - 2] is IExpression)
                {
                    Debugger.Break();
                    throw new DecompileException();
                }
#endif

                SwitchCase swc = new SwitchCase();
                swc.From = c.From;
                swc.To = c.To;
                Add(swc.Body, kidCode);
                st.Cases.Add(swc);
            }

            if (node.Condition.IsLabeled)
            {
                IStatement s = code[0] as IStatement;
                node.Condition.Label = Label(s);
                code[0] = node.Condition.Label;
            }

            return code;
        }
        #endregion

        #region DecompileTry
        private IVariable FindExceptionVariable(IType type)
        {
            foreach (IVariable v in _body.LocalVariables)
            {
                if (v.Type == type)
                    return v;
            }
            return null;
        }

        private ICatchClause _catchClause;

        private IStatement DecompileTry(TryNode node)
        {
            TryCatchStatement st = new TryCatchStatement();
            DecompileBlock(st.Try, node.Body);

            foreach (HandlerNode h in node.Handlers)
            {
                switch (h.Block.Type)
                {
                    case BlockType.Catch:
                        {
                            CatchBlock catchBlock = (CatchBlock)h.Block;
                            CatchClause catchClause = new CatchClause();
                            IType type = catchBlock.ExceptionType;
                            catchClause.ExceptionType = type;
                            if (type != SystemTypes.Object)
                            {
                                catchClause.Condition = new TypeReferenceExpression(type);
                            }
                            catchClause.Variable = FindExceptionVariable(type);
                            _catchClause = catchClause;
                            DecompileBlock(catchClause.Body, h.Body);
                            st.CatchClauses.Add(catchClause);
                            _catchClause = null;
                        }
                        break;

                    case BlockType.Filter:
                        {
                            throw new NotImplementedException();
                        }
                        break;

                    case BlockType.Finally:
                        {
                            DecompileBlock(st.Finally, h.Body);
                        }
                        break;

                    case BlockType.Fault:
                        {
                            DecompileBlock(st.Fault, h.Body);
                        }
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return st;
        }
        #endregion

        #region Labels & Goto
        private int _label;

        private ILabeledStatement Label(IStatement st)
        {
            if (st == null)
                throw new ArgumentNullException();
            ++_label;
            return new LabeledStatement("L" + _label, st);
        }

        private ICodeNode[] Label(Node node, IStatement st)
        {
            if (node.IsLabeled)
            {
                node.Label = Label(st);
                return new ICodeNode[] { node.Label };
            }
            return new ICodeNode[] { st };
        }

        private void CheckLabel(Node node, ICodeNode[] code)
        {
            if (node.IsLabeled && node.Label == null)
            {
                IStatement st = code[0] as IStatement;
                if (st != null)
                {
                    node.Label = Label(st);
                    code[0] = node.Label;
                }
            }
        }

        private readonly List<Node> _gotos = new List<Node>();

        private IGotoStatement Goto(Node node)
        {
            if (node.Goto != null)
            {
                if (node.GotoStatement == null)
                {
                    //NodeType nt = node.NodeType;
                    //if (nt == NodeType.BasicBlock)
                    //{
                    //    int cn = node.Code.Count;
                    //    if (cn == 0 || (cn == 1 && node.Code[0].Code == InstructionCode.Nop))
                    //    {
                    //        node.Goto = null;
                    //        return null;
                    //    }
                    //}

                    IGotoStatement go = new GotoStatement(node.Goto.Label);
                    node.GotoStatement = go;
                    if (go.Label == null)
                    {
                        _gotos.Add(node);
                    }
                }
                return node.GotoStatement;
            }
            return null;
        }

        private ICodeNode[] LabelGoto(Node node, ICodeNode[] code)
        {
            if (code == null) return code;
            CheckLabel(node, code);
            IGotoStatement go = Goto(node);
            return Combine(code, go);
        }
        #endregion

        #region Utils
        private static IExpression ToExpression(ICodeNode[] code)
        {
            int n = code.Length;
            if (n == 0) return null;
            if (n == 1)
            {
                return code[0] as IExpression;
            }
            //IExpression last = code[n - 1] as IExpression;
            //if (last != null)
            //{
            //    ExpressionCollection e = new ExpressionCollection();
            //    for (int i = 0; i < n - 1; ++i)
            //    {
            //        IExpressionStatement s = code[i] as IExpressionStatement;
            //        if (s == null)
            //            throw new DecompileException();
            //        e.Add(s.Expression);
            //    }
            //    e.Add(last);
            //    return e;
            //}
            return null;
        }

        private static ICodeNode[] Combine(ICodeNode[] code, ICodeNode node)
        {
            if (node != null)
            {
                int n = code.Length;
                ICodeNode[] result = new ICodeNode[n + 1];
                Array.Copy(code, result, n);
                result[n] = node;
                return result;
            }
            return code;
        }
        #endregion
    }
}