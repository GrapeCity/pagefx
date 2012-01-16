using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DataDynamics.PageFX.FLI.ABC
{
    /// <summary>
    /// Represents method paramater.
    /// </summary>
    public class AbcParameter : ISupportXmlDump
    {
        #region ctors
        public AbcParameter()
        {
        }

        public AbcParameter(AbcMultiname type)
        {
            _type = type;
        }

        public AbcParameter(AbcMultiname type, AbcConst<string> name)
        {
            _type = type;
            _name = name;
        }
        #endregion

        #region Properties
        public AbcConst<string> Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private AbcConst<string> _name;

        public bool HasName
        {
            get
            {
                //if (_name == null) return false;
                //return !string.IsNullOrEmpty(_name.Value);
                return _name != null;
            }
        }

        public AbcMultiname Type
        {
            get { return _type; }
            set { _type = value; }
        }
        private AbcMultiname _type;

        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }
        private object _value;

        public bool IsOptional
        {
            get { return _isOptional; }
            set { _isOptional = value; }
        }
        private bool _isOptional;
        #endregion

        #region Dump
        public void DumpXml(XmlWriter writer)
        {
            writer.WriteStartElement("param");
            if (_name != null)
                writer.WriteAttributeString("name", _name.Value);

            if (_type != null)
            {
                writer.WriteAttributeString("type", _type.ToString());
            }

            if (_isOptional)
            {
                writer.WriteAttributeString("value", _value != null ? _value.ToString() : "null");
            }

            if (_type != null)
            {
                _type.DumpXml(writer, "type");
            }

            writer.WriteEndElement();
        }
        #endregion

        #region Object Override Members
        public override string ToString()
        {
            return ToString("s");
        }

        public string ToString(string typeFormat)
        {
            var s = new StringBuilder();
            if (_name != null && !string.IsNullOrEmpty(_name.Value))
            {
                s.Append(_name.Value);
                s.Append(":");
            }
            s.Append(_type != null ? _type.ToString(typeFormat) : "*");
            if (_isOptional)
            {
                s.Append(" = ");
                s.Append(_value != null ? _value.ToString() : "null");
            }
            return s.ToString();
        }
        #endregion
    }

    /// <summary>
    /// List of <see cref="AbcParameter"/> objects.
    /// </summary>
    public class AbcParameterList : List<AbcParameter>, ISupportXmlDump
    {
        #region Dump
        public void DumpXml(XmlWriter writer)
        {
            if (Count > 0)
            {
                writer.WriteStartElement("params");
                foreach (var p in this)
                    p.DumpXml(writer);
                writer.WriteEndElement();
            }
        }
        #endregion

        public void CopyFrom(AbcMethod method)
        {
            if (method == null)
                throw new ArgumentNullException("method");
            CopyFrom(method.Parameters);
        }

        public void CopyFrom(IEnumerable<AbcParameter> list)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            foreach (var p in list)
            {
                Add(new AbcParameter(p.Type, p.Name));
            }
        }
    }

    public delegate void AbcParametersHandler(AbcParameterList list);
}