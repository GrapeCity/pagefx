namespace DataDynamics.PageFX.CodeModel
{
    public sealed class MethodCode
    {
        public MethodCode(IMethodBody body)
        {
            _body = body;
        }
        private readonly IMethodBody _body;

        public IExpression ArgRef(int index)
        {
            return new ArgumentReferenceExpression(_body.Method.Parameters[index]);
        }

        public void AddStatement(IStatement st)
        {
            _body.Statements.Add(st);
        }

        public void AddExpressionStatement(IExpression e)
        {
            AddStatement(new ExpressionStatement(e));
        }

        public void Assign(IExpression left, IExpression right)
        {
            AddExpressionStatement(new BinaryExpression(left, right, BinaryOperator.Assign));
        }

        public void SetField(IExpression target, IField field, IExpression value)
        {
            Assign(new FieldReferenceExpression(target, field), value);
        }

        public void SetField(IField field, IExpression value)
        {
            if (field.IsStatic)
            {
                SetField(new TypeReferenceExpression(field.DeclaringType), field, value);
            }
            else
            {
                SetField(new ThisReferenceExpression(field.DeclaringType), field, value);
            }
        }

        public void Call(IExpression target, IMethod method)
        {
            AddExpressionStatement(new CallExpression(new MethodReferenceExpression(target, method)));
        }

        public void Call(IMethod method)
        {
            if (method.IsStatic)
            {
                Call(new TypeReferenceExpression(method.DeclaringType), method);
            }
            else
            {
                var type = method.DeclaringType;
                if (type == _body.Method.DeclaringType)
                {
                    Call(new ThisReferenceExpression(type), method);
                }
                else
                {
                    Call(new BaseReferenceExpression(type), method);
                }
            }
        }

        public override string ToString()
        {
            return _body.Statements.ToString();
        }
    }
}