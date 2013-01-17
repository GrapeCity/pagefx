using System;
using System.Linq;
using DataDynamics.PageFX.Common.CompilerServices;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.Swf;
using DataDynamics.PageFX.FlashLand.Swf.Tags.Control;

namespace DataDynamics.PageFX.FlashLand.Core
{
    internal sealed class Embed
    {
        private static class Attrs
        {
            public const string Source = "source";
            public const string Symbol = "symbol";
            public const string ExportSymbol = "exportSymbol";
        }

        public Embed()
        {
        }

        public Embed(Embed other)
        {
            Source = other.Source;
            MimeType = other.MimeType;
            Symbol = other.Symbol;
            ExportSymbol = other.ExportSymbol;
        }

        public string Source;
        public string MimeType;
        public string Symbol; //used to embed swiff asset.
        public string ExportSymbol;

        /// <summary>
        /// Asset to import
        /// </summary>
        public SwfAsset Asset;

        /// <summary>
        /// SWF file from which tag will be imported.
        /// </summary>
        public SwfMovie Movie;

        public AbcInstance Instance;

	    public static void Resolve(AbcTrait trait, AbcMetaEntry e, SwfMovie lib)
        {
            if (trait.Embed != null) return;

            var klass = trait.Class;
            if (klass == null)
            {
                throw new InvalidOperationException("Embed can be applied to class trait only");
            }

            string symbol = e[Attrs.Symbol];
            string exportSymbol = e[Attrs.ExportSymbol];
            string source = e[Attrs.Source];

            var asset = lib.FindAsset(symbol);
            if (asset == null)
            {
                asset = lib.FindAsset(exportSymbol);
                if (asset == null)
                    throw Errors.Linker.UnableToFindSymbol.CreateException(symbol);
                asset.IsExported = true;
            }

            var instance = klass.Instance;

            var embed = new Embed
                            {
                                Symbol = symbol,
                                Asset = asset,
                                Movie = lib,
                                Source = source,
                                ExportSymbol = exportSymbol,
                                Instance = instance
                            };

            trait.Embed = embed;
            trait.AssetInstance = instance;
        }

		public static void Apply(AbcTrait trait, SwfAsset asset, SwfMovie lib)
		{
			if (trait.Embed != null) return;

			var klass = trait.Class;
			if (klass == null)
			{
				throw new InvalidOperationException("Embed can be applied to class trait only");
			}

			var instance = klass.Instance;

			var embed = new Embed
			            	{
			            		Asset = asset,
			            		Movie = lib,
			            		Instance = instance
			            	};

			trait.Embed = embed;
			trait.AssetInstance = instance;
		}

	    public static Embed FromDirective(Function f)
        {
            int n = f.Arguments.Count;
            if (n <= 0) return null;

            if (n == 1) //source
            {
                string source = f.Arguments[0].Value as string;
                if (String.IsNullOrEmpty(source))
                    throw Errors.RBC.UnableToResolveEmbed.CreateException(f.ToString());
                return new Embed { Source = source, MimeType = MimeTypes.AutoDetect(source) };
            }

			var src = f.Arguments.FirstOrDefault(a => a.Name == null || String.Equals(a.Name, "source", StringComparison.OrdinalIgnoreCase));
            if (src != null)
            {
                string source = src.Value as string;
                if (String.IsNullOrEmpty(source))
                    throw Errors.RBC.UnableToResolveEmbed.CreateException(f.ToString());
                string mt = MimeTypes.AutoDetect(source);
                mt = GetMimeType(f, mt);
                return new Embed { Source = source, MimeType = mt };
            }

            return null;
        }

        private static string GetMimeType(Function f, string defval)
        {
            var mt = f.Find("mimeType", true);
            if (mt != null)
            {
                string s = mt.Value as string;
                if (!String.IsNullOrEmpty(s))
                {
                    if (MimeTypes.IsSupported(s))
                        return s;
                }
            }
            return defval;
        }

	    public static Embed FromCustomAttribute(ICustomAttribute attr)
	    {
		    var embed = new Embed();

		    int n = attr.Arguments.Count;
		    if (n == 1) //only source, mime-type is auto detected
		    {
			    embed.Source = attr.Arguments[0].Value as string;
			    if (String.IsNullOrEmpty(embed.Source))
				    throw new InvalidOperationException("Invalid source in EmbedAttribute");
			    embed.MimeType = MimeTypes.AutoDetect(embed.Source);
			    if (String.IsNullOrEmpty(embed.MimeType))
				    throw new InvalidOperationException("Unable to auto detect mimeType in EmbedAttribute");
		    }
		    else if (n == 2) //source and mime-type are given
		    {
			    embed.Source = attr.Arguments[0].Value as string;
			    if (String.IsNullOrEmpty(embed.Source))
				    throw new InvalidOperationException("Invalid source in EmbedAttribute");
			    embed.MimeType = attr.Arguments[1].Value as string;
			    if (String.IsNullOrEmpty(embed.MimeType))
				    throw new InvalidOperationException("Invalid mimeType in EmbedAttribute");
		    }
		    else
		    {
			    throw new NotImplementedException();
		    }

		    MimeTypes.Check(embed.MimeType);

		    return embed;
	    }
    }
}