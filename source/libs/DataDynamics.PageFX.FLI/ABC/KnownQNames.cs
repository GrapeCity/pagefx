using System;

namespace DataDynamics.PageFX.FLI.ABC
{
    internal enum KnownNamespace
    {
        Global,
        Internal,
        BodyTrait,
        AS3,
        MxInternal,
        PfxPackage,
        PfxPublic,
    }

    internal abstract class KnownQName
    {
        private readonly string _name;

        protected KnownQName(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            _name = name;
        }

        protected virtual KnownNamespace KnownNamespace
        {
            get 
            {
                return KnownNamespace.Global;
            }
        }

        protected virtual AbcNamespace DefineNamespace(AbcFile abc)
        {
            return abc.DefineNamespace(KnownNamespace);
        }

        public AbcMultiname Define(AbcFile abc)
        {
            var ns = DefineNamespace(abc);
            return abc.DefineQName(ns, _name);
        }
    }

    internal class GlobalQName : KnownQName
    {
        public GlobalQName(string name)
            : base(name)
        {
        }

        protected override KnownNamespace KnownNamespace
        {
            get { return KnownNamespace.Global; }
        }
    }

    internal class PfxQName : KnownQName
    {
        public PfxQName(string name)
            : base(name)
        {
        }

        protected override KnownNamespace KnownNamespace
        {
            get { return KnownNamespace.PfxPackage; }
        }
    }

    internal class AS3QName : KnownQName
    {
        public AS3QName(string name)
            : base(name)
        {
        }

        protected override KnownNamespace KnownNamespace
        {
            get { return KnownNamespace.AS3; }
        }
    }
}