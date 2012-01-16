using System.Collections.Generic;
using System.IO;

namespace DataDynamics.PageFX
{
    public sealed class SourceFile
    {
        public SourceFile()
        {
        }

        public SourceFile(string name, string text)
        {
            Name = name;
            Text = text;
        }

    	/// <summary>
    	/// Gets or sets name of file
    	/// </summary>
    	public string Name { get; set; }

    	/// <summary>
    	/// Gets or sets the source text
    	/// </summary>
    	public string Text { get; set; }

    	public override string ToString()
        {
            return Name;
        }
    }

    public sealed class SourceFileList : List<SourceFile>
    {
        public SourceFile this[string name]
        {
            get { return Find(f => f.Name == name); }
        }

        public void Add(string name, string text)
        {
            Add(new SourceFile(name, text));
        }

        public void Add(string name, Stream stream)
        {
            Add(name, QA.ReadAllText(stream));
        }

        public string[] Names
        {
            get
            {
                int n = Count;
                var names = new string[n];
                for (int i = 0; i < n; ++i)
                    names[i] = this[i].Name;
                return names;
            }
        }
    }
}