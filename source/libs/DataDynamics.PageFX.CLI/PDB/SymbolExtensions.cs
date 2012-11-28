using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using DataDynamics.PageFX.CLI.PDB.Interop;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.PDB
{
    internal static class SymbolExtensions
    {
        #region GetSymbolReader
        public static PdbReader GetPdbReader(this Assembly assembly)
        {
            return assembly.Location.GetPdbReader();
        }

        public static PdbReader GetPdbReader(this string pathModule)
        {
            if (!Path.IsPathRooted(pathModule))
                pathModule = Path.Combine(Environment.CurrentDirectory, pathModule);
            return pathModule.GetPdbReader(Path.GetDirectoryName(pathModule));
        }

        public static PdbReader GetPdbReader(this string pathModule, string searchPath)
        {
            if (!Path.IsPathRooted(pathModule))
                pathModule = Path.Combine(Environment.CurrentDirectory, pathModule);
            return GetPdbReader(new SymBinder(), pathModule, searchPath);
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

        // We demand Unmanaged code permissions because we're reading from the file system and calling out to the Symbol Reader
        // @TODO - make this more specific.
        [SecurityPermission(
            SecurityAction.Demand,
            Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static PdbReader GetPdbReader(SymBinder binder, string pathModule, string searchPath)
        {
            try
            {
                // First create the Metadata dispenser.
                var pDispenser = new IMetaDataDispenserEx();
                // Now open an Importer on the given filename. We'll end up passing this importer straight
                // through to the Binder.
                var importerIID = new Guid(Guids.IID_IMetaDataImport);
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
            	       		Binder = binder,
            	       		SymReader = symReader
            	       	};
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region GetSymbolMethod
        public static ISymbolMethod GetSymbolMethod(this ISymbolReader reader, IMethod method)
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
        #endregion

        #region ReadSequencePoints
        public static List<SequencePoint> ReadSequencePoints(this ISymbolMethod symMethod)
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
        #endregion

        #region Naming Local Variables
        public static void SetNames(this IVariableCollection vars, ISymbolScope scope)
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
                vars.SetNames(child);
            }
        }

        public static void SetGoodNames(this IVariableCollection vars)
        {
            foreach (var v in vars)
            {
                string name = ReplaceBadChars(v.Name);
                if (name != v.Name)
                {
                    v.Name = UnifyName(vars, name);
                }
            }
        }

        private static IVariable FindVar(IEnumerable<IVariable> vars, string name)
        {
            return vars.FirstOrDefault(v => v.Name == name);
        }

		private static string UnifyName(IEnumerable<IVariable> vars, string name)
        {
            int n = 1;
            string original = name;
            while (FindVar(vars, name) != null)
            {
                name = original + n;
                ++n;
            }
            return name;
        }

        static string ReplaceBadChars(string name)
        {
            var sb = new StringBuilder();
            int n = name.Length;
            for (int i = 0; i < n; ++i)
            {
                char c = name[i];
                if (i == 0)
                {
                    if (c.IsSimpleIdStartChar())
                    {
                        sb.Append(c);
                        continue;
                    }
                }
                else
                {
                    if (c.IsSimpleIdChar())
                    {
                        sb.Append(c);
                        continue;
                    }
                }

                sb.Append("x" + ((int)c).ToString("X"));
            }
            return sb.ToString();
        }
        #endregion
    }
}