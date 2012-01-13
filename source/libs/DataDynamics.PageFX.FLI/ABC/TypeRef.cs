using System.Collections;
using System.Collections.Generic;
using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI.ABC
{
    public enum TypeRefTarget
    {
        Inheritance,
        Member,
        Code
    }

    public struct TypeRef
    {
        public TypeRef(string fullname, TypeRefTarget target)
        {
            _fullname = fullname;
            _target = target;
        }

        public string FullName
        {
            get { return _fullname; }
        }
        readonly string _fullname;

        public TypeRefTarget Target
        {
            get { return _target; }
        }
        readonly TypeRefTarget _target;

        public DepKind DepKind
        {
            get 
            {
                switch (_target)
                {
                        case TypeRefTarget.Inheritance:
                            return DepKind.Pre;
                }
                return DepKind.Post;
            }
        }

        public static List<TypeRef> GetRefs(AbcFile abc)
        {
            var list = new List<TypeRef>();
            var cache = new Hashtable();
            GetRefs(abc, list, cache);
            return list;
        }

        static void GetRefs(AbcFile abc, List<TypeRef> list, Hashtable cache)
        {
            foreach (var script in abc.Scripts)
                GetRefs(script, list, cache);
        }

        static void GetRefs(IAbcTraitProvider provider, List<TypeRef> list, Hashtable cache)
        {
            var instance = provider as AbcInstance;
            if (instance != null)
            {
                Add(instance.SuperName, TypeRefTarget.Inheritance, list, cache);
                foreach (var ifaceName in instance.Interfaces)
                    Add(ifaceName, TypeRefTarget.Inheritance, list, cache);

                GetRefs(instance.GetAllTraits(), list, cache);
            }
            else
            {
                GetRefs(provider.Traits, list, cache);
            }
        }

        static void GetRefs(IEnumerable<AbcTrait> traits, List<TypeRef> list, Hashtable cache)
        {
            foreach (var t in traits)
            {
                switch (t.Kind)
                {
                    case AbcTraitKind.Function:
                    case AbcTraitKind.Getter:
                    case AbcTraitKind.Method:
                    case AbcTraitKind.Setter:
                        {
                            var m = t.Method;
                            GetRefs(m, false, list, cache);
                        }
                        break;

                    case AbcTraitKind.Class:
                        {
                            var klass = t.Class;
                            GetRefs(klass.Instance, list, cache);
                        }
                        break;

                    case AbcTraitKind.Const:
                    case AbcTraitKind.Slot:
                        {
                            Add(t.SlotType, TypeRefTarget.Member, list, cache);
                        }
                        break;
                }
            }
        }

        static void GetRefs(AbcMethod m, bool fromCode, List<TypeRef> list, Hashtable cache)
        {
            var target = fromCode ? TypeRefTarget.Code : TypeRefTarget.Member;
            Add(m.ReturnType, target, list, cache);

            foreach (var p in m.Parameters)
                Add(p.Type, target, list, cache);

            var body = m.Body;
            if (body != null)
            {
                GetRefs(body.Traits, list, cache);

                foreach (var e in body.Exceptions)
                    Add(e.Type, TypeRefTarget.Code, list, cache);

                foreach (var instr in body.IL)
                    GetRefs(instr, list, cache);
            }
        }

        static bool ExcludeInstruction(Instruction instr)
        {
            if (!instr.HasOperands) return true;
            switch (instr.Code)
            {
                case InstructionCode.Getlex:
                case InstructionCode.Finddef:
                case InstructionCode.Findproperty:
                case InstructionCode.Findpropstrict:
                case InstructionCode.Getproperty:
                case InstructionCode.Setproperty:
                case InstructionCode.Initproperty:
                case InstructionCode.Constructprop:
                case InstructionCode.Applytype:
                case InstructionCode.Astype:
                case InstructionCode.Istype:
                case InstructionCode.Coerce:
                case InstructionCode.Instanceof:
                case InstructionCode.Newfunction:
                    return false;
            }
            return true;
        }

        static void GetRefs(Instruction instr, List<TypeRef> list, Hashtable cache)
        {
            if (ExcludeInstruction(instr)) return;
            foreach (var op in instr.Operands)
            {
                object v = op.Value;
                if (v == null) continue;

                var m = v as AbcMethod;
                if (m != null)
                {
                    GetRefs(m, true, list, cache);
                    continue;
                }

                var mn = v as AbcMultiname;
                if (mn != null)
                {
                    Add(mn, TypeRefTarget.Code, list, cache);
                }
            }
        }

        static void Add(AbcMultiname type, TypeRefTarget target, ICollection<TypeRef> list, Hashtable cache)
        {
            if (type == null) return;
            if (type.IsRuntime) return;

            if (type.IsParameterizedType)
            {
                Add(type.Type, target, list, cache);
                Add(type.TypeParameter, target, list, cache);
                return;
            }

            foreach (var fn in type.GetFullNames())
                Add(fn, target, list, cache);
        }

        static void Add(string fullname, TypeRefTarget target, ICollection<TypeRef> list, Hashtable cache)
        {
            if (string.IsNullOrEmpty(fullname)) return;
            if (cache.Contains(fullname)) return;
            cache[fullname] = fullname;
            list.Add(new TypeRef(fullname, target));
        }
    }
}