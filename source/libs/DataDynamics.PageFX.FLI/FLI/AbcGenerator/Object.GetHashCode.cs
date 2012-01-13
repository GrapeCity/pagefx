using DataDynamics.PageFX.FLI.ABC;

namespace DataDynamics.PageFX.FLI
{
    partial class AbcGenerator
    {
        AbcMethod HashCodeCalc()
        {
            var instance = DefineRuntimeInstance();

            return instance.DefineStaticMethod(
                "get_hash_code", AvmTypeCode.Int32,
                code =>
                {
                    var getDic = GetHashCodeDic();

                    const int varKey = 1;
                    const int varDic = 2;
                    const int varHC = 3;

                    code.PushInt(0);
                    code.SetLocal(varHC);

                    code.LoadThis();
                    code.Call(getDic);
                    code.SetLocal(varDic);

                    code.GetLocal(varDic);
                    code.GetLocal(varKey);
                    code.GetNativeArrayItem(); //[]
                    code.CoerceInt32();
                    code.SetLocal(varHC);

                    code.GetLocal(varHC);
                    code.PushInt(0);
                    var br = code.IfNotEquals();

                    code.GetLocal(varDic);
                    code.GetLocal(varKey);
                    code.CallStatic(GetMethod(ObjectMethodId.NewHashCode));
                    code.SetLocal(varHC);
                    code.GetLocal(varHC);
                    code.SetNativeArrayItem(); //dic[key] = value

                    br.BranchTarget = code.Label();
                    code.GetLocal(varHC);
                    code.ReturnValue();
                },
                AvmTypeCode.Object);
        }

        AbcMethod GetHashCodeDic()
        {
            var instance = DefineRuntimeInstance();

            var dicType = _abc.DefineQName("flash.utils", "Dictionary");
            var dic = instance.DefineStaticSlot("hcdic$", dicType);

            return instance.DefineStaticMethod(
                "get_hashcode_dic", dicType,
                code =>
                {
                    code.LoadThis();
                    code.GetProperty(dic);
                    var br = code.IfNotNull();

                    code.LoadThis();
                    code.CreateInstance(dicType);
                    code.SetProperty(dic);

                    br.BranchTarget = code.Label();
                    code.LoadThis();
                    code.GetProperty(dic);
                    code.ReturnValue();
                });
        }
    }
}