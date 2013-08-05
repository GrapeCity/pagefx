using System.Text;
using System.Xml;
using DataDynamics.PageFX.Flash.Swf;

namespace DataDynamics.PageFX.Flash.Abc
{
    /// <summary>
    /// Represents info about exception handler in <see cref="AbcMethodBody"/>
    /// </summary>
    public sealed class AbcExceptionHandler : ISupportXmlDump, ISwfIndexedAtom
    {
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
    }
}