using System;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI
{
    internal partial class AbcGenerator
    {
        #region ImplementStringMethod
        void ImplementStringMethod(IMethod method)
        {
            var iface = method.DeclaringType;
            if (!iface.IsInterface) return;

            if (iface.Name.StartsWith("IEnumerable"))
            {
                ImplementStringGetEnumerator(method);
                return;
            }

            if (iface.Name.StartsWith("IComparable"))
            {
                DefineStringIComparableMethod(method);
                return;
            }

            if (iface.Name.StartsWith("IEquatable"))
            {
                DefineStringIEquatableMethod(method);
                return;
            }

            switch (iface.Name)
            {
                case "IConvertible":
                    DefineStringIConvertibleMethod(method);
                    return;

                case "ICloneable":
                    DefineStringICloneableMethod(method);
                    return;
            }
        }
        #endregion

        #region ImplementStringGetEnumerator
        //NOTE: Also used to implement IEnumerable<char>.GetEnumerator
        void ImplementStringGetEnumerator(IMethod method)
        {
            var m = method.Tag as AbcMethod;
            if (m == null) return;

            var CharEnumerator = CorlibTypes[CorlibTypeId.CharEnumerator];

            _newAPI.SetProtoFunction(
                AvmTypeCode.String, m,
                code =>
                    {
                        var ctor = CharEnumerator.FindConstructor(1);
                        code.NewObject(ctor, () => code.LoadThis());
                        code.ReturnValue();
                    });
        }
        #endregion

        #region DefineStringIConvertibleMethod
        void DefineStringIConvertibleMethod(IMethod method)
        {
            var m = method.Tag as AbcMethod;
            if (m == null) return;

            string name = method.Name;

            if (name == "ToString")
            {
                _newAPI.SetProtoFunction(AvmTypeCode.String, m.TraitName, DefineString_ToString());

                return;
            }

            if (name == "GetTypeCode")
            {
                _newAPI.SetProtoFunction(
                    AvmTypeCode.String, m,
                    code =>
                        {
                            code.PushInt(18);
                            code.ReturnValue();
                        });
                return;
            }

            var convertType = Corlib.Types.Convert;
            var convertInstance = DefineAbcInstance(convertType);

            int paramCount = method.Parameters.Count;
            IMethod convertMethod;
            if (name == "ToType")
            {
	            convertMethod = convertType.Methods.Find(method.Name, 3);
            }
            else
            {
                convertMethod =
                    convertType.FindMethod(method.Name, paramCount + 1,
                                  args => args[0].Type == SystemTypes.String);
            }

            if (convertMethod == null)
                throw new InvalidOperationException("Bad corlib");

            var cm = DefineAbcMethod(convertMethod);

            _newAPI.SetProtoFunction(
                AvmTypeCode.String, m,
                code =>
                    {
                        code.Getlex(convertInstance);
                        code.GetLocals(0, paramCount);
                        code.Call(cm);
                        code.ReturnValue();
                    });
        }
        #endregion

        #region DefineStringICloneableMethod
        void DefineStringICloneableMethod(IMethod method)
        {
            var m = method.Tag as AbcMethod;
            if (m == null) return;

            _newAPI.SetProtoFunction(
                AvmTypeCode.String, m,
                code =>
                {
                    code.LoadThis();
                    code.ReturnValue();
                });
        }
        #endregion

        #region DefineStringIComparableMethod
        //NOTE: Also used to implement IComparable<String>
        void DefineStringIComparableMethod(IMethod method)
        {
            var m = method.Tag as AbcMethod;
            if (m == null) return;

            var cmp = SystemTypes.String.Methods.Find("CompareTo", SystemTypes.Object);

            if (cmp == null)
                throw new InvalidOperationException("String has no CompareTo(object) method");

            var abccmp = DefineAbcMethod(cmp);

            _newAPI.SetProtoFunction(
                AvmTypeCode.String, m,
                code =>
                    {
                        code.Getlex(abccmp);
                        code.LoadThis();
                        code.GetLocal(1);
                        code.Call(abccmp);
                        code.ReturnValue();
                    });
        }
        #endregion

        #region DefineStringIEquatableMethod
        void DefineStringIEquatableMethod(IMethod method)
        {
            var m = method.Tag as AbcMethod;
            if (m == null) return;

            _newAPI.SetProtoFunction(
                AvmTypeCode.String, m,
                code =>
                    {
                        code.LoadThis();
                        code.GetLocal(1);
                        code.Add(InstructionCode.Equals);
                        code.FixBool();
                        code.ReturnValue();
                    });
        }
        #endregion
    }
}