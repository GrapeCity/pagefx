using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    public sealed class TypeFactory
    {
        private readonly IDictionary<string, IType> _types = new Dictionary<string, IType>();

		private IType Make(string key, Func<IType> create)
		{
			IType result;
			if (_types.TryGetValue(key, out result))
				return result;

			result = create();

			_types.Add(key, result);

			return result;
		}

        public IType MakeArray(IType type, IArrayDimensionCollection dim)
        {
            if (dim == null)
                dim = ArrayDimensionCollection.Single;

        	var dimensionString = dim.ToString();
        	string key = BuildKey(type, dimensionString);

	        return Make(key, () => new ArrayType(type, dim));
        }

        public IType MakeArray(IType type)
        {
            return MakeArray(type, null);
        }

        public IType MakePointerType(IType type)
        {
            string key = BuildKey(type, CLRNames.Ptr);
	        return Make(key, () => new PointerType(type));
        }

        public IType MakeReferenceType(IType type)
        {
            string key = BuildKey(type, CLRNames.Ref);
	        return Make(key, () => new ReferenceType(type));
        }

        public IType MakeGenericType(IType type, IEnumerable<IType> args)
        {
	        var list = args.ToList();
            string key = BuildKey(type, list);
	        return Make(key, () => new GenericInstance(type, list) {Key = key});
        }

	    public IType MakeGenericType(IType type, IType arg)
        {
	        return MakeGenericType(type, new[] {arg});
        }

		private static string BuildKey(IType type, string suffix)
		{
			return type.Key + suffix;
		}

		private static string BuildKey(IType type, IEnumerable<IType> args)
		{
			var sb = new StringBuilder();
			sb.Append(type.FullName);
			sb.Append('<');
			foreach (var arg in args)
			{
				sb.Append(arg.Key);
				sb.Append(',');
			}
			sb.Length -= 1;
			sb.Append('>');
			return sb.ToString();
		}
    }
}