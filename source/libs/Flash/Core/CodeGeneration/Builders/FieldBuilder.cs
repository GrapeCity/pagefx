using System;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.Abc;
using DataDynamics.PageFX.Flash.Core.Tools;

namespace DataDynamics.PageFX.Flash.Core.CodeGeneration.Builders
{
    //contains code to define IField in output ABC file
    internal sealed class FieldBuilder
    {
	    private readonly AbcGenerator _generator;

	    public FieldBuilder(AbcGenerator generator)
		{
			_generator = generator;
		}

	    private AbcFile Abc
	    {
			get { return _generator.Abc; }
	    }

	    public void Build(IField field)
        {
            if (field == null)
                throw new ArgumentNullException("field");

            if (MustExclude(field))
                return;    

            if (Abc.IsDefined(field))
                return;

            var declType = field.DeclaringType;
			var tag = _generator.TypeBuilder.Build(declType);

            var instance = tag as AbcInstance;
            if (instance == null)
                throw new InvalidOperationException();

            if (instance.IsForeign) return;

            if (Abc.IsDefined(field)) return;

			var type = _generator.TypeBuilder.BuildMemberType(field.Type);
            if (Abc.IsDefined(field)) return;

#if DEBUG
		    DebugService.LogInfo("ABC DefineField started for field {0}.{1}", field.DeclaringType.FullName, field.Name);
#endif

            var name = DefineName(field);
            bool isStatic = field.IsStatic;
            AbcTrait trait = null;

            //Try to find trait, may be it has already been defined
            if (field.Data != null)
            {
            	var kind = field.IsConstant ? AbcTraitKind.Const : AbcTraitKind.Slot;
            	trait = isStatic ? instance.Class.Traits.Find(name, kind) : instance.Traits.Find(name, kind);
            }

            if (trait == null)
            {
                trait = AbcTrait.CreateSlot(type, name);
                _generator.SetData(field, trait);
                instance.AddTrait(trait, isStatic);

                if (IsImportableConstant(field))
                {
                    trait.Kind = AbcTraitKind.Const;
                    trait.HasValue = true;
                    trait.SlotValue = Abc.ImportValue(field.Value);
                }

                _generator.EmbeddedAssets.Build(field, trait);
            }
            else
            {
                _generator.SetData(field, trait);
            }

            trait.Type = field.Type;
            trait.Field = field;

#if DEBUG
            DebugService.LogInfo("ABC DefineField succeeded for field {0}", field.FullName);
#endif
        }

	    public AbcMultiname GetFieldName(IField field)
        {
            Build(field);
            var trait = field.Data as AbcTrait;
            if (trait == null)
                throw new InvalidOperationException();
            return trait.Name;
        }

        private static bool UseGlobalPackage(IType declType)
        {
            if (declType.IsEnum)
                return true;

            var st = declType.SystemType();
            if (st != null)
                return true;

            if (declType.IsNullableInstance())
                return true;

            return false;
        }

        private AbcNamespace DefineNamespace(IField field)
        {
            var declType = field.DeclaringType;
            if (UseGlobalPackage(declType))
                return Abc.KnownNamespaces.GlobalPackage;

            return Abc.DefineNamespace(declType, field.Visibility, field.IsStatic);
        }

		private AbcMultiname DefineName(IField field)
        {
            //NOTE: For enums we will use m_value name for internal value.
            field.Rename();
            var ns = DefineNamespace(field);
            return Abc.DefineQName(ns, field.Name);
        }

	    private static bool IsImportableConstant(IField field)
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

		private static bool MustExclude(IField field)
		{
			var declType = field.DeclaringType;
			if (declType.IsEnum)
			{
				if (field.IsStatic)
					return AbcGenConfig.ExludeEnumConstants;
				return true;
			}
			if (field.IsArrayInitializer())
				return true;
			return false;
		}
    }
}