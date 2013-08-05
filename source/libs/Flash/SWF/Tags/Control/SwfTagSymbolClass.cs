using System;
using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags.Control
{
    /// <summary>
    /// The SymbolClass tag creates associations between symbols in the SWF file and ActionScript 3.0 classes.
    /// </summary>
    [SwfTag(SwfTagCode.SymbolClass)]
    public sealed class SwfTagSymbolClass : SwfTag, ISwfAssetContainer
    {
	    public SwfTagSymbolClass()
        {
        }

        public SwfTagSymbolClass(params object[] args)
        {
            if (args != null)
            {
                int n = args.Length;
                if ((n & 1) != 0)
                    throw new ArgumentException("");
                for (int i = 0; i < n; i += 2)
                {
                    var id = args[i] as IConvertible;
                    if (id == null)
                        throw new ArgumentException("Invalid symbol id");
                    string name = args[i + 1] as string;
                    if (string.IsNullOrEmpty(name))
                        throw new ArgumentException("Invalid symbol name");
                    AddSymbol((ushort)id.ToInt16(null), name);
                }
            }
        }

	    public SwfAssetCollection Symbols
        {
            get { return _symbols; }
        }
        private readonly SwfAssetCollection _symbols = new SwfAssetCollection();

	    SwfAssetCollection ISwfAssetContainer.Assets
	    {
		    get { return Symbols; }
	    }

	    public void AddSymbol(ushort id, string name)
        {
        	Symbols.Add(new SwfAsset(id, name)
        	            	{
        	            		IsSymbol = true,
        	            		Name = name
        	            	});
        }

	    public void AddSymbol(ISwfCharacter obj, string name)
        {
        	Symbols.Add(new SwfAsset(obj, name)
        	            	{
        	            		IsSymbol = true,
        	            		Name = name
        	            	});
        }

	    public override SwfTagCode TagCode
        {
            get { return SwfTagCode.SymbolClass; }
        }

	    public override void ReadTagData(SwfReader reader)
        {
            _symbols.Read(reader, SwfAssetFlags.Symbol);
        }

	    public override void WriteTagData(SwfWriter writer)
        {
            _symbols.Write(writer);
        }

	    public override void DumpBody(XmlWriter writer)
        {
            _symbols.DumpXml(writer, "symbols", "symbol");
        }
    }
}