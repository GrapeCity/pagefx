#if NUNIT
using System.Diagnostics;
using System.IO;
using System.Text;
using DataDynamics.PageFX.Core.Metadata;
using NUnit.Framework;
using Newtonsoft.Json;

namespace DataDynamics.PageFX.Core.Tests
{
	[Explicit]
	[TestFixture]
	public class MetadataDumpFixture
	{
		[TestCase(@"c:\tsv\pets\vmjs\test\mscorlib.dll")]
		public void DumpLocations(string path)
		{
			var md = new MetadataReader(path);
			var sb = new StringBuilder();
			using (var output = new StringWriter(sb))
			using (var writer = new JsonTextWriter(output)
				{
					Formatting = Formatting.Indented
				})
			{
				writer.WriteStartObject();
				for (var i = 0; i < 64; i++)
				{
					var id = (TableId) i;
					var table = md.GetTable(id);
					writer.WritePropertyName(id.ToString());
					if (table == null)
					{
						writer.WriteNull();
						continue;
					}
					writer.WriteStartObject();
					writer.WritePropertyName("offset");
					writer.WriteValue(table.Offset);
					writer.WritePropertyName("size");
					writer.WriteValue(table.Size);
					writer.WritePropertyName("rowCount");
					writer.WriteValue(table.RowCount);
					writer.WritePropertyName("rowSize");
					writer.WriteValue(table.RowSize);
					writer.WritePropertyName("isSorted");
					writer.WriteValue(table.IsSorted);
					writer.WriteEndObject();
				}
				writer.WriteEndObject();
			}
			Debug.WriteLine(sb.ToString());
		}
	}
}
#endif