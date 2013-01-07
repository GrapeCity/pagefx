using System;
using System.Drawing;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FlashLand.Core.CodeGeneration
{
    internal partial class AbcGenerator
    {
        #region GetEmbedInfo
        static Embed GetEmbedInfo(ICustomAttribute attr)
        {
            var embed = new Embed();
            int n = attr.Arguments.Count;
            if (n == 1) //only source, mime-type is auto detected
            {
                embed.Source = attr.Arguments[0].Value as string;
                if (string.IsNullOrEmpty(embed.Source))
                    throw new InvalidOperationException("Invalid source in EmbedAttribute");
                embed.MimeType = MimeTypes.AutoDetect(embed.Source);
                if (string.IsNullOrEmpty(embed.MimeType))
                    throw new InvalidOperationException("Unable to auto detect mimeType in EmbedAttribute");
            }
            else if (n == 2) //source and mime-type are given
            {
                embed.Source = attr.Arguments[0].Value as string;
                if (string.IsNullOrEmpty(embed.Source))
                    throw new InvalidOperationException("Invalid source in EmbedAttribute");
                embed.MimeType = attr.Arguments[1].Value as string;
                if (string.IsNullOrEmpty(embed.MimeType))
                    throw new InvalidOperationException("Invalid mimeType in EmbedAttribute");
            }
            else
            {
                throw new NotImplementedException();
            }
            MimeTypes.Check(embed.MimeType);
            return embed;
        }
        #endregion

        #region DefineEmbeddedAsset
        void DefineEmbeddedAsset(IField field, AbcTrait trait)
        {
            var attr = field.FindAttribute(Attrs.Embed);
            if (attr == null) return;

            CheckEmbedAsset(field);

            var embed = GetEmbedInfo(attr);
            trait.Embed = embed;

            string type = embed.MimeType;
            if (MimeTypes.IsBitmap(type))
            {
                var instance = DefineBitmapAsset(trait);
                SwfCompiler.DefineBitmapAsset(embed.Source, instance);
                return;
            }

            if (MimeTypes.IsJpeg(type))
            {
                var instance = DefineBitmapAsset(trait);
                SwfCompiler.DefineJpegAsset(embed.Source, instance);
                return;
            }

            //TODO: Support other mime-types
            throw Errors.RBC.NotSupportedMimeType.CreateException(embed.Source, embed.MimeType);
        }
        #endregion

        #region DefineAssetInstanceName
        AbcMultiname DefineAssetInstanceName(AbcTrait trait)
        {
            var owner = trait.Instance;
            string name = owner.NameString + "_" + trait.Name.NameString;
            return Abc.DefineQName(owner.Name.Namespace, name);
        }
        #endregion

        #region DefineAssetInstance
        public AbcInstance DefineAssetInstance(AbcTrait trait, string superNS, string superName)
        {
            var super = Abc.DefinePackageQName(superNS, superName);
            return DefineAssetInstance(trait, super);
        }

        public AbcInstance DefineAssetInstance(AbcMultiname name, string superNS, string superName)
        {
            var super = Abc.DefinePackageQName(superNS, superName);
            return DefineAssetInstance(name, super);
        }

        AbcMultiname GetBitmapAssetSuperName()
        {
            //TODO: For flash application it can flash.display.Bitmap
            return Abc.DefineQName("mx.core", "BitmapAsset");
        }

        public AbcInstance DefineBitmapAsset(AbcMultiname name)
        {
            return DefineAssetInstance(name, GetBitmapAssetSuperName());
        }

        public AbcInstance DefineBitmapAsset(AbcMultiname name, Image image, bool jpeg)
        {
            var instance = DefineAssetInstance(name, GetBitmapAssetSuperName());
            SwfCompiler.DefineBitmapAsset(image, instance, jpeg);
            return instance;
        }

        AbcInstance DefineBitmapAsset(AbcTrait trait)
        {
            return DefineAssetInstance(trait, GetBitmapAssetSuperName());
        }
        
        AbcInstance DefineAssetInstance(AbcTrait trait, AbcMultiname superName)
        {
            var name = DefineAssetInstanceName(trait);
            //TODO: Check existance of instance

            var instance = DefineAssetInstance(name, superName);
            instance.Embed = trait.Embed;
            trait.AssetInstance = instance;

            return instance;
        }

        AbcInstance DefineAssetInstance(AbcMultiname name, AbcMultiname superName)
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
		            Initializer = Abc.DefineEmptyConstructor((string)null, true),
		            BaseInstance = superType,
		            Class = {Initializer = Abc.DefineEmptyMethod(true)}
	            };

	        AddInstance(instance);

            return instance;
        }

        private AbcInstance FindAssetSuperType(AbcMultiname superName)
        {
            var type = FindTypeDefOrRef(superName.FullName);
            if (type == null)
                throw new InvalidOperationException();

            var instance = type.AbcInstance();
            if (instance == null)
                throw new InvalidOperationException();

            instance = Abc.ImportInstance(instance);

            return instance;
        }
        #endregion
    }
}