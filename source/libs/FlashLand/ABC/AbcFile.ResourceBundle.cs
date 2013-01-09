using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.FlashLand.Core;
using DataDynamics.PageFX.FlashLand.Core.ResourceBundles;
using DataDynamics.PageFX.FlashLand.Core.SwfGeneration;
using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Abc
{
    public partial class AbcFile
    {
        private IEnumerable<string> Locales
        {
            get
            {
                var sfc = SwfCompiler;
                return sfc == null ? SwfCompilerOptions.DefaultLocales : sfc.Locales;
            }
        }

        private bool ImportResourceBundle(AbcFile abc, AbcMetaEntry e)
        {
            if (e.NameString != MetadataTags.ResourceBundle)
                return false;

            string name = e.GetResourceBundleName();
            if (string.IsNullOrEmpty(name))
                throw Errors.RBC.BadMetaEntry.CreateException();

            var swc = abc.Swc;
            if (swc != null)
                swc.LoadResourceBundles();

            var context = new ResourceBundleContext { Swc = swc };

            ImportResourceBundle(name, context);
            return true;
        }

        public void ImportResourceBundle(string name)
        {
            ImportResourceBundle(name, (ResourceBundleContext)null);
        }

        public void ImportResourceBundle(string name, ResourceBundleContext context)
        {
            foreach (var locale in Locales)
                ImportResourceBundle(locale, name, context);
        }

        public void ImportResourceBundle(string locale, string name)
        {
            ImportResourceBundle(locale, name, null);
        }

        public void ImportResourceBundle(string locale, string name, ResourceBundleContext context)
        {
            if (context == null)
                context = new ResourceBundleContext();
            context.Locale = locale;

            ResourceBundles.CopyFlexLocale(locale);

            string key = locale + "$" + name;
            if (ContainsResourceBundle(key)) return;

            var rb = ResourceBundles.Get(locale, name);
            //NOTE: null in case of Dynamic Resource Modules!!!
            if (rb == null) return;

            var superType = ResourceBundleSuper;

            var instance = new AbcInstance(true)
                               {
                                   ResourceBundleName = name,
                                   Locale = locale
                               };

            _rbcache[key] = instance;

            //NOTE: naming is strongly determined in Flex Resource Manager.
            string fullname = locale + '$' + name + "_properties";
            instance.Name = DefineGlobalQName(fullname);
            instance.Flags = AbcClassFlags.Sealed | AbcClassFlags.ProtectedNamespace;
            instance.ProtectedNamespace = DefineProtectedNamespace(fullname);

            instance.BaseInstance = superType;
            instance.BaseTypeName = superType.Name;

            AddInstance(instance);

            instance.Initializer = DefineVoidMethod(
	            code =>
		            {
			            code.PushThisScope();
			            code.LoadThis();
			            code.PushString(locale);
			            code.PushString(name);
			            code.ConstructSuper(2);
			            code.ReturnVoid();
		            });

            instance.Class.Initializer = DefineEmptyMethod();
            
            var mn = DefineQName(instance.ProtectedNamespace, "getContent");
	        instance.DefineMethod(
		        Sig.@override(mn, AvmTypeCode.Object),
		        code =>
			        {
				        int n = 0;
				        var lines = rb.Content;
				        for (int i = 0; i < lines.Length; ++i)
				        {
					        string line = lines[i];
					        context.Line = i + 1;
					        context.ResourceBundle = rb;
					        if (PushKeyValue(line, code, context))
						        ++n;
				        }
				        code.Add(InstructionCode.Newobject, n);
				        code.ReturnValue();
			        }
		        );

            DefineScript(instance);
        }

        readonly Hashtable _rbcache = new Hashtable();

        private AbcInstance ResourceBundleSuper
        {
            get
            {
                //TODO: Check mx context
                if (_rbsuper == null)
                {
                    var superName = DefinePackageQName("mx.resources", "ResourceBundle");
                    _rbsuper = ImportInstance(superName);
                }
                return _rbsuper;
            }
        }
        private AbcInstance _rbsuper;

        private bool ContainsResourceBundle(string key)
        {
            var pf = PrevFrame;
            while (pf != null)
            {
                if (pf._rbcache.Contains(key))
                    return true;
                pf = pf.PrevFrame;
            }
            return _rbcache.Contains(key);
        }

        private const string PrefixEmbed = "Embed(";
		private const string PrefixClassReference = "ClassReference(";

        private bool PushKeyValue(string line, AbcCode code, ResourceBundleContext context)
        {
            if (string.IsNullOrEmpty(line)) return false;
            line = line.Trim();
            if (string.IsNullOrEmpty(line)) return false;
            if (line.IsResourceBundleComment()) return false;

            int sep = line.IndexOf('=');
            if (sep >= 0)
            {
                //TODO: process standard escape sequences
                string key = line.Substring(0, sep).Trim();
                string value = line.Substring(sep + 1).Trim();
                code.PushString(key);

                if (value.StartsWith(PrefixEmbed))
                {
                    var res = EmbedResource(context, value);
                    code.Getlex(res);
                    return true;
                }

                if (value.StartsWith(PrefixClassReference))
                {
                    var cr = ResolveClassRef(value);
                    code.Getlex(cr);
                    return true;
                }

                code.PushString(value);
                return true;
            }

            //TODO: Is it warning or error?
            return false;
        }

        private static Function ParseDirective(string value)
        {
            Function func;
            try
            {
                func = Function.Parse(value);
            }
            catch (Exception)
            {
                throw Errors.RBC.UnableToParseDirective.CreateException(value);
            }
            if (func == null)
                throw Errors.RBC.UnableToParseDirective.CreateException(value);
            return func;
        }

        private AbcInstance ResolveClassRef(string value)
        {
            var func = ParseDirective(value);
            if (func.Arguments.Count != 1)
                throw Errors.RBC.BadDirective.CreateException(value);
            string name = func.Arguments[0].Value as string;
            if (string.IsNullOrEmpty(name))
                throw Errors.RBC.BadDirective.CreateException(value);
            var instance = Generator.ImportType(name);
            if (instance == null)
                throw Errors.RBC.UnableToResolveClassReference.CreateException(value);
            return instance;
        }

        #region EmbedResource
        private AbcInstance EmbedResource(ResourceBundleContext context, string value)
        {
            var func = ParseDirective(value);
            return EmbedResource(context, func);
        }

        private AbcInstance EmbedResource(ResourceBundleContext context, Function func)
        {
            var embed = Embed.FromDirective(func);
            if (embed == null)
                throw Errors.RBC.UnableToResolveEmbed.CreateException(func);
            string type = embed.MimeType;
            string source = embed.Source;
            bool jpeg = false;
            if (MimeTypes.IsBitmap(type) || (jpeg = MimeTypes.IsJpeg(type)))
            {
                var image = ResolveImage(context, source);
                if (image == null)
                    throw Errors.RBC.UnableToResolveImage.CreateException(source);
                var name = DefineAssetName(context, "embedded_image_");
                var instance = _embedAssetInstances[name] as AbcInstance;
                if (instance != null) return instance;
                var mn = DefinePfxName(name);
                instance = Generator.DefineBitmapAsset(mn, image, jpeg);
                _embedAssetInstances[name] = instance;
                return instance;
            }

            //TODO: Support other mime-types
            throw Errors.RBC.NotSupportedMimeType.CreateException(embed.Source, embed.MimeType);
        }

        readonly Hashtable _embedAssetInstances = new Hashtable();

		private static string DefineAssetName(ResourceBundleContext context, string prefix)
        {
            string name = prefix;
            if (context.Swc != null)
            {
                name += context.Swc.Name;
                name += "_";
            }
            name += context.ResolvedSource;
            return name.ToValidName();
        }

        private static Image ResolveImage(ResourceBundleContext context, string source)
        {
            try
            {
                var rb = context.ResourceBundle;
                if (context.Swc != null)
                {
                    if (rb.IsZipped)
                    {
                        string dir = System.IO.Path.GetDirectoryName(rb.ZipEntry);
                        string imagePath = System.IO.Path.Combine(dir, source);
                        imagePath = imagePath.Replace('\\', '/');
                        imagePath = imagePath.ToFullPath();
                        context.ResolvedSource = imagePath;
                        return context.Swc.ResolveImage(imagePath);
                    }
                }
                return null;
            }
            catch (Exception)
            {
                throw Errors.RBC.UnableToResolveImage.CreateException(source);
            }
        }
        #endregion
    }
}