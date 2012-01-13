namespace Microsoft.VisualBasic
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
    public sealed class MyGroupCollectionAttribute : Attribute
    {
        private string m_DefaultInstanceAlias;
        private string m_NameOfBaseTypeToCollect;
        private string m_NameOfCreateMethod;
        private string m_NameOfDisposeMethod;

        public MyGroupCollectionAttribute(string typeToCollect, string createInstanceMethodName, string disposeInstanceMethodName, string defaultInstanceAlias)
        {
            this.m_NameOfBaseTypeToCollect = typeToCollect;
            this.m_NameOfCreateMethod = createInstanceMethodName;
            this.m_NameOfDisposeMethod = disposeInstanceMethodName;
            this.m_DefaultInstanceAlias = defaultInstanceAlias;
        }

        public string CreateMethod
        {
            get
            {
                return this.m_NameOfCreateMethod;
            }
        }

        public string DefaultInstanceAlias
        {
            get
            {
                return this.m_DefaultInstanceAlias;
            }
        }

        public string DisposeMethod
        {
            get
            {
                return this.m_NameOfDisposeMethod;
            }
        }

        public string MyGroupName
        {
            get
            {
                return this.m_NameOfBaseTypeToCollect;
            }
        }
    }
}

