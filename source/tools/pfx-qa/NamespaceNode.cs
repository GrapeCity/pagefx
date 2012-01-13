namespace DataDynamics.PageFX
{
    internal class NamespaceNode : ApiNode
    {
        public AssemblyNode Assembly
        {
            get { return Parent as AssemblyNode; }
        }

        public override NodeKind NodeKind
        {
            get { return NodeKind.Namespace; }
        }

        public override string Class
        {
            get { return "namespace"; }
        }

        public override string Image
        {
            get { return Images.Namespace; }
        }

        protected override bool UseNameForUrl
        {
            get  { return Assembly != null; }
        }

        protected override string UrlPrefix
        {
            get 
            {
                var an = Assembly;
                if (an != null)
                    return an.Name;
                return "";
            }
        }
    }
}