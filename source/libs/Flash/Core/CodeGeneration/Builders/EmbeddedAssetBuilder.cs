using System;
using System.Drawing;
using DataDynamics.PageFX.Common.CompilerServices;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.Abc;

namespace DataDynamics.PageFX.Flash.Core.CodeGeneration.Builders
{
    internal sealed class EmbeddedAssetBuilder
    {
	    private readonly AbcGenerator _generator;

	    public EmbeddedAssetBuilder(AbcGenerator generator)
		{
			_generator = generator;
		}

	    private AbcFile Abc
	    {
			get { return _generator.Abc; }
	    }

	    public void Build(IField field, AbcTrait trait)
        {
            var attr = field.FindAttribute(Attrs.Embed);
            if (attr == null) return;

			_generator.CheckEmbedAsset(field);

            var embed = Embed.FromCustomAttribute(attr);
            trait.Embed = embed;

            string type = embed.MimeType;
            if (MimeTypes.IsBitmap(type))
            {
                var instance = BuildBitmapAsset(trait);
				_generator.SwfCompiler.Assets.DefineBitmapAsset(embed.Source, instance);
                return;
            }

            if (MimeTypes.IsJpeg(type))
            {
                var instance = BuildBitmapAsset(trait);
				_generator.SwfCompiler.Assets.DefineJpegAsset(embed.Source, instance);
                return;
            }

		    if (string.Equals(type, MimeTypes.Application.OctetStream, StringComparison.OrdinalIgnoreCase))
		    {
				var instance = BuildByteArrayAsset(trait);
				_generator.SwfCompiler.Assets.DefineByteArrayAsset(embed.Source, instance);
				return;
		    }

            //TODO: Support other mime-types
            throw Errors.RBC.NotSupportedMimeType.CreateException(embed.Source, embed.MimeType);
        }

	    private AbcMultiname GetAssetInstanceName(AbcTrait trait)
        {
            var owner = trait.Instance;
            string name = owner.NameString + "_" + trait.Name.NameString;
            return Abc.DefineQName(owner.Name.Namespace, name);
        }

	    private AbcMultiname GetBitmapAssetSuperName()
	    {
			// for flash application we use flash.display.Bitmap
		    var qname = _generator.IsFlexApplication
			    ? QName.Package("mx.core", "BitmapAsset")
			    : QName.Package("flash.display", "Bitmap");
		    return Abc.DefineName(qname);
        }

	    public AbcInstance BuildBitmapAsset(AbcMultiname name, Image image, bool jpeg)
        {
            var instance = BuildAssetInstance(name, GetBitmapAssetSuperName());
			_generator.SwfCompiler.Assets.DefineBitmapAsset(image, instance, jpeg);
            return instance;
        }

        private AbcInstance BuildBitmapAsset(AbcTrait trait)
        {
            return BuildAssetInstance(trait, GetBitmapAssetSuperName());
        }

		private AbcInstance BuildByteArrayAsset(AbcTrait trait)
		{
			var qname = _generator.IsFlexApplication
				? QName.Package("mx.core", "ByteArrayAsset")
				: QName.Package("flash.utils", "ByteArray");
			return BuildAssetInstance(trait, Abc.DefineName(qname));
		}
        
        private AbcInstance BuildAssetInstance(AbcTrait trait, AbcMultiname superName)
        {
            var name = GetAssetInstanceName(trait);
            //TODO: Check existance of instance

            var instance = BuildAssetInstance(name, superName);
            instance.Embed = trait.Embed;
            trait.AssetInstance = instance;

            return instance;
        }

        private AbcInstance BuildAssetInstance(AbcMultiname name, AbcMultiname superName)
        {
            var superType = FindAssetSuperType(superName);
            if (superType == null)
                throw Errors.Type.UnableToFind.CreateException(superName.FullName);

            //TODO: Check existance of instance

            var instance = new AbcInstance(true)
	            {
		            Name = name,
		            BaseTypeName = superName,
		            Flags = (AbcClassFlags.Sealed | AbcClassFlags.ProtectedNamespace),
		            ProtectedNamespace = Abc.DefineProtectedNamespace(name.NameString),
		            Initializer = Abc.DefineMethod(
			            Sig.@void(),
			            code =>
				            {
					            code.PushThisScope();
					            code.ConstructSuper();
					            code.ReturnVoid();
				            }),
		            BaseInstance = superType,
		            Class = {Initializer = Abc.DefineMethod(
			            Sig.global(null),
			            code =>
				            {
					            if (true)
						            code.PushThisScope();
					            code.ReturnVoid();
				            })}
	            };

	        Abc.AddInstance(instance);

	        return instance;
        }

        private AbcInstance FindAssetSuperType(AbcMultiname superName)
        {
			var type = _generator.FindTypeDefOrRef(superName.FullName);
            if (type == null)
                throw new InvalidOperationException();

            var instance = type.AbcInstance();
            if (instance == null)
                throw new InvalidOperationException();

            instance = Abc.ImportInstance(instance);

            return instance;
        }
    }
}