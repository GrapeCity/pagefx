using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DataDynamics.PageFX.CodeModel
{
    /// <summary>
    /// Component to compile method body statements to <see cref="IInstructionList"/> using given <see cref="IInstructionProvider"/>.
    /// </summary>
    public class InstructionCompiler : IInstructionCompiler
    {
        #region Fields
        private IInstructionProvider _provider;
        private InstructionList _list;
        private readonly ActionSelector _statements = new ActionSelector();
        private readonly ActionSelector _expressions = new ActionSelector();
        #endregion

        #region Constructors
        public InstructionCompiler()
        {
            AddStatement<IVariableDeclarationStatement>(CompileVariableDeclarationStatement);
            AddStatement<IExpressionStatement>(CompileExpressionStatement);
            AddStatement<IStatementCollection>(CompileStatementCollection);
            AddStatement<IReturnStatement>(CompileReturnStatement);
            AddStatement<IIfStatement>(CompileIfStatement);
            AddStatement<ILoopStatement>(CompileLoopStatement);
            AddStatement<ISwitchStatement>(CompileSwitchStatement);
            AddStatement<IGotoStatement>(CompileGotoStatement);
            AddStatement<ILabeledStatement>(CompileLabeledStatement);
            AddStatement<ITryCatchStatement>(CompileTryCatchStatement);
            AddStatement<IThrowExceptionStatement>(CompileThrowExceptionStatement);
            AddStatement<IBreakStatement>(CompileBreakStatement);
            AddStatement<IContinueStatement>(CompileContinueStatement);
            AddStatement<ICommentStatement>(CompileCommentStatement);
            AddStatement<IDebuggerBreakStatement>(CompileDebuggerBreakStatement);
            
            AddExpression<IConstantExpression>(CompileConstantExpression);

            AddExpression<IVariableReferenceExpression>(CompileVariableReferenceExpression);
            AddExpression<IArgumentReferenceExpression>(CompileArgumentReferenceExpression);
            AddExpression<IFieldReferenceExpression>(CompileFieldReferenceExpression);
            AddExpression<IThisReferenceExpression>(CompileThisReferenceExpression);
            AddExpression<IBaseReferenceExpression>(CompileBaseReferenceExpression);
            AddExpression<IPropertyReferenceExpression>(CompilePropertyReferenceExpression);

            AddExpression<IIndexerExpression>(CompileIndexerExpression);
            AddExpression<IMethodReferenceExpression>(CompileMethodReferenceExpression);
            AddExpression<ITypeReferenceExpression>(CompileTypeReferenceExpression);
            
            AddExpression<IBinaryExpression>(CompileBinaryExpression);
            AddExpression<IUnaryExpression>(CompileUnaryExpression);

            AddExpression<ICallExpression>(CompileCallExpression);

            AddExpression<INewObjectExpression>(CompileNewObjectExpression);
            AddExpression<INewArrayExpression>(CompileNewArrayExpression);

            AddExpression<IArrayIndexerExpression>(CompileArrayIndexerExpression);
            AddExpression<IArrayLengthExpression>(CompileArrayLengthExpression);

            AddExpression<ICastExpression>(CompileCastExpression);
            AddExpression<IBoxExpression>(CompileBoxExpression);
            AddExpression<IUnboxExpression>(ComileUnboxExpression);

            AddExpression<IConditionExpression>(CompileConditionExpression);
            AddExpression<IExpressionCollection>(CompileExpressionCollection);

            AddExpression<INewDelegateExpression>(CompileNewDelegateExpression);
            AddExpression<IDelegateInvokeExpression>(CompileDelegateInvokeExpression);

            AddExpression<ITypeOfExpression>(CompileTypeOfExpression);
            AddExpression<ISizeOfExpression>(CompileSizeOfExpression);
        }
        #endregion

        #region IInstructionCompiler Members
        /// <summary>
        /// Gets the method being compiled
        /// </summary>
        public IMethod Method
        {
             get { return _method; }
        }
        private IMethod _method;

#if DEBUG
        private string SourceCode
        {
            get
            {
                if (_sourceCode == null)
                    _sourceCode = _method.ToString("lang = c#; mode = full", null);
                return _sourceCode;
            }
        }
        private string _sourceCode;
#endif

        public IInstructionList Compile(IMethod method, IInstructionProvider provider)
        {
            if (method.Body == null)
                throw new InvalidOperationException();

            _method = method;
            _provider = provider;
            _list = new InstructionList();

            Emit(_provider.BeginCompilation());

            IStatementCollection body = method.Body.Statements;

            //if (method.DeclaringType.FullName == "System.IO.TextWriter" && method.Name == "Write")
            //if (method.DeclaringType.FullName == "System.String" && method.Name == "ToCharArray")
            //    Debugger.Break();

            CompileStatementCollection(body);

            //Add excplicitly return void
            if (TypeService.IsVoid(method))
            {
                int i = body.Count - 1;
                if (i < 0 || !(body[i] is IReturnStatement))
                {
                    Emit(_provider.Return(true));
                }
            }

            Emit(_provider.EndCompilation());

            return _list;
        }

        private void Emit(IInstruction instruction)
        {
            if (instruction != null)
            {
                instruction.Index = _list.Count;
                _list.Add(instruction);
            }
        }

        private void Emit(params IInstruction[] collection)
        {
            if (collection != null)
            {
                foreach (IInstruction instruction in collection)
                {
                    Emit(instruction);
                }
            }
        }
        #endregion

        #region Statements
        private void AddStatement<T>(Action<T> method)
            where T : class, IStatement
        {
            _statements.Add(method);
        }

        private void CompileStatement(IStatement statement)
        {
            if (!_statements.Run(statement))
                throw new NotImplementedException();
        }

        private void CompileStatementCollection(IStatementCollection statements)
        {
            if (statements == null) return;
            foreach (IStatement statement in statements)
            {
                CompileStatement(statement);
            }
        }

        private void CompileVariableDeclarationStatement(IVariableDeclarationStatement s)
        {
            IInstruction[] i = _provider.DeclareVariable(s.Variable);
            Emit(i);
        }

        private void CompileExpressionStatement(IExpressionStatement s)
        {
            CompileExpression(s.Expression);
        }

        private void CompileReturnStatement(IReturnStatement s)
        {
            if (s.Expression == null)
            {
                Emit(_provider.Return(true));
            }
            else
            {
                CompileExpression(s.Expression);
                Emit(_provider.Return(false));
            }
        }

        private BranchOperator CompileCondition(IExpression condition, bool negate)
        {
            IBinaryExpression bin = condition as IBinaryExpression;
            if (bin != null)
            {
                BranchOperator op = ExpressionService.ToBranchOperator(bin.Operator);
                if (op != BranchOperator.None)
                {
                    if (_provider.SupportBranchOperator(op))
                    {
                        if (negate)
                            op = ExpressionService.Negate(op);
                        CompileExpression(bin.Left);
                        CompileExpression(bin.Right);
                        return op;
                    }
                }
            }
            IUnaryExpression un = condition as IUnaryExpression;
            if (un != null)
            {
                if (un.Operator == UnaryOperator.BooleanNot)
                {
                    CompileExpression(un.Expression);
                    if (negate) return BranchOperator.True;
                    return BranchOperator.False;
                }
            }

            //if (true)?
            if (condition == null)
            {
                LoadConstant(true);
            }
            else
            {
                CompileExpression(condition);
            }
            return BranchOperator.False;
        }

        private void CompileIfStatement(IIfStatement s)
        {
            //TODO:
            BranchOperator op = CompileCondition(s.Condition, true);
            IInstruction[] skipThen = _provider.Branch(op, null, null);
            Emit(skipThen);
            CompileStatementCollection(s.Then);

            IInstruction skipElse = null;
            bool hasElse = s.Else.Count > 0;
            if (hasElse)
            {
                //Note: after return instruction we must avoid emit of skip else jump
                IInstruction last = _list[_list.Count - 1];
                if (!(last.IsReturn || last.IsThrow || last.IsUnconditionalBranch))
                {
                    skipElse = _provider.Branch();
                    Emit(skipElse);   
                }
            }

            _provider.SetBranchTarget(skipThen[skipThen.Length - 1], _list.Count);
            if (hasElse)
            {
                CompileStatementCollection(s.Else);
                if (skipElse != null)
                    _provider.SetBranchTarget(skipElse, _list.Count);
            }
        }

        private void CompileLoopStatement(ILoopStatement s)
        {
            //TODO:
            int loopStart = _list.Count;
            AddLabel(false);
            LoopType type = s.LoopType;
            if (type == LoopType.PostTested)
            {
                CompileStatementCollection(s.Body);
                BranchOperator op = CompileCondition(s.Condition, false);
                IInstruction[] gotoStart = _provider.Branch(op, null, null);
                Emit(gotoStart);
                _provider.SetBranchTarget(gotoStart[gotoStart.Length - 1], loopStart);
            }
            else
            {
                if (type == LoopType.Endless)
                {
                    CompileStatementCollection(s.Body);
                    IInstruction gotoStart = _provider.Branch(loopStart);
                    Emit(gotoStart);
                }
                else
                {
                    BranchOperator op = CompileCondition(s.Condition, true);
                    IInstruction[] gotoEnd = _provider.Branch(op, null, null);
                    Emit(gotoEnd);
                    CompileStatementCollection(s.Body);
                    IInstruction gotoStart = _provider.Branch(loopStart);
                    Emit(gotoStart);
                    _provider.SetBranchTarget(gotoEnd[gotoEnd.Length - 1], _list.Count);
                }
            }
        }

        private void CompileSwitchStatement(ISwitchStatement s)
        {
            if (_provider.IsSwitchSupported)
            {
                CompileExpression(s.Expression);

                int n = s.Cases.Count;
                int lastIndex = s.Cases[n - 1].To;
                int n2 = lastIndex + 1;

                IInstruction sw = _provider.Switch(n2);
                Emit(sw);
                
                int[] targets = new int[n2];
                for (int i = 0; i < n; ++i)
                {
                    ISwitchCase swc = s.Cases[i];
                    int target = _list.Count;
                    for (int k = swc.From; k <= swc.To; ++k)
                        targets[k] = target;
                    AddLabel(false);
                    CompileStatementCollection(swc.Body);
                }

                int defaultTarget = _list.Count;
                AddLabel(false);

                _provider.SetCaseTargets(sw, targets, defaultTarget);
            }
            else
            {
                //TODO: Implement using usual conditional branches
            }
        }

        private void AddLabel(bool check)
        {
            if (check)
            {
                int n = _list.Count;
                if (n > 0 && _provider.IsLabel(_list[n - 1]))
                    return;
            }
            Emit(_provider.Label());
        }

        private void CompileLabeledStatement(ILabeledStatement s)
        {
            if (s.Statement == null)
                throw new InvalidOperationException();

            int labelTarget = _list.Count;
            AddLabel(false);

            //update previous gotos to this statement
            if (s.Tag != null)
            {
                List<IInstruction> gotos = s.Tag as List<IInstruction>;
                if (gotos == null)
                {
                    Debugger.Break();
                    throw new InvalidOperationException();
                }
                foreach (IInstruction br in gotos)
                {
                    _provider.SetBranchTarget(br, labelTarget);    
                }
            }

            s.Tag = labelTarget;
            CompileStatement(s.Statement);
        }

        private void CompileGotoStatement(IGotoStatement s)
        {
            ILabeledStatement label = s.Label;
            if (label == null)
            {
                return;
            }

            IInstruction br = _provider.Branch();
            Emit(br);

            object tag = label.Tag;
            if (tag != null)
            {
                List<IInstruction> gotos = tag as List<IInstruction>;
                if (gotos != null)
                {
                    gotos.Add(br);
                }
                else
                {
                    int target = (int)tag;
                    _provider.SetBranchTarget(br, target);
                }
            }
            else
            {
                List<IInstruction> gotos = new List<IInstruction>();
                gotos.Add(br);
                label.Tag = gotos;
            }
        }

        private int _tryDepth;

        private void CompileTryCatchStatement(ITryCatchStatement s)
        {
            //begin try
            IInstruction[] code = _provider.BeginTry();
            IInstruction tryBegin = code[0];
            Emit(code);
            _tryDepth++;

            CompileStatementCollection(s.Try);

            //end try
            IInstruction jump;
            code = _provider.EndTry(true, out jump);
            IInstruction tryEnd = code[0];
            Emit(code);

            List<IInstruction> gotoTryEnd = new List<IInstruction>();
            if (jump != null)
                gotoTryEnd.Add(jump);
            _tryDepth--;

            int n = s.CatchClauses.Count;
            for (int i = 0; i < n; ++i)
            {
                ICatchClause clause = s.CatchClauses[i] as ICatchClause;
                if (clause == null)
                    throw new InvalidOperationException();

                int var = -1;
                if (clause.Variable != null)
                    var = clause.Variable.Index;

                code = _provider.BeginCatch(clause.ExceptionType, var, tryBegin, tryEnd, _tryDepth);
                Emit(code);

                CompileStatementCollection(clause.Body);

                int catchEnd = _list.Count;
                code = _provider.EndCatch(i == n - 1, true, out jump);
                Emit(code);
                if (jump != null)
                    gotoTryEnd.Add(jump);
            }
            SetTargets(gotoTryEnd);
            if (s.Fault.Count > 0)
            {
                code = _provider.BeginFault(tryBegin, tryEnd, _tryDepth);
                Emit(code);
                CompileStatementCollection(s.Fault);
                code = _provider.EndFault();
                Emit(code);
            }
            if (s.Finally.Count > 0)
            {
                code = _provider.BeginFinally(tryBegin, tryEnd, _tryDepth);
                Emit(code);
                CompileStatementCollection(s.Finally);
                code = _provider.EndFinally();
                Emit(code);
            }
        }

        private void CompileThrowExceptionStatement(IThrowExceptionStatement e)
        {
            if (e.Expression != null)
            {
                CompileExpression(e.Expression);
                Emit(_provider.Throw());
            }
            else
            {
                Emit(_provider.Rethrow());
            }
        }

        private static void CompileBreakStatement(IBreakStatement s)
        {
        }

        private static void CompileContinueStatement(IContinueStatement s)
        {
        }

        private static void CompileCommentStatement(ICommentStatement s)
        {
        }

        private void CompileDebuggerBreakStatement(IDebuggerBreakStatement s)
        {
            IInstruction i = _provider.DebuggerBreak();
            if (i != null)
                Emit(i);
        }
        #endregion

        #region Expressions
        #region IExpression, IExpressionCollection
        private void AddExpression<T>(Action<T> method)
            where T : class, IExpression
        {
            _expressions.Add(method);
        }

        private readonly Stack<IExpression> _expStack = new Stack<IExpression>();

        private void CompileExpression(IExpression e)
        {
            _expStack.Push(e);
            if (!_expressions.Run(e))
                throw new NotImplementedException();
            _expStack.Pop();
        }

        private void CompileExpressionCollection(IExpressionCollection e)
        {
            foreach (IExpression expression in e)
            {
                CompileExpression(expression);
            }
        }
        #endregion

        #region AssignInfo
        private enum AssignTarget
        {
            Unknown,
            Variable,
            Argument,
            Field,
            Property,
            Indexer,
            ArrayItem
        }

        private class AssignInfo
        {
            public bool Enabled;
            public int TargetIndex;
            public AssignTarget TargetType;
            public IField Field;
            public IPropertyReferenceExpression Property;
            public IType ElementType;
            public IVariable Variable;
            public IParameter Argument;

            public IType Type
            {
                get
                {
                    switch (TargetType)
                    {
                        case AssignTarget.Variable:
                            return Variable.Type;
                        case AssignTarget.Argument:
                            return Argument.Type;
                        case AssignTarget.Field:
                            return Field.Type;
                        case AssignTarget.Property:
                        case AssignTarget.Indexer:
                            return Property.Property.Type;
                        case AssignTarget.ArrayItem:
                            return ElementType;
                    }
                    return null;
                }
            }
        }
        private readonly AssignInfo _assign = new AssignInfo();
        #endregion

        #region Constant
        private void LoadConstant(object value)
        {
            Emit(_provider.LoadConstant(value));
        }

        private void CompileConstantExpression(IConstantExpression e)
        {
            LoadConstant(e.Value);
        }
        #endregion

        #region References
        private void CompileVariableReferenceExpression(IVariableReferenceExpression e)
        {
            IVariable v = e.Variable;
            int index = v.Index;
            if (_assign.Enabled)
            {
                _assign.TargetIndex = index;
                _assign.TargetType = AssignTarget.Variable;
                _assign.Variable = e.Variable;
            }
            else
            {
                GetVar(index, IsByRef, v.Type);
            }
        }

        private void CompileArgumentReferenceExpression(IArgumentReferenceExpression e)
        {
            IParameter arg = e.Argument;
            int index = arg.Index - 1;
            if (_assign.Enabled)
            {
                _assign.TargetIndex = index;
                _assign.TargetType = AssignTarget.Argument;
                _assign.Argument = arg;
            }
            else
            {
                GetArg(index, IsByRef, arg.Type);
            }
        }

        private void CompileFieldReferenceExpression(IFieldReferenceExpression e)
        {
            bool byref = IsByRef;

            bool assign = _assign.Enabled;
            _assign.Enabled = false;
            CompileExpression(e.Target);
            _assign.Enabled = assign;

            IField field = e.Field;
            if (_assign.Enabled)
            {
                _assign.TargetType = AssignTarget.Field;
                _assign.Field = field;
            }
            else
            {
                GetField(field, byref);
            }
        }

        private void CompilePropertyReferenceExpression(IPropertyReferenceExpression e)
        {
            //Note: A property or indexer may not be passed as an out or ref parameter
            //bool byref = IsByRef;

            IProperty prop = e.Property;

            //bool assign = _assign.Enabled;
            //IMethod m = _assign.Enabled ? prop.Setter : prop.Getter;
            //_assign.Enabled = false;
            //if (_provider.CanCompileTarget(m, false))
            //    CompileExpression(e.Target);
            //_assign.Enabled = assign;

            bool assign = _assign.Enabled;
            _assign.Enabled = false;
            CompileExpression(e.Target);
            _assign.Enabled = assign;

            if (_assign.Enabled)
            {
                _assign.TargetType = AssignTarget.Property;
                _assign.Property = e;
            }
            else
            {
                GetProperty(e);
            }
        }

        private void CompileIndexerExpression(IIndexerExpression e)
        {
            //Note: A property or indexer may not be passed as an out or ref parameter
            //bool byref = IsByRef;

            bool assign = _assign.Enabled;
            _assign.Enabled = false;
            CompileExpression(e.Property.Target);
            CompileExpressionCollection(e.Index);
            _assign.Enabled = assign;

            if (_assign.Enabled)
            {
                _assign.TargetType = AssignTarget.Indexer;
                _assign.Property = e.Property;
            }
            else
            {
                GetProperty(e.Property);
            }
        }

        private void CompileArrayIndexerExpression(IArrayIndexerExpression e)
        {
            bool byref = IsByRef;

            bool assign = _assign.Enabled;
            _assign.Enabled = false;
            CompileExpression(e.Array);
            CompileExpressionCollection(e.Index);
            _assign.Enabled = assign;

            IType elemType = e.ResultType;
            if (_assign.Enabled)
            {
                _assign.TargetType = AssignTarget.ArrayItem;
                _assign.ElementType = elemType;
            }
            else
            {
                GetArrayItem(elemType, byref);
            }
        }

        private void CompileArrayLengthExpression(IArrayLengthExpression e)
        {
            CompileExpression(e.Array);
            Emit(_provider.GetArrayLength());
        }

        private void CompileMethodReferenceExpression(IMethodReferenceExpression e)
        {
            CompileExpression(e.Target);
        }

        private void CompileTypeReferenceExpression(ITypeReferenceExpression e)
        {
            Emit(_provider.LoadStaticInstance(e.Type));
        }

        private void CompileThisReferenceExpression(IThisReferenceExpression e)
        {
            Emit(_provider.LoadThis(IsByRef));
        }

        private void CompileBaseReferenceExpression(IBaseReferenceExpression e)
        {
            Emit(_provider.LoadBase());
        }
        #endregion

        #region CompileBooleanExpression
        private static bool IsBooleanOperator(BinaryOperator op)
        {
            return op == BinaryOperator.BooleanAnd || op == BinaryOperator.BooleanOr;
        }

        private class BoolContext
        {
            public IInstruction[] False;
            public IInstruction[] True;
            public readonly List<IInstruction> GotoEnd = new List<IInstruction>();
            public readonly List<IInstruction> GotoFalse = new List<IInstruction>();
            public readonly List<IInstruction> GotoTrue = new List<IInstruction>();
        }

        private class BoolNode
        {
            public BoolNode Parent;
            public BoolNode Left;
            public BoolNode Right;
            public bool IsAnd;
            public IExpression LeftExpression;
            public IExpression RightExpression;
            public readonly List<IInstruction> GotoRight = new List<IInstruction>();
            public readonly List<IInstruction> GotoEnd = new List<IInstruction>();

            public override string ToString()
            {
                StringBuilder s = new StringBuilder();
                s.Append("(");

                if (Left != null) s.Append(Left);
                else s.Append(LeftExpression);

                s.Append(IsAnd ? " && " : " || ");

                if (Right != null) s.Append(Right);
                else s.Append(RightExpression);

                s.Append(")");
                return s.ToString();
            }
        }

        private void BuildBoolNode(IBinaryExpression e, BoolNode node)
        {
            node.IsAnd = e.Operator == BinaryOperator.BooleanAnd;

            IBinaryExpression be = e.Left as IBinaryExpression;
            if (be != null && IsBooleanOperator(be.Operator))
            {
                BoolNode left = new BoolNode();
                left.Parent = node;
                BuildBoolNode(be, left);
                node.Left = left;
            }
            else
            {
                node.LeftExpression = e.Left;
            }

            be = e.Right as IBinaryExpression;
            if (be != null && IsBooleanOperator(be.Operator))
            {
                BoolNode right = new BoolNode();
                right.Parent = node;
                BuildBoolNode(be, right);
                node.Right = right;
            }
            else
            {
                node.RightExpression = e.Right;
            }
        }

        private void CompileBooleanExpression(BoolNode node, BoolContext ctx)
        {
            //TODO:
            
            bool isAnd = node.IsAnd;

            if (node.Left != null)
                CompileBooleanExpression(node.Left, ctx);
            else
                CompileExpression(node.LeftExpression);
            
            //branch to skip right expression
            BranchOperator op = isAnd ? BranchOperator.False : BranchOperator.True;
            IInstruction[] br = _provider.Branch(op, null, null);
            Emit(br);

            SetTargets(node.GotoRight);
            
            if (node.Right != null)
                CompileBooleanExpression(node.Right, ctx);
            else
                CompileExpression(node.RightExpression);

            //skip const
            BoolNode parent = node.Parent;
            if (parent == null)
            {
                IInstruction br2 = _provider.Branch();
                Emit(br2);
                ctx.GotoEnd.Add(br2);
            }
            
            SetTargets(node.GotoEnd);

            if (parent == null)
            {
                if (isAnd) ctx.GotoFalse.Add(br[br.Length - 1]);
                else ctx.GotoTrue.Add(br[br.Length - 1]);
            }
            else
            {
                if (node == parent.Left)
                {
                    BoolNode cur = node;
                    while (parent != null)
                    {
                        if (!(parent.IsAnd == cur.IsAnd))
                        {
                            break;
                        }
                        cur = parent;
                        parent = cur.Parent;
                    }
                    if (parent == null)
                    {
                        if (isAnd) ctx.GotoFalse.Add(br[br.Length - 1]);
                        else ctx.GotoTrue.Add(br[br.Length - 1]);
                    }
                    else
                    {
                        if (cur == parent.Left)
                        {
                            parent.GotoRight.Add(br[br.Length - 1]);
                        }
                        else if (parent.Parent == null)
                        {
                            if (isAnd) ctx.GotoFalse.Add(br[br.Length - 1]);
                            else ctx.GotoTrue.Add(br[br.Length - 1]);
                        }
                        else
                        {
                            parent.GotoEnd.Add(br[br.Length - 1]);
                        }
                    }
                }
                else
                {
                    //BoolNode cur = node;
                    while (parent != null)
                    {
                        if (parent.Parent == null)
                        {
                            if (isAnd) ctx.GotoFalse.Add(br[br.Length - 1]);
                            else ctx.GotoTrue.Add(br[br.Length - 1]);
                            break;
                        }
                        else if (parent == parent.Parent.Left
                            && parent.Parent.IsAnd != node.IsAnd)
                        {
                            parent.Parent.GotoRight.Add(br[br.Length - 1]);
                            break;
                        }
                        else
                        {
                            //cur = parent;
                            parent = parent.Parent;
                        }
                    }
                }
            }

            //enshure constant
            if (isAnd)
            {
                if (ctx.False == null)
                    ctx.False = _provider.LoadConstant(false);
            }
            else
            {
                if (ctx.True == null)
                    ctx.True = _provider.LoadConstant(true);
            }
        }

        private void SetTargets(IEnumerable<IInstruction> list)
        {
            foreach (IInstruction br in list)
            {
                _provider.SetBranchTarget(br, _list.Count);
            }
        }

        private void CompileBooleanExpression(IBinaryExpression e)
        {
            BoolNode root = new BoolNode();
            BuildBoolNode(e, root);
            BoolContext ctx = new BoolContext();
            CompileBooleanExpression(root, ctx);
            if (ctx.False != null && ctx.True != null)
            {
                SetTargets(ctx.GotoFalse);
                Emit(ctx.False);
                IInstruction br = _provider.Branch();
                Emit(br);
                ctx.GotoEnd.Add(br);
                SetTargets(ctx.GotoTrue);
                Emit(ctx.True);
            }
            else if (ctx.False != null)
            {
                SetTargets(ctx.GotoFalse);
                Emit(ctx.False);
            }
            else
            {
                SetTargets(ctx.GotoTrue);
                Emit(ctx.True);
            }
            SetTargets(ctx.GotoEnd);
        }
        #endregion

        #region CompileBinaryExpression
        private void CopyValue(IType type)
        {
            if (type.Kind == TypeKind.Struct)
            {
                Emit(_provider.CopyValue(type));
            }
        }

        private static bool CanCopyValue(IExpression right)
        {
            if (right is INewObjectExpression) return false;
            return true;
        }

        private void Assign(IExpression left, IExpression right)
        {
            _assign.Enabled = true;
            CompileExpression(left);
            _assign.Enabled = false;
            CompileExpression(right);
            if (CanCopyValue(right))
            {
                CopyValue(_assign.Type);
            }
            switch (_assign.TargetType)
            {
                case AssignTarget.Unknown:
                    throw new InvalidOperationException();

                case AssignTarget.Variable:
                    SetVar(_assign.TargetIndex);
                    break;

                case AssignTarget.Argument:
                    SetArg(_assign.TargetIndex);
                    break;

                case AssignTarget.Field:
                    SetField(_assign.Field);
                    break;

                case AssignTarget.Property:
                    SetProperty(_assign.Property);
                    break;

                case AssignTarget.Indexer:
                    SetProperty(_assign.Property);
                    break;

                case AssignTarget.ArrayItem:
                    SetArrayItem(_assign.ElementType);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CompileBinaryExpression(IBinaryExpression e)
        {
            BinaryOperator op = e.Operator;
            if (op == BinaryOperator.BooleanAnd || op == BinaryOperator.BooleanOr)
            {
                CompileBooleanExpression(e);
            }
            else if (op == BinaryOperator.Assign)
            {
                Assign(e.Left, e.Right);
            }
            else
            {
                IExpression left = ExpressionService.FixConstant(e.Left, e.Right.ResultType);
                IExpression right = ExpressionService.FixConstant(e.Right, left.ResultType);
                CompileExpression(left);
                CompileExpression(right);
                IType type = BinaryExpression.GetResultType(left.ResultType, right.ResultType, op);
                //TODO: checkOverflow
                IInstruction[] i = _provider.Op(op, left.ResultType, right.ResultType, type, false);
                Emit(i);
            }
        }
        #endregion

        #region CompileUnaryExpression
        private void Increment(IType type, bool post, bool add)
        {
            if (post) Dup();
            if (_provider.SupportIncrementOperators)
            {
                if (add) Emit(_provider.Increment(type));
                else Emit(_provider.Decrement(type));
            }
            else
            {
                LoadConstant(1);
                Op(type, type, add ? BinaryOperator.Addition : BinaryOperator.Subtraction);
            }
            if (!post) Dup();
        }

        private void LoadIndex(IEnumerable<int> vars)
        {
            foreach (int i in vars)
            {
                GetTempVar(i);
                Swap();
            }
        }

        private void KillTempVars(IEnumerable<int> vars)
        {
            foreach (int i in vars)
                KillTempVar(i);
        }

        private void _CompileIncrement(IExpression e, bool post, bool add)
        {
            IVariableReferenceExpression vref = e as IVariableReferenceExpression;
            if (vref != null)
            {
                IVariable v = vref.Variable;
                GetVar(v);
                Increment(v.Type, post, add);
                SetVar(v);
                return;
            }

            IArgumentReferenceExpression aref = e as IArgumentReferenceExpression;
            if (aref != null)
            {
                IParameter v = aref.Argument;
                GetArg(v);
                Increment(v.Type, post, add);
                SetArg(v);
                return;
            }

            IFieldReferenceExpression fref = e as IFieldReferenceExpression;
            if (fref != null)
            {
                IField v = fref.Field;

                CompileExpression(fref.Target);
                int var = SaveTarget(fref);
                GetField(v);

                Increment(v.Type, post, add);

                if (var >= 0)
                {
                    GetTempVar(var);
                    Swap();
                    SetField(v);
                    KillTempVar(var);
                }
                else
                {
                    SetField(v);
                }
                return;
            }

            IPropertyReferenceExpression pref = e as IPropertyReferenceExpression;
            if (pref != null)
            {
                IProperty v = pref.Property;

                CompileExpression(pref.Target);
                int var = SaveTarget(pref);
                GetProperty(pref);

                Increment(v.Type, post, add);

                if (var >= 0)
                {
                    GetTempVar(var);
                    Swap();
                    SetProperty(pref);
                    KillTempVar(var);
                }
                else
                {
                    SetProperty(pref);
                }
                return;
            }

            IIndexerExpression ie = e as IIndexerExpression;
            if (ie != null)
            {
                pref = ie.Property;
                IProperty v = pref.Property;

                CompileExpression(ie.Property.Target);
                int var = SaveTarget(pref);
                List<int> idx = new List<int>();
                foreach (IExpression i in ie.Index)
                {
                    CompileExpression(i);
                    idx.Add(SetTempVar(true));
                }
                GetProperty(pref);

                Increment(v.Type, post, add);

                if (var >= 0)
                {
                    GetTempVar(var);
                    Swap();
                    LoadIndex(idx);
                    SetProperty(pref);
                    KillTempVar(var);
                    KillTempVars(idx);
                }
                else //static
                {
                    LoadIndex(idx);
                    SetProperty(pref);
                    KillTempVars(idx);
                }
                return;
            }

            IArrayIndexerExpression ai = e as IArrayIndexerExpression;
            if (ai != null)
            {
                IType elemType = ai.ResultType;

                CompileExpression(ai.Array);
                int var = SaveTarget(pref);
                List<int> idx = new List<int>();
                foreach (IExpression i in ai.Index)
                {
                    CompileExpression(i);
                    idx.Add(SetTempVar(true));
                }
                GetArrayItem(elemType);

                Increment(elemType, post, add);

                if (var >= 0)
                {
                    GetTempVar(var);
                    Swap();
                    LoadIndex(idx);
                    SetArrayItem(elemType);
                    KillTempVar(var);
                    KillTempVars(idx);
                }
                else //static
                {
                    LoadIndex(idx);
                    SetArrayItem(elemType);
                    KillTempVars(idx);
                }
                return;
            }

            throw new NotImplementedException();
        }

        private void CompileIncrement(IExpression e, bool post, bool add)
        {
            bool assign = _assign.Enabled;
            _assign.Enabled = false;
            _CompileIncrement(e, post, add);
            _assign.Enabled = assign;
        }

        private void CompileUnaryExpression(IUnaryExpression e)
        {
            IExpression ee = e.Expression;
            UnaryOperator op = e.Operator;
            switch (op)
            {
                case UnaryOperator.PreIncrement:
                    CompileIncrement(ee, false, true);
                    break;

                case UnaryOperator.PostIncrement:
                    CompileIncrement(ee, true, true);
                    break;

                case UnaryOperator.PreDecrement:
                    CompileIncrement(ee, false, false);
                    break;

                case UnaryOperator.PostDecrement:
                    CompileIncrement(ee, true, false);
                    break;

                default:
                    {
                        //TODO: checkOverflow
                        CompileExpression(ee);
                        IType type = ee.ResultType;
                        Emit(_provider.Op(e.Operator, type, false));
                    }
                    break;
            }
        }
        #endregion

        #region CompileConditionExpression
        private void CompileConditionExpression(IConditionExpression e)
        {
            //TODO:
            BranchOperator op = CompileCondition(e.Condition, true);
            IInstruction[] skipTrue = _provider.Branch(op, null, null);
            Emit(skipTrue);
            CompileExpression(e.TrueExpression);
            IInstruction skipFalse = _provider.Branch();
            Emit(skipFalse);
            _provider.SetBranchTarget(skipTrue[skipTrue.Length - 1], _list.Count);
            CompileExpression(e.FalseExpression);
            _provider.SetBranchTarget(skipFalse, _list.Count);
        }
        #endregion

        #region Call
        private class Call
        {
            public IParameter CurrentArgument;
            public bool PreventByRef;

            public bool IsByRef
            {
                get
                {
                    if (PreventByRef) return false;
                    if (CurrentArgument != null)
                        return CurrentArgument.IsByRef;
                    return false;
                }
            }
        }
        private readonly Stack<Call> _callStack = new Stack<Call>();

        private Call CurrentCall
        {
            get
            {
                if (_callStack.Count > 0)
                    return _callStack.Peek();
                return null;
            }
        }

        private bool IsByRef
        {
            get
            {
                bool byref = false;
                Call call = CurrentCall;
                if (call != null)
                {
                    if (call.IsByRef)
                    {
                        byref = true;
                        call.PreventByRef = true;
                    }
                }
                return byref;
            }
        }

        private static bool IsThisOrBase(IExpression target)
        {
            if (target == null) return false;
            return target is IThisReferenceExpression || target is IBaseReferenceExpression;
        }

        private void CallMethod(IExpression target, IMethod method, IList<IExpression> args,
            bool isDelegate, bool newobj)
        {
            Call call = new Call();
            _callStack.Push(call);

            Emit(_provider.BeginCall(method));

            if (target != null)
            {
                if (_provider.CanCompileTarget(method, isDelegate))
                    CompileExpression(target);
            }
            Emit(_provider.LoadReceiver(method, isDelegate, newobj));

            if (args != null)
            {
                int n = args.Count;
                if (n > 0)
                {
                    for (int i = 0; i < n; ++i)
                    {
                        IParameter p = method.Parameters[i];
                        IExpression arg = args[i];
                        arg = ExpressionService.FixConstant(arg, p.Type);
                        call.CurrentArgument = p;
                        CompileExpression(arg);
                        //Note: value type must be copied
                        CopyValue(p.Type);
                    }
                    call.CurrentArgument = null;
                }
            }

            if (isDelegate)
            {
                Emit(_provider.InvokeDelegate(method));
            }
            else
            {
                //TODO: determine virtcall
                Emit(_provider.CallMethod(method, IsThisOrBase(target), false, newobj));
            }

            Emit(_provider.EndCall(method));

            _callStack.Pop();
        }

        private void CompileCallExpression(ICallExpression e)
        {
            IMethodReferenceExpression r = e.Method;
            CallMethod(r.Target, r.Method, e.Arguments, false, false);
        }
        #endregion

        #region INewObjectExpression
        private void CompileNewObjectExpression(INewObjectExpression e)
        {
            if (e.Constructor == null)
                Emit(_provider.InitObject(e.ObjectType));
            else
                CallMethod(null, e.Constructor, e.Arguments, false, true);
        }
        #endregion

        #region INewArrayExpression
        private void CompileNewArrayExpression(INewArrayExpression e)
        {
            IType elemType = e.ElementType;
            int n = e.Dimensions.Count;
            if (n == 1)
            {
                n = e.Initializers.Count;
                //Emit(_provider.NewArrayTarget(elemType));
                //put array size onto the stack
                CompileExpression(e.Dimensions[0]);
                Emit(_provider.NewArray(elemType));
                for (int i = 0; i < n; ++i)
                {
                    //put array onto the stack
                    Emit(_provider.Dup());
                    LoadConstant(i); //index
                    CompileExpression(e.Initializers[i]);
                    Emit(_provider.SetArrayItem(elemType));
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        #region Cast Expressions
        private void CompileCastExpression(ICastExpression e)
        {
            CompileExpression(e.Expression);
            if (e.Operator == CastOperator.Is)
            {
                Emit(_provider.Is(e.TargetType));
            }
            else if (e.Operator == CastOperator.As)
            {
                Emit(_provider.As(e.TargetType));
            }
            else
            {
                Emit(_provider.Cast(e.SourceType, e.TargetType));
            }
        }

        private void CompileBoxExpression(IBoxExpression e)
        {
            CompileExpression(e.Expression);
            Emit(_provider.Box(e.SourceType));
        }

        private void ComileUnboxExpression(IUnboxExpression e)
        {
            CompileExpression(e.Expression);
            Emit(_provider.Unbox(e.TargetType));
        }
        #endregion

        #region Delegates
        private void CompileNewDelegateExpression(INewDelegateExpression e)
        {
            CompileExpression(e.Method.Target);
            Emit(_provider.LoadFunction(e.Method.Method));
        }

        private void CompileDelegateInvokeExpression(IDelegateInvokeExpression e)
        {
            CallMethod(e.Target, e.Method, e.Arguments, true, false);
        }
        #endregion

        private void CompileTypeOfExpression(ITypeOfExpression e)
        {
            Emit(_provider.TypeOf(e.Type));
        }

        private void CompileSizeOfExpression(ISizeOfExpression e)
        {
            Emit(_provider.SizeOf(e.Type));
        }
        #endregion

        #region Utils
        private void Dup()
        {
            Emit(_provider.Dup());
        }

        private void Swap()
        {
            Emit(_provider.Swap());
        }

        private void GetVar(int index, bool byref, IType type)
        {
            Emit(_provider.LoadVariable(index, byref, type));
        }

        private void GetVar(IVariable v, bool byref)
        {
            GetVar(v.Index, byref, v.Type);
        }

        private void GetVar(IVariable v)
        {
            GetVar(v, false);
        }

        private void SetVar(int index)
        {
            Emit(_provider.StoreVariable(index));
        }

        private void SetVar(IVariable v)
        {
            SetVar(v.Index);
        }

        private void GetArg(int index, bool byref, IType type)
        {
            Emit(_provider.LoadArgument(index, byref, type));
        }

        private void GetArg(int index, IType type)
        {
            GetArg(index, false, type);
        }

        private void GetArg(IParameter p)
        {
            GetArg(p.Index - 1, TypeService.UnwrapRef(p.Type));
        }

        private void SetArg(int index)
        {
            Emit(_provider.StoreArgument(index));
        }

        private void SetArg(IParameter p)
        {
            SetArg(p.Index - 1);
        }

        private void Op(IType left, IType right, BinaryOperator op)
        {
            //TODO: checkOverflow
            IType type = BinaryExpression.GetResultType(left, right, op);
            Emit(_provider.Op(op, left, right, type, false));
        }

        private void GetField(IField field, bool byref)
        {
            Emit(_provider.LoadField(field, byref));
        }

        private void GetField(IField field)
        {
            GetField(field, false);
        }

        private void SetField(IField field)
        {
            Emit(_provider.StoreField(field));
        }

        //private void LoadReceiver(IMethod method, bool newobj)
        //{
        //    Emit(_provider.LoadReceiver(method, false, newobj));
        //}

        private void GetProperty(IPropertyReferenceExpression pref)
        {
            IProperty prop = pref.Property;
            Emit(_provider.CallMethod(prop.Getter, IsThisOrBase(pref.Target), true, false));
        }

        private void SetProperty(IPropertyReferenceExpression pref)
        {
            IProperty prop = pref.Property;
            Emit(_provider.CallMethod(prop.Setter, IsThisOrBase(pref.Target), true, false));
        }

        private void GetArrayItem(IType elemType, bool byref)
        {
            Emit(_provider.GetArrayItem(elemType, byref));
        }

        private void GetArrayItem(IType elemType)
        {
            GetArrayItem(elemType, false);
        }

        private void SetArrayItem(IType elemType)
        {
            Emit(_provider.SetArrayItem(elemType));
        }

        private int SetTempVar(bool keepStackState)
        {
            int var;
            Emit(_provider.SetTempVar(out var, keepStackState));
            return var;
        }

        private void GetTempVar(int var)
        {
            if (var >= 0)
            {
                Emit(_provider.GetTempVar(var));
            }
        }

        private void KillTempVar(int var)
        {
            if (var >= 0)
            {
                Emit(_provider.KillTempVar(var));
            }
        }

        private int SaveTarget(IMemberReferenceExpression e)
        {
            if (e.Member.IsStatic)
            {
                if (!_provider.SupportStaticTarget)
                    return -1;
            }
            return SetTempVar(true);
        }
        #endregion
    }
}