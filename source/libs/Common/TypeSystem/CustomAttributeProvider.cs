using System.Linq;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	public class CustomAttributeProvider : ICustomAttributeProvider
	{
		public ICustomAttributeCollection CustomAttributes
		{
			get { return _attributes ?? (_attributes = new CustomAttributeCollection()); }
			set { _attributes = value; }
		}
		private ICustomAttributeCollection _attributes;

		/// <summary>
		/// Gets or sets value that identifies a metadata element. 
		/// </summary>
		public int MetadataToken { get; set; }
	}

	public static class CustomAttributeProviderExtensions
	{
		//TODO: make this is a part of ICustomAttributeCollection to make this search faster
		public static ICustomAttribute FindAttribute(this ICustomAttributeProvider p, string fullname)
		{
			return p.CustomAttributes.FirstOrDefault(attr => attr.TypeName == fullname);
		}

		public static bool HasAttribute(this ICustomAttributeProvider p, string fullname)
		{
			return p.FindAttribute(fullname) != null;
		}

		public static bool HasAttribute(this ICustomAttributeProvider p, params string[] attrs)
		{
			return p.CustomAttributes.Any(attr => attrs.Contains(attr.TypeName));
		}
	}
}