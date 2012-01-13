using System;
using System.Collections.Generic;
using DataDynamics.PageFX.FLI.SWF;

namespace DataDynamics.PageFX.FLI.ABC
{
    #region class ImportContext
    public class ImportContext
    {
        public AblFile ABL { get; set; }

        public AbcFile CurrentABC { get; set; }

        public AbcFile TargetABC { get; set; }

        internal SwfCompiler SwfCompiler
        {
            get { return TargetABC.SwfCompiler; }
        }
    }
    #endregion

    #region class Dep
    /// <summary>
    /// Represents abstract dependency for ABC files.
    /// </summary>
    public abstract class Dep
    {
        public DepKind Kind { get; set; }

        public abstract DepType Type { get; }

        public abstract void Read(AblFile abl, SwfReader reader);

        public abstract void Write(SwfWriter writer);

        public abstract void Import(ImportContext ctx);

        public static Dep Create(DepType type)
        {
            switch (type)
            {
                case DepType.FileRef: return new DepFileRef();
                case DepType.TypeRef: return new DepTypeRef();
                case DepType.NamespaceRef: return new DepNamespaceRef();
                case DepType.InstanceRef: return new DepInstanceRef(null);
                case DepType.ResourceBundle: return new DepResourceBundle();
                case DepType.RemoteClass: return new DepRemoteClass();
                case DepType.EffectTrigger: return new DepEffectTrigger();
                case DepType.StyleMixin: return new DepStyleMixin();
                case DepType.Asset: return new DepAsset();
                case DepType.Mixin: return new DepMixin();
            }
            return null;
        }
    }
    #endregion

    #region class DepList
    public class DepList : List<Dep>
    {
        public DepList()
        {
        }

        public DepList(AblFile abl)
        {
            _abl = abl;
        }

        public AblFile ABL
        {
            get { return _abl; }
        }
        readonly AblFile _abl;

        public void Read(SwfReader reader)
        {
            int n = (int)reader.ReadUIntEncoded();
            for (int i = 0; i < n; ++i)
            {
                var dt = (DepType)reader.ReadByte();
                var kind = (DepKind)reader.ReadByte();
                var dep = Dep.Create(dt);
                dep.Kind = kind;
                dep.Read(_abl, reader);
            }
        }

        public void Write(SwfWriter writer)
        {
            int n = Count;
            writer.WriteUIntEncoded(n);
            for (int i = 0; i < n; ++i)
            {
                var dep = this[i];
                writer.WriteByte((byte)dep.Type);
                writer.WriteByte((byte)dep.Kind);
                dep.Write(writer);
            }
        }

        public void Import(ImportContext ctx, DepKind kind)
        {
            ctx.ABL = _abl;
            foreach (var dep in this)
            {
                if (dep.Kind == kind)
                    dep.Import(ctx);
            }
        }
    }
    #endregion

    #region class DepFileRef
    public class DepFileRef : Dep
    {
        public DepFileRef()
        {
        }

        public DepFileRef(int index)
        {
            Index = index;
        }

        public override DepType Type
        {
            get { return DepType.FileRef; }
        }

        public int Index { get; set; }

        public override void Read(AblFile abl, SwfReader reader)
        {
            Index = reader.ReadInt32();
        }

        public override void Write(SwfWriter writer)
        {
            writer.WriteInt32(Index);
        }

        public override void Import(ImportContext ctx)
        {
            var f = ctx.ABL.Files[Index];
            ctx.TargetABC.Import(f);
        }
    }
    #endregion

    #region class DepTypeRef
    public class DepTypeRef : Dep
    {
        public DepTypeRef()
        {
        }

        public DepTypeRef(string fullname)
        {
            FullName = fullname;
        }

        public override DepType Type
        {
            get { return DepType.TypeRef; }
        }

        public string FullName { get; set; }

        public override void Read(AblFile abl, SwfReader reader)
        {
            FullName = reader.ReadString();
        }

        public override void Write(SwfWriter writer)
        {
            writer.WriteString(FullName);
        }

        public override void Import(ImportContext ctx)
        {
            ctx.TargetABC.ImportInstance(FullName);
        }
    }
    #endregion

    #region class DepNamespaceRef
    public class DepNamespaceRef : Dep
    {
        public DepNamespaceRef()
        {
        }

        public DepNamespaceRef(int index)
        {
            Index = index;
            Kind = DepKind.Post;
        }

        public override DepType Type
        {
            get { return DepType.NamespaceRef; }
        }

        public int Index { get; set; }

        public override void Read(AblFile abl, SwfReader reader)
        {
            Index = (int)reader.ReadUIntEncoded();
        }

        public override void Write(SwfWriter writer)
        {
            writer.WriteUIntEncoded(Index);
        }

        public override void Import(ImportContext ctx)
        {
            var list = ctx.ABL.Namespaces;
            string name = list.GetName(Index);
            var ns = list[Index];
            ctx.TargetABC.DefineNamespaceScript(ns, name);
        }
    }
    #endregion

    #region class DepInstanceRef
    public class DepInstanceRef : Dep
    {
        public DepInstanceRef(AbcInstance instance)
        {
            Instance = instance;
        }

        public override DepType Type
        {
            get { return DepType.InstanceRef; }
        }

        public AbcInstance Instance { get; set; }

        public override void Read(AblFile abl, SwfReader reader)
        {
            throw new NotImplementedException();
        }

        public override void Write(SwfWriter writer)
        {
            throw new NotImplementedException();
        }

        public override void Import(ImportContext ctx)
        {
            ctx.TargetABC.ImportInstance(Instance);
        }
    }
    #endregion

    #region class DepResourceBundle
    public class DepResourceBundle : Dep
    {
        public DepResourceBundle()
        {
        }

        public DepResourceBundle(string name)
        {
            ResourceBundle = name;
            Kind = DepKind.Post;
        }

        public override DepType Type
        {
            get { return DepType.ResourceBundle; }
        }

        public string ResourceBundle { get; set; }

        public override void Read(AblFile abl, SwfReader reader)
        {
            ResourceBundle = reader.ReadString();
        }

        public override void Write(SwfWriter writer)
        {
            writer.WriteString(ResourceBundle);
        }

        public override void Import(ImportContext ctx)
        {
            ctx.TargetABC.ImportResourceBundle(ResourceBundle);
        }
    }
    #endregion

    #region class DepRemoteClass
    public class DepRemoteClass : Dep
    {
        public DepRemoteClass()
        {
        }

        public DepRemoteClass(string alias)
        {
            Alias = alias;
            Kind = DepKind.Post;
        }

        public override DepType Type
        {
            get { return DepType.RemoteClass; }
        }

        public string Alias { get; set; }

        public override void Read(AblFile abl, SwfReader reader)
        {
            Alias = reader.ReadString();
        }

        public override void Write(SwfWriter writer)
        {
            writer.WriteString(Alias);
        }

        public override void Import(ImportContext ctx)
        {
            var sfc = ctx.SwfCompiler;
            if (sfc == null)
                throw new InvalidOperationException("invalid context");
            var f = ctx.CurrentABC;
            sfc.RegisterRemoteClass(Alias, f.FirstInstance);
        }
    }
    #endregion

    #region class DepEffectTrigger
    public class DepEffectTrigger : Dep
    {
        public DepEffectTrigger()
        {
        }

        public DepEffectTrigger(string name, string eventName)
        {
            Name = name;
            Event = eventName;
            Kind = DepKind.Post;
        }

        public override DepType Type
        {
            get { return DepType.EffectTrigger; }
        }

        public string Name { get; set; }
        public string Event { get; set; }

        public override void Read(AblFile abl, SwfReader reader)
        {
            Name = reader.ReadString();
            Event = reader.ReadString();
        }

        public override void Write(SwfWriter writer)
        {
            writer.WriteString(Name);
            writer.WriteString(Event);
        }

        public override void Import(ImportContext ctx)
        {
            var sfc = ctx.SwfCompiler;
            if (sfc == null)
                throw new InvalidOperationException("invalid context");
            sfc.RegisterEffectTrigger(Name, Event);
        }
    }
    #endregion

    #region class DepAsset
    public class DepAsset : Dep
    {
        public override DepType Type
        {
            get { return DepType.Asset; }
        }

        public int Index;

        public override void Read(AblFile abl, SwfReader reader)
        {
            Index = (int)reader.ReadUIntEncoded();
        }

        public override void Write(SwfWriter writer)
        {
            writer.WriteUIntEncoded(Index);
        }

        public override void Import(ImportContext ctx)
        {
            var sfc = ctx.SwfCompiler;
            if (sfc == null)
                throw new InvalidOperationException("invalid context");
            var instance = ctx.CurrentABC.FirstInstance;
            var a = ctx.ABL.Assets[Index];
            sfc.ImportAsset(a, instance);
        }
    }
    #endregion

    #region class DepStyleMixin
    public class DepStyleMixin : Dep
    {
        public override DepType Type
        {
            get { return DepType.StyleMixin; }
        }

        public string Name { get; set; }

        public override void Read(AblFile abl, SwfReader reader)
        {
            Name = reader.ReadString();
        }

        public override void Write(SwfWriter writer)
        {
            writer.WriteString(Name);
        }

        public override void Import(ImportContext ctx)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region class DepMixin
    public class DepMixin : Dep
    {
        public DepMixin()
        {
            Kind = DepKind.Post;
        }

        public override DepType Type
        {
            get { return DepType.Mixin; }
        }

        public override void Read(AblFile abl, SwfReader reader)
        {
        }

        public override void Write(SwfWriter writer)
        {
        }

        public override void Import(ImportContext ctx)
        {
            var sfc = ctx.SwfCompiler;
            if (sfc == null)
                throw new InvalidOperationException("invalid context");

            var f = ctx.CurrentABC;
            var instance = f.FirstInstance;
            sfc.RegisterMixin(instance);
        }
    }
    #endregion

    #region Enums
    public enum DepType : byte 
    {
        FileRef,
        TypeRef,
        NamespaceRef,
        InstanceRef,
        ResourceBundle,
        Asset,
        StyleMixin,
        RemoteClass,
        EffectTrigger,
        Mixin,
    }

    public enum DepKind : byte 
    {
        Pre,
        Post
    }
    #endregion
}