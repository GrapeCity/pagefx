using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Xml;
using DataDynamics.PageFX.FlashLand.Swf.Filters;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags.Buttons
{
    public class SwfButton
    {
        #region ctors
        public SwfButton()
        {
        }

        public SwfButton(SwfButtonState state)
        {
            _state = state;
        }
        #endregion

        #region Properties
        public SwfButtonState State
        {
            get { return _state; }
            set { _state = value; }
        }
        private SwfButtonState _state;

        /// <summary>
        /// Gets or sets id of character to place
        /// </summary>
        public ushort CharID
        {
            get { return _charID; }
            set { _charID = value; }
        }
        private ushort _charID;

        public ushort PlaceDepth
        {
            get { return _placeDepth; }
            set { _placeDepth = value; }
        }
        private ushort _placeDepth;

        public Matrix PlaceMatrix
        {
            get { return _placeMatrix; }
            set { _placeMatrix = value; }
        }
        private Matrix _placeMatrix;

        public SwfColorTransform ColorTransform
        {
            get { return _colorTransform; }
            set { _colorTransform = value; }
        }
        private SwfColorTransform _colorTransform;

        public SwfFilterList Filters
        {
            get { return _filters; }
        }
        private readonly SwfFilterList _filters = new SwfFilterList();

        public SwfBlendMode BlendMode
        {
            get { return _blendMode;  }
            set { _blendMode = value; }
        }
        private SwfBlendMode _blendMode;
        #endregion

        #region IO
        private static bool HasAlpha(SwfTagCode tagCode)
        {
            return tagCode == SwfTagCode.DefineButton2;
        }

        public void Read(SwfReader reader, SwfTagCode tagCode)
        {
            //state is already read
            _charID = reader.ReadUInt16();
            _placeDepth = reader.ReadUInt16();
            _placeMatrix = reader.ReadMatrix();
            _colorTransform = reader.ReadColorTransform(HasAlpha(tagCode));
            if ((_state & SwfButtonState.HasFilterList) != 0)
                _filters.Read(reader);
            if ((_state & SwfButtonState.HasBlendMode) != 0)
                _blendMode = (SwfBlendMode)reader.ReadUInt8();
        }

        public void Write(SwfWriter writer, SwfTagCode tagCode)
        {
            writer.WriteUInt8((byte)_state);
            writer.WriteUInt16(_charID);
            writer.WriteUInt16(_placeDepth);
            writer.WriteMatrix(_placeMatrix);
            _colorTransform.Write(writer, HasAlpha(tagCode));
            if ((_state & SwfButtonState.HasFilterList) != 0)
                _filters.Write(writer);
            if ((_state & SwfButtonState.HasBlendMode) != 0)
                writer.WriteUInt8((byte)_blendMode);
        }

        public void Dump(XmlWriter writer, SwfTagCode tagCode)
        {
            writer.WriteStartElement("button");
            writer.WriteAttributeString("id", _charID.ToString());
            writer.WriteAttributeString("depth", _placeDepth.ToString());
            writer.WriteElementString("matrix", _placeMatrix.GetMatrixString());
            _colorTransform.Dump(writer, HasAlpha(tagCode));
            if (_filters.Count > 0)
                _filters.Dump(writer);
            if ((_state & SwfButtonState.HasBlendMode) != 0)
                writer.WriteElementString("blend-mode", _blendMode.ToString());
            writer.WriteEndElement();
        }
        #endregion
    }

    public class SwfButtonList : List<SwfButton>
    {
        #region IO
        public void Read(SwfReader reader, SwfTagCode tagCode)
        {
            while (true)
            {
                byte state = reader.ReadUInt8();
                if (state == 0) break;
                var btn = new SwfButton((SwfButtonState)state);
                btn.Read(reader, tagCode);
                Add(btn);
            }
        }

        public void Write(SwfWriter writer, SwfTagCode tagCode)
        {
            int n = Count;
            for (int i = 0; i < n; ++i)
                this[i].Write(writer, tagCode);
            writer.WriteUInt8(0);
        }

        public void Dump(XmlWriter writer, SwfTagCode tagCode)
        {
            writer.WriteStartElement("buttons");
            int n = Count;
            writer.WriteAttributeString("count", n.ToString());
            for (int i = 0; i < n; ++i)
                this[i].Dump(writer, tagCode);
            writer.WriteEndElement();
        }
        #endregion
    }
}