namespace DataDynamics.PageFX.FlashLand.Swf.Tags
{
    public class SwfCharacterAny : SwfCharacter
    {
        #region ctors
        public SwfCharacterAny(SwfTagCode tagCode)
        {
            _tagCode = tagCode;
        }

        public SwfCharacterAny(ushort id, SwfTagCode tagCode) : base(id)
        {
            _tagCode = tagCode;
        }

        public SwfCharacterAny(SwfTagCode tagCode, byte[] body)
        {
            _tagCode = tagCode;
            _body = body;
        }

        public SwfCharacterAny(ushort id, SwfTagCode tagCode, byte[] body)
            : base(id)
        {
            _tagCode = tagCode;
            _body = body;
        }
        #endregion

        public byte[] Body
        {
            get { return _body; }
            set { _body = value; }
        }
        private byte[] _body;

        public override SwfTagCode TagCode
        {
            get { return _tagCode; }
        }
        private readonly SwfTagCode _tagCode;

        protected override void ReadBody(SwfReader reader)
        {
            _body = reader.ReadToEnd();
        }

        protected override void WriteBody(SwfWriter writer)
        {
            if (_body != null)
                writer.Write(_body);
        }

        public override SwfTag Clone()
        {
            if (_body != null)
            {
                var body = (byte[])_body.Clone();
                return new SwfCharacterAny(CharacterId, _tagCode, body);
            }
            return new SwfCharacterAny(CharacterId, _tagCode);
        }
    }
}