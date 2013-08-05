using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DataDynamics.PageFX.Common.CompilerServices;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.Abc;
using DataDynamics.PageFX.Flash.Swf;
using DataDynamics.PageFX.Flash.Swf.Tags;
using DataDynamics.PageFX.Flash.Swf.Tags.Control;

namespace DataDynamics.PageFX.Flash.Core.SwfGeneration
{
    internal sealed class AssetCompiler
    {
	    private readonly SwfCompiler _compiler;
	    private readonly Hashtable _charCache = new Hashtable();
		private readonly Hashtable _imageCache = new Hashtable();
		private readonly SwfAssetCollection _symbols = new SwfAssetCollection();
		private readonly SwfAssetCollection _exports = new SwfAssetCollection();

		public AssetCompiler(SwfCompiler compiler)
		{
			_compiler = compiler;
		}

	    private SwfMovie Swf
	    {
			get { return _compiler.Swf; }
	    }

	    private Image ResolveImageResoucre(string source)
        {
            var res = _compiler.AppAssembly.FindResource(source);
            if (res == null)
                throw Errors.SWF.UnableToFindImageResource.CreateException(source);

            try
            {
                var data = res.Data;
                return Image.FromStream(data);
            }
            catch (Exception exc)
            {
                throw Errors.SWF.BadImageResource.CreateInnerException(exc, source);
            }
        }

        private ISwfCharacter DefineImageAsset(string source, Converter<Image,SwfCharacter> creator)
        {
            var c = _imageCache[source] as ISwfCharacter;
            if (c == null)
            {
                var image = ResolveImageResoucre(source);
                c = creator(image);
                _imageCache[source] = c;
            }
            return c;
        }

        public void DefineBitmapAsset(string source, AbcInstance instance)
        {
            var c = DefineImageAsset(source, Swf.CreateBitmapTag);
            AddSymbol(c, instance);
        }

        public void DefineBitmapAsset(Image image, AbcInstance instance)
        {
            var c = Swf.CreateBitmapTag(image);
            AddSymbol(c, instance);
        }

        public void DefineJpegAsset(string source, AbcInstance instance)
        {
            var c = DefineImageAsset(source, Swf.CreateJpegTag);
            AddSymbol(c, instance);
        }

        public void DefineJpegAsset(Image image, AbcInstance instance)
        {
            var c = Swf.CreateJpegTag(image);
            AddSymbol(c, instance);
        }

        public void DefineBitmapAsset(Image image, AbcInstance instance, bool jpeg)
        {
            if (jpeg)
                DefineJpegAsset(image, instance);
            else
                DefineBitmapAsset(image, instance);
        }

	    /// <summary>
        /// Imports embedded asset.
        /// </summary>
        /// <param name="embed">contains info to locate asset to import.</param>
        /// <returns></returns>
        public SwfAsset Import(Embed embed)
        {
            if (embed == null)
                throw new ArgumentNullException("embed");
            if (embed.Asset == null)
                throw new ArgumentException("embed.Asset is null");

            if (DeferredAssetImport)
            {
                _lateAssets.Add(embed);
                return embed.Asset;
            }

            if (LateAssetNames.Contains(embed.Asset.Name))
            {
                _lateAssets.Add(embed);
                return embed.Asset;
            }

            return ImportCore(embed);
        }

        private SwfAsset ImportCore(Embed embed)
        {
            var c = ImportCharacter(embed.Movie, embed.Asset);
            return AddSymbol(c, embed.Instance);
        }

        private ISwfCharacter ImportCharacter(SwfMovie from, SwfAsset asset)
        {
            var c = _charCache[asset.Name] as ISwfCharacter;
            if (c != null) return c;

            var tag = asset.Character as SwfTag;
            CheckCharacter(tag);

            var mytag = Swf.Import(from, tag);
            CheckCharacter(mytag);
            c = (ISwfCharacter)mytag;

            if (asset.IsExported)
                _exports.Add(c, asset.Name);

            _charCache[asset.Name] = c;

            return c;
        }

        private static readonly string[] LateAssetNames =
            {
                //"mx.controls.RichTextEditor__embed_mxml_assets_icon_align_left_png_1838390231",
                //"mx.controls.RichTextEditor__embed_mxml_assets_icon_align_right_png_1943059559",
                //"mx.controls.RichTextEditor__embed_mxml_assets_icon_align_center_png_1776178135",
                //"mx.controls.RichTextEditor__embed_mxml_assets_icon_style_underline_png_1069931999",
                //"mx.controls.RichTextEditor__embed_mxml_assets_icon_bullet_png_1274773207",
                //"mx.controls.RichTextEditor__embed_mxml_assets_icon_align_justify_png_2129348769",
                //"mx.controls.RichTextEditor__embed_mxml_assets_icon_style_italic_png_1808113631",
                //"mx.controls.RichTextEditor__embed_mxml_assets_icon_style_bold_png_1670544607",
            };

        private readonly List<Embed> _lateAssets = new List<Embed>();
        private static readonly bool DeferredAssetImport = true;

        public void ImportLateAssets()
        {
            if (_lateAssets.Count > 0)
            {
                if (DeferredAssetImport)
                {
                    foreach (var embed in _lateAssets)
                    {
                        ImportCore(embed);
                    }
                }
                else
                {
                    foreach (var name in LateAssetNames)
                    {
						var embed = _lateAssets.FirstOrDefault(e => e.Asset.Name == name);
                        if (embed != null)
                        {
                            ImportCore(embed);
                        }
                    }
                }

                _lateAssets.Clear();
            }
        }

        private static void CheckCharacter(SwfTag tag)
        {
            if (tag == null)
                throw new ArgumentNullException("tag");
            var obj = tag as ISwfCharacter;
            if (obj == null)
                throw Errors.SWF.TagIsNotCharacter.CreateException();
        }

	    private SwfAsset AddSymbol(ISwfCharacter obj, AbcInstance instance)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            if (instance == null)
                throw new ArgumentNullException("instance");
            var tag = obj as SwfTag;
            if (tag == null)
                throw new ArgumentException("Character is not swf tag");
            if (!Swf.Tags.Contains(tag))
                Swf.Tags.Add(tag);
            string name = instance.FullName;
            var asset = new SwfAsset(obj, name)
                            {
                                IsSymbol = true,
                            };
            _symbols.Add(asset);
            return asset;
        }

        //NOTE: assets should be defined before ABC file.
        public void FlushAssets(SwfTagSymbolClass table)
        {
            if (_exports.Count > 0)
            {
                var export = new SwfTagExportAssets();
                export.Assets.AddRange(_exports);
                Swf.Tags.Add(export);
                _exports.Clear();
            }

            if (_symbols.Count > 0)
            {
                //_symbols.Sort(delegate (SwfAsset a, SwfAsset b)
                //                  {
                //                      AbcScript s1 = a.Class.Script;
                //                      AbcScript s2 = b.Class.Script;
                //                      return s1.Index - s2.Index;
                //                  });

                table.Symbols.AddRange(_symbols);
                _symbols.Clear();
            }
        }
    }
}