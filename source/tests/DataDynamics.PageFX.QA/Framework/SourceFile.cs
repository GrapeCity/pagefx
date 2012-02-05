using System.Collections.Generic;
using System.IO;

namespace DataDynamics.PageFX
{
    public class SourceFile
    {
        public SourceFile()
        {
        }

        public SourceFile(string name, string text)
        {
            _name = name;
            _text = text;
        }

        /// <summary>
        /// Gets or sets name of file
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _name;

        /// <summary>
        /// Gets or sets the source text
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
        private string _text;

        public override string ToString()
        {
            return _name;
        }
    }

    public class SourceFileList : List<SourceFile>
    {
        public SourceFile this[string name]
        {
            get
            {
                return Find(delegate(SourceFile f)
                                {
                                    return f.Name == name;
                                });
            }
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