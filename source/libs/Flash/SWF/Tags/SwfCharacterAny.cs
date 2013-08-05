namespace DataDynamics.PageFX.FlashLand.Swf.Tags
{
    public class SwfCharacterAny : SwfCharacter
    {
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
            Body = body;
        }

        public SwfCharacterAny(ushort id, SwfTagCode tagCode, byte[] body)
            : base(id)
        {
            _tagCode = tagCode;
            Body = body;
        }

	    public byte[] Body { get; set; }

	    public override SwfTagCode TagCode
        {
            get { return _tagCode; }
        }
        private readonly SwfTagCode _tagCode;

        protected override void ReadBody(SwfReader reader)
        {
            Body = reader.ReadToEnd();
        }

        protected override void WriteBody(SwfWriter writer)
        {
            if (Body != null)
                writer.Write(Body);
        }

        public override SwfTag Clone()
        {
            if (Body != null)
            {
                var body = (byte[])Body.Clone();
                return new SwfCharacterAny(CharacterId, _tagCode, body);
            }
            return new SwfCharacterAny(CharacterId, _tagCode);
        }
    }
}