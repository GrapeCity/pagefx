using System;
using DataDynamics.PageFX.Common.CodeModel.Expressions;
using DataDynamics.PageFX.Common.CodeModel.Statements;

namespace DataDynamics.PageFX.Common.CodeModel
{
    public class StatementVisitor
    {
        private readonly ActionSelector _statements = new ActionSelector();
        private readonly ActionSelector _expressions = new ActionSelector();

        public StatementVisitor()
        {
            AddStatement<IVariableDeclarationStatement>(VisitVariableDeclarationStatement);
            AddStatement<IExpressionStatement>(VisitExpressionStatement);
            AddStatement<IStatementCollection>(VisitStatementCollection);
            AddStatement<IReturnStatement>(VisitReturnStatement);
            AddStatement<IIfStatement>(VisitIfStatement);
            AddStatement<ILoopStatement>(VisitLoopStatement);
            AddStatement<ISwitchStatement>(VisitSwitchStatement);
            AddStatement<IGotoStatement>(VisitGotoStatement);
            AddStatement<ILabeledStatement>(VisitLabeledStatement);
            AddStatement<ITryCatchStatement>(VisitTryCatchStatement);
            AddStatement<IBreakStatement>(VisitBreakStatement);
            AddStatement<IContinueStatement>(VisitContinueStatement);
            AddStatement<ICommentStatement>(VisitCommentStatement);
            AddStatement<IDebuggerBreakStatement>(VisitDebuggerBreakStatement);

            AddExpression<IConstantExpression>(VisitConstantExpression);
            AddExpression<IVariableReferenceExpression>(VisitVariableReferenceExpression);
            AddExpression<IArgumentReferenceExpression>(VisitArgumentReferenceExpression);
            AddExpression<IFieldReferenceExpression>(VisitFieldReferenceExpression);
            AddExpression<IBinaryExpression>(VisitBinaryExpression);
            AddExpression<IUnaryExpression>(VisitUnaryExpression);
            AddExpression<ICallExpression>(VisitCallExpression);
            AddExpression<IMethodReferenceExpression>(VisitMethodReferenceExpression);
            AddExpression<ITypeReferenceExpression>(VisitTypeReferenceExpression);
            AddExpression<IConditionExpression>(VisitConditionExpression);
        }

        #region Statements
        private void AddStatement<T>(Action<T> method)
            where T : class, IStatement
        {
            _statements.Add(method);
        }

        public virtual void VisitStatement(IStatement statement)
        {
            if (!_statements.Run(statement))
                throw new NotImplementedException();
        }

        public virtual void VisitStatementCollection(IStatementCollection statements)
        {
            if (statements == null) return;
            foreach (var statement in statements)
            {
                VisitStatement(statement);
            }
        }

        public virtual void VisitVariableDeclarationStatement(IVariableDeclarationStatement s)
        {
        }

        public virtual void VisitExpressionStatement(IExpressionStatement s)
        {
            VisitExpression(s.Expression);
        }

        public virtual void VisitReturnStatement(IReturnStatement s)
        {
        }

        public virtual void VisitIfStatement(IIfStatement s)
        {
        }

        public virtual void VisitLoopStatement(ILoopStatement s)
        {
        }

        public virtual void VisitSwitchStatement(ISwitchStatement s)
        {
        }

        public virtual void VisitLabeledStatement(ILabeledStatement s)
        {
        }

        public virtual void VisitGotoStatement(IGotoStatement s)
        {
        }

        public virtual void VisitTryCatchStatement(ITryCatchStatement s)
        {
        }

        public virtual void VisitBreakStatement(IBreakStatement s)
        {
        }

        public virtual void VisitContinueStatement(IContinueStatement s)
        {
        }

        public virtual void VisitCommentStatement(ICommentStatement s)
        {
        }

        public virtual void VisitDebuggerBreakStatement(IDebuggerBreakStatement s)
        {
        }
        #endregion

        #region Expressions
        private void AddExpression<T>(Action<T> method)
            where T : class, IExpression
        {
            _expressions.Add(method);
        }

        public virtual void VisitExpression(IExpression e)
        {
            if (!_expressions.Run(e))
                throw new NotImplementedException();
        }

        public virtual void VisitConstantExpression(IConstantExpression e)
        {
        }

        public virtual void VisitVariableReferenceExpression(IVariableReferenceExpression e)
        {
        }

        public virtual void VisitArgumentReferenceExpression(IArgumentReferenceExpression e)
        {
        }

        public virtual void VisitFieldReferenceExpression(IFieldReferenceExpression e)
        {
        }

        public virtual void VisitBinaryExpression(IBinaryExpression e)
        {
            VisitExpression(e.Left);
            VisitExpression(e.Right);
        }

        public virtual void VisitUnaryExpression(IUnaryExpression e)
        {
            VisitExpression(e.Expression);
        }

        public virtual void VisitConditionExpression(IConditionExpression e)
        {
        }

        public virtual void VisitMethodReferenceExpression(IMethodReferenceExpression e)
        {
        }

        public virtual void VisitTypeReferenceExpression(ITypeReferenceExpression e)
        {
        }

        public virtual void VisitCallExpression(ICallExpression e)
        {
        }
        #endregion
    }
}