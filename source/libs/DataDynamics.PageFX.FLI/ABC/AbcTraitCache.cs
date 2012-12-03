using System;
using System.Collections;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.FLI.ABC
{
    internal class AbcTraitCache
    {
        readonly Hashtable _getters = new Hashtable();
        readonly Hashtable _setters = new Hashtable();
        readonly Hashtable _other = new Hashtable();

        public void Add(AbcTrait trait)
        {
            string key = AbcQName.GetKey(trait);
            switch (trait.Kind)
            {
                case AbcTraitKind.Getter:
                    _getters[key] = trait;
                    break;

                case AbcTraitKind.Setter:
                    _setters[key] = trait;
                    break;

                default:
                    _other[key] = trait;
                    break;
            }
        }

        public void Add(IEnumerable<AbcTrait> set)
        {
            foreach (var trait in set)
                Add(trait);
        }

        static AbcTrait Find(Hashtable cache, ITypeMember member)
        {
            var qn = new AbcQName(member);
            return cache[qn.Key] as AbcTrait;
        }

        public AbcTrait Find(ITypeMember member)
        {
            var method = member as IMethod;
            if (method != null)
            {
	            switch (method.ResolveTraitKind())
	            {
		            case AbcTraitKind.Getter:
						return Find(_getters, member);
		            case AbcTraitKind.Setter:
						return Find(_setters, member);
	            }
            }
            return Find(_other, member);
        }
    }

    #region QName
    internal class AbcQName
    {
        public string name;
        public string ns = "";
        public AbcConstKind nskind = AbcConstKind.PackageNamespace;

        public string FullName
        {
            get { return ns.MakeFullName(name); }
        }

        #region ctor
        public AbcQName(ITypeMember m)
        {
            var type = m as IType;
            var attr = m.FindAttribute(Attrs.QName);
            if (attr == null)
            {
                if (type != null)
                    ns = type.Namespace;

                var method = m as IMethod;
                if (method != null && (method.IsGetter() || method.IsSetter()))
                {
                    name = m.Name.Substring(4);
                    return;
                }

                name = m.Name;
            }
            else
            {
                int n = attr.Arguments.Count;
                if (n == 0)
                    throw new InvalidOperationException("Invalid qname attribute");
                name = (string)attr.Arguments[0].Value;
                if (n > 1)
                {
                    ns = (string)attr.Arguments[1].Value;
                    string kind = (string)attr.Arguments[2].Value;
                    nskind = AbcNamespace.FromShortNsKind(kind);
                }
            }
        }
        #endregion

        public string Key
        {
            get { return GetKey(ns, name, nskind); }
        }

        public static string GetKey(AbcTrait t)
        {
            return GetKey(t.Name);
        }

        public static string GetKey(AbcMultiname name)
        {
            var ns = name.Namespace;
            return GetKey(ns.NameString, name.Name.Value, ns.Kind);
        }

        public static string GetKey(string ns, string name, AbcConstKind nskind)
        {
            string result = "";
            if (!string.IsNullOrEmpty(ns))
            {
                result += ns;
                result += ".";
            }
            result += name;
            result += ":";
            result += (int)nskind;
            return result;
        }

        public bool Equals(AbcMultiname mn)
        {
            if (mn == null) return false;
            if (mn.Kind != AbcConstKind.QName)
                return false;
            if (mn.NameString != name)
                return false;
            if (mn.Namespace.Kind != nskind)
                return false;
            if (string.IsNullOrEmpty(ns))
                return string.IsNullOrEmpty(mn.NamespaceString);
            return mn.NamespaceString == ns;
        }
    }
    #endregion
}