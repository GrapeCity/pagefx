using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX
{
    internal class AssemblyNode : ApiNode
    {
        public IAssembly Assembly;

        public override NodeKind NodeKind
        {
            get { return NodeKind.Assembly; }
        }

        public override string Class
        {
            get { return "assembly"; }
        }

        public override string Image
        {
            get { return Images.Assembly; }
        }

        protected override bool UseNameForUrl
        {
            get { return true; }
        }

        protected override string UrlPrefix
        {
            get { return "asm"; }
        }

        public void Load()
        {
            //Name = Assembly.Name + " [version = " + Assembly.Version + "]";
            Name = Assembly.Name;

            foreach (var type in Assembly.Types)
            {
                if (type.DeclaringType != null) continue;
                if (!ClassNode.TypeFilter(type)) continue;

                string ns = type.Namespace;
                var nsnode = this[ns] as NamespaceNode;
                if (nsnode == null)
                {
                    nsnode = new NamespaceNode {Name = ns};
                    Add(nsnode);
                }

                var classNode = new ClassNode { Type = type };
                nsnode.Add(classNode);

                classNode.Load();
            }
        }
    }
}