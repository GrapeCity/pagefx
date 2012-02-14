using System;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.ABC;

namespace DataDynamics.PageFX.FLI
{
    //contains code to define IField in output ABC file
    partial class AbcGenerator
    {
        #region DefineField
        static bool MustExcludeField(IField field)
        {
            var declType = field.DeclaringType;
            if (declType.IsEnum)
            {
                if (field.IsStatic)
                    return AbcGenConfig.ExludeEnumConstants;
                return true;
            }
            if (TypeHelper.IsArrayInitializer(field.Type))
                return true;
            return false;
        }

        public void DefineField(IField field)
        {
            if (field == null)
                throw new ArgumentNullException("field");

            if (MustExcludeField(field))
                return;    

            if (_abc.IsDefined(field))
                return;

            var declType = field.DeclaringType;
            var tag = DefineType(declType);

#if DEBUG
            DebugService.DoCancel();
#endif

            var instance = tag as AbcInstance;
            if (instance == null)
                throw new InvalidOperationException();

            if (instance.IsForeign) return;

#if DEBUG
            DebugService.DoCancel();
#endif

            if (_abc.IsDefined(field)) return;

            var type = DefineMemberType(field.Type);
            if (_abc.IsDefined(field)) return;

#if DEBUG
            DebugService.DoCancel();
            DebugService.LogInfo("ABC DefineField started for field {0}.{1}", field.DeclaringType.FullName, field.Name);
#endif

            var name = DefineFieldName(field);
            bool isStatic = field.IsStatic;
            AbcTrait trait = null;

            //Try to find trait, may be it has already been defined
            if (field.Tag != null)
            {
            	var kind = field.IsConstant ? AbcTraitKind.Const : AbcTraitKind.Slot;
            	trait = isStatic ? instance.Class.Traits.Find(name, kind) : instance.Traits.Find(name, kind);
            }

#if DEBUG
            DebugService.DoCancel();
#endif

            if (trait == null)
            {
                trait = AbcTrait.CreateSlot(type, name);
                field.Tag = trait;
                instance.AddTrait(trait, isStatic);

                if (IsImportableConstantField(field))
                {
                    trait.Kind = AbcTraitKind.Const;
                    trait.HasValue = true;
                    trait.SlotValue = _abc.ImportValue(field.Value);
                }

                DefineEmbeddedAsset(field, trait);
            }
            else
            {
                field.Tag = trait;
            }
            trait.Type = field.Type;
            trait.Field = field;

#if DEBUG
            DebugService.LogInfo("ABC DefineField succeeded for field {0}", field.FullName);
            DebugService.DoCancel();
#endif
        }
        #endregion

        #region GetFieldName
        public AbcMultiname GetFieldName(IField field)
        {
            DefineField(field);
            var trait = field.Tag as AbcTrait;
            if (trait == null)
                throw new InvalidOperationException();
            return trait.Name;
        }

        static bool UseGlobalPackage(IType declType)
        {
            if (declType.IsEnum)
                return true;

            var st = declType.SystemType;
            if (st != null)
                return true;

            if (TypeService.IsNullableInstance(declType))
                return true;

            return false;
        }

        AbcNamespace DefineFieldNamespace(IField field)
        {
            var declType = field.DeclaringType;
            if (UseGlobalPackage(declType))
                return _abc.GlobalPackage;

            return _abc.DefineNamespace(declType, field.Visibility, field.IsStatic);
        }

        AbcMultiname DefineFieldName(IField field)
        {
            //NOTE: For enums we will use m_value name for internal value.
            AvmHelper.RenameField(field);
            var ns = DefineFieldNamespace(field);
            return _abc.DefineQName(ns, field.Name);
        }
        #endregion

        #region Utils
        public AbcMultiname DefineMemberType(IType type)
        {
            DefineType(type);
            if (type.Tag == null)
                throw new InvalidOperationException(string.Format("Type {0} is not defined", type.FullName));
            return _abc.GetTypeName(type, true);
        }

        static bool IsImportableConstantField(IField field)
        {
            if (!field.IsConstant) return false;

            var v = field.Value;
            if (v == null) return false;

            var declType = field.DeclaringType;
            if (declType.IsEnum && AbcGenConfig.InitEnumFields)
                return false;
            
            switch (Type.GetTypeCode(v.GetType()))
            {
                case TypeCode.Decimal:
                case TypeCode.UInt64:
                case TypeCode.Int64:
                case TypeCode.DateTime:
                case TypeCode.DBNull:
                case TypeCode.Empty:
                case TypeCode.Object:
                    return false;
            }
            return true;
        }

    	#endregion
    }
}