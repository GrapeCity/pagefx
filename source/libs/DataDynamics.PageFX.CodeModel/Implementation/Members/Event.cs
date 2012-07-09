namespace DataDynamics.PageFX.CodeModel
{
    public class Event : TypeMember, IEvent
    {
        /// <summary>
        /// Gets the kind of this member.
        /// </summary>
        public override MemberType MemberType
        {
            get { return MemberType.Event; }
        }

        public bool IsFlash { get; set; }

        #region IEvent Members
        public IMethod Adder { get; set; }

        public IMethod Remover { get; set; }

        public IMethod Raiser { get; set; }
        #endregion
    }
}