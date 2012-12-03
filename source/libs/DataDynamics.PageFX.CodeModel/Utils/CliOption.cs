using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Linq;

namespace DataDynamics
{
	/// <summary>
	/// Represents command line option.
	/// </summary>
    public class CliOption
    {
        #region ctors
        public CliOption(string name)
        {
            Name = name;
        }

        public CliOption(string name, string aliases)
        {
            Name = name;
            Aliases = aliases;
        }
        #endregion

        #region Fields
        /// <summary>
        /// Name of this option
        /// </summary>
        public string Name;

        /// <summary>
        /// Format of value
        /// </summary>
        public string Format;

        /// <summary>
        /// Description for this option
        /// </summary>
        public string Description;

        /// <summary>
        /// Default value for this option
        /// </summary>
        public string DefaultValue;

        public string Aliases;

        /// <summary>
        /// Cetegory of this option
        /// </summary>
        public string Category;

        public bool DotNetCompiler;

        public bool PlusMinus;
        #endregion

        #region Properties
        static readonly char[] sep = { ',', ';' };

        public string[] Names
        {
            get
            {
                if (_names == null)
                {
                    var list = new List<string>();

                    AddName(list, Name);
                    
                    if (!string.IsNullOrEmpty(Aliases))
                    {
                    	var names = Aliases.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                    	foreach (string name in names)
                    		AddName(list, name);
                    }

                	_names = list.ToArray();
                }
                return _names;
            }
        }

        void AddName(ICollection<string> list, string name)
        {
            list.Add(name);
            if (PlusMinus)
            {
                list.Add(name + '+');
                list.Add(name + '-');
            }
        }

        string[] _names;
        #endregion

        public override string ToString()
        {
            return ToString(DefaultValue);
        }

        public string ToString(string value)
        {
            if (value == null)
                return string.Format("/{0}", Name);
            return string.Format("/{0}:{1}", Name, value);
        }

        public bool CheckName(string name, bool ignoreCase)
        {
            if (string.IsNullOrEmpty(name))
                return false;

        	return Names.Contains(name, StringComparer.CurrentCultureIgnoreCase);
        }

        public static void Init(Type options)
        {
            var fields = options.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var fi in fields)
            {
                if (fi.FieldType == typeof(CliOption))
                {
                    var opt = fi.GetValue(null) as CliOption;
                    if (opt != null)
                    {
                        var desc = fi.GetAttribute<DescriptionAttribute>(true);
                        if (desc != null)
                            opt.Description = desc.Description;

                        var cat = fi.GetAttribute<CategoryAttribute>(true);
                        if (cat != null)
                            opt.Category = cat.Category;

                        var format = fi.GetAttribute<FormatAttribute>(true);
                        if (format != null)
                            opt.Format = format.Value;

                        var defVal = fi.GetAttribute<DefaultValueAttribute>(true);
                        if (defVal != null)
                            opt.DefaultValue = defVal.Value.ToString();
                    }
                }
            }
        }
    }
}