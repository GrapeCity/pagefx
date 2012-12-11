using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Ecma335.IL;

namespace DataDynamics.PageFX.Ecma335.Pdb.Interop
{
    internal sealed class PdbReader : ISymbolLoader
    {
	    private PdbReader()
		{
		}

	    internal ISymbolReader SymbolReader { get; private set; }

	    public void Dispose()
        {
	        SymbolReader = null;
        }

	    public bool LoadSymbols(IClrMethodBody body)
	    {
			var symMethod = GetSymbolMethod(SymbolReader, body.Method);
			if (symMethod == null) return false;

			var points = ReadSequencePoints(symMethod);
			body.SetSequencePoints(points);

			SetVariableNames(body.LocalVariables, symMethod.RootScope);

		    return true;
	    }

		public static ISymbolLoader Create(string path)
	    {
		    if (!Path.IsPathRooted(path))
			    path = Path.Combine(Environment.CurrentDirectory, path);
		    return Create(path, Path.GetDirectoryName(path));
	    }

		public static ISymbolLoader Create(string path, string searchPath)
	    {
		    if (!Path.IsPathRooted(path))
			    path = Path.Combine(Environment.CurrentDirectory, path);
		    return Create(new SymBinder(), path, searchPath);
	    }

	    [Flags]
	    private enum COR_OPEN_FLAGS : uint
	    {
		    ofRead = 0x00000000,     // Open scope for read
		    ofWrite = 0x00000001,     // Open scope for write.
		    ofReadWriteMask = 0x00000001,     // Mask for read/write bit.

		    ofCopyMemory = 0x00000002,     // Open scope with memory. Ask metadata to maintain its own copy of memory.

		    ofManifestMetadata = 0x00000008,     // Open scope on ngen image, return the manifest metadata instead of the IL metadata
		    ofReadOnly = 0x00000010,     // Open scope for read. Will be unable to QI for a IMetadataEmit* interface
		    ofTakeOwnership = 0x00000020,     // The memory was allocated with CoTaskMemAlloc and will be freed by the metadata

		    // These are obsolete and are ignored.
		    ofCacheImage = 0x00000004,     // EE maps but does not do relocations or verify image
		    ofNoTypeLib = 0x00000080,     // Don't OpenScope on a typelib.

		    // Internal bits
		    ofReserved1 = 0x00000100,     // Reserved for internal use.
		    ofReserved2 = 0x00000200,     // Reserved for internal use.
		    ofReserved = 0xffffff40,      // All the reserved bits.

		    Default = ofCopyMemory | ofReadOnly
	    }

	    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	    private static ISymbolLoader Create(SymBinder binder, string pathModule, string searchPath)
	    {
		    try
		    {
			    // First create the Metadata dispenser.
			    var pDispenser = new IMetaDataDispenserEx();
			    // Now open an Importer on the given filename. We'll end up passing this importer straight
			    // through to the Binder.
			    var importerIID = new Guid("7DAC8207-D3AE-4c75-9B67-92801A497D44");
			    var pImporter = IntPtr.Zero;
			    pDispenser.OpenScope(pathModule, (uint)COR_OPEN_FLAGS.Default,
			                         ref importerIID, out pImporter);
			    ISymbolReader symReader;
			    try
			    {
				    symReader = binder.GetReader(pImporter, pathModule, searchPath);
			    }
			    catch (Exception)
			    {
				    return null;
			    }
			    finally
			    {
				    if (pImporter != IntPtr.Zero)
					    Marshal.Release(pImporter);
				    Marshal.ReleaseComObject(pDispenser);
			    }

			    return new PdbReader
				    {
					    SymbolReader = symReader
				    };
		    }
		    catch (Exception)
		    {
			    return null;
		    }
	    }

	    private static ISymbolMethod GetSymbolMethod(ISymbolReader reader, IMethod method)
	    {
		    if (method.IsAbstract) return null;
		    if (method.IsInternalCall) return null;
		    if (method.CodeType != MethodCodeType.IL) return null;
		    try
		    {
			    return reader.GetMethod(new SymbolToken(method.MetadataToken));
		    }
		    catch (Exception e)
		    {
			    return null;
		    }
	    }

	    private static IEnumerable<SequencePoint> ReadSequencePoints(ISymbolMethod symMethod)
	    {
		    try
		    {
			    int count = symMethod.SequencePointCount;

			    // Get the sequence points from the symbol store. 
			    // We could cache these arrays and reuse them.
			    var offsets = new int[count];
			    var docs = new ISymbolDocument[count];
			    var startColumn = new int[count];
			    var endColumn = new int[count];
			    var startRow = new int[count];
			    var endRow = new int[count];
			    symMethod.GetSequencePoints(offsets, docs, startRow, startColumn, endRow, endColumn);

			    var list = new List<SequencePoint>(count);
			    for (int i = 0; i < count; ++i)
			    {
				    int offset = offsets[i];

				    // See http://blogs.msdn.com/jmstall/archive/2005/06/19/FeeFee_SequencePoints.aspx
				    if (startRow[i] == 0xFeeFee) //hidden block
				    {

				    }
				    else
				    {
					    list.Add(new SequencePoint
						    {
							    File = docs[i].URL,
							    Offset = offset,
							    StartRow = startRow[i],
							    EndRow = endRow[i],
							    StartColumn = startColumn[i],
							    EndColumn = endColumn[i]
						    });
				    }
			    }

			    return list;
		    }
		    catch (Exception)
		    {
			    return null;
		    }
	    }

	    private static void SetVariableNames(IVariableCollection vars, ISymbolScope scope)
	    {
		    if (vars == null) return;
		    if (vars.Count <= 0) return;

		    foreach (var l in scope.GetLocals())
		    {
			    if (l.AddressKind == SymAddressKind.ILOffset)
			    {
				    int index = l.AddressField1;
				    vars[index].Name = l.Name;
			    }
		    }

		    foreach (var child in scope.GetChildren())
		    {
			    SetVariableNames(vars, child);
		    }
	    }
    }
}