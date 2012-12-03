using System.Collections.Generic;
using DataDynamics.PageFX.Common;
using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX
{
    internal class MethodNode : ApiNode
    {
        public MethodNode(IMethod method)
        {
            Method = method;
            Name = method.ApiName(true);
            FullName = method.ApiFullName();
        }

        public string FullName;
        public IMethod Method;

        public override NodeKind NodeKind
        {
            get
            {
                return NodeKind.Method;
            }
        }
        
        public override string Class
        {
            get { return "method"; }
        }

        public override string Image
        {
            get 
            {
                if (Method.IsConstructor)
                    return Images.Constructor;
                if (Method.Name.StartsWith("op_"))
                    return Images.Operator;
                return Images.Method;
            }
        }

        public void CalcStat()
        {
            var tests = Global.TestCache[FullName] as List<TestNode>;
            if (tests != null)
            {
                foreach (var test in tests)
                {
                    AddTest(test);
                    UpdateStats(test);
                }
                var parent = Parent as ApiNode;
                while (parent != null)
                {
                    parent.UpdateStat(this);
                    parent = parent.Parent as ApiNode;
                }
            }
        }
    }
}