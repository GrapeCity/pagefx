using System.Collections.Generic;
using System.Text;
using System.Xml;
using DataDynamics.PageFX.FlashLand.IL;
using DataDynamics.PageFX.FlashLand.Swf;

namespace DataDynamics.PageFX.FlashLand.Abc
{
    /// <summary>
    /// Represents info about exception handler in <see cref="AbcMethodBody"/>
    /// </summary>
    public class AbcExceptionHandler : ISupportXmlDump, ISwfIndexedAtom
    {
        #region Constructors
        public AbcExceptionHandler()
        {
        }

        public AbcExceptionHandler(SwfReader reader)
        {
            Read(reader);
        }
        #endregion

        #region Properties
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets begin offset of protected block.
        /// </summary>
        public int From { get; set; }

        /// <summary>
        /// Gets or sets end offset of protected block.
        /// </summary>
        public int To { get; set; }

        /// <summary>
        /// Gets or sets offset to entry point of SEH block.
        /// </summary>
        public int Target { get; set; }

        /// <summary>
        /// Gets or sets name of exception type.
        /// </summary>
        public AbcMultiname Type { get; set; }

        //NOTE: It seems that variable name is multiname, but not string

        /// <summary>
        /// Gets or sets name of exception variable. Used for debugging purposes.
        /// </summary>
        public AbcMultiname VariableName { get; set; }

		//TODO: use it to specify variable type at the begin of method body.
		/// <summary>
		/// Gets or sets index of temporary variable assigned during code generation.
		/// </summary>
		public int LocalVariable { get; set; }

        #endregion

        #region IAbcAtom Members
        public void Read(SwfReader reader)
        {
            From = (int)reader.ReadUIntEncoded();
            To = (int)reader.ReadUIntEncoded();
            Target = (int)reader.ReadUIntEncoded();
            Type = reader.ReadMultiname();
            VariableName = reader.ReadMultiname();
        }

        public void Write(SwfWriter writer)
        {
            writer.WriteUIntEncoded((uint)From);
            writer.WriteUIntEncoded((uint)To);
            writer.WriteUIntEncoded((uint)Target);

            if (Type == null) writer.WriteUInt8(0);
            else writer.WriteUIntEncoded((uint)Type.Index);

            if (VariableName == null) writer.WriteUInt8(0);
            else writer.WriteUIntEncoded((uint)VariableName.Index);
        }
        #endregion

        #region ISupportXmlDump Members
        public void DumpXml(XmlWriter writer)
        {
            writer.WriteStartElement("exception");
            writer.WriteAttributeString("from", From.ToString());
            writer.WriteAttributeString("to", To.ToString());
            writer.WriteAttributeString("target", Target.ToString());
            writer.WriteAttributeString("type", Type != null ? Type.ToString() : "*");
            writer.WriteAttributeString("var", VariableName != null ? VariableName.ToString("s") : "*");
            writer.WriteEndElement();
        }
        #endregion

        #region Object Override Members
        public override string ToString()
        {
            var s = new StringBuilder();

        	s.Append(Type != null ? Type.ToString() : "*");

        	if (VariableName != null)
            {
                s.Append(" ");
                s.Append(VariableName);
            }

            return s.ToString();
        }
        #endregion
    }

    public class AbcExceptionHandlerCollection : List<AbcExceptionHandler>, ISwfAtom, ISupportXmlDump
    {
		public new void Sort()
		{
			base.Sort((x, y)=>
			          	{
			          		int c = x.Target - y.Target;
							if (c != 0) return c;

			          		c = x.From - y.From;
							if (c != 0) return c;

			          		return 0;
			          	});

			for (int i = 0; i < Count; i++)
			{
				this[i].Index = i;
			}
		}

    	public new void Add(AbcExceptionHandler e)
        {
            e.Index = Count;
            base.Add(e);
        }

    	#region IAbcAtom Members
        public void Read(SwfReader reader)
        {
            int n = (int)reader.ReadUIntEncoded();
            for (int i = 0; i < n; ++i)
                Add(new AbcExceptionHandler(reader));
        }

        public void Write(SwfWriter writer)
        {
            int n = Count;
            writer.WriteUIntEncoded((uint)n);
            for (int i = 0; i < n; ++i)
                this[i].Write(writer);
        }
        #endregion

        #region ISupportXmlDump Members
        public void DumpXml(XmlWriter writer)
        {
            if (Count > 0)
            {
                writer.WriteStartElement("exceptions");
                foreach (var h in this)
                    h.DumpXml(writer);
                writer.WriteEndElement();
            }
        }
        #endregion
    }

    public class AbcTryBlock
    {
        /// <summary>
        /// Gets or sets begin offset of protected block.
        /// </summary>
        public int From
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets or sets end offset of protected block.
        /// </summary>
        public int To
        {
            get;
            set;
        }

        /// <summary>
        /// Used to check BeginCatch/EndCatch balance
        /// </summary>
        internal AbcExceptionHandler CurrentHandler;

        internal Instruction SkipHandlers;

        public AbcExceptionHandlerCollection Handlers
        {
            get { return _handlers; }
        }
        private readonly AbcExceptionHandlerCollection _handlers = new AbcExceptionHandlerCollection();
    }

    public class AbcTryBlockList : List<AbcTryBlock>
    {
    }
}