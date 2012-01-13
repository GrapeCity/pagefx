namespace DataDynamics.PageFX.CodeModel
{
    public class Event : TypeMember, IEvent
    {
        /// <summary>
        /// Gets the kind of this member.
        /// </summary>
        public override TypeMemberType MemberType
        {
            get { return TypeMemberType.Event; }
        }

        public bool IsFlash { get; set; }

        #region IEvent Members
        public IMethod Adder { get; set; }

        public IMethod Remover { get; set; }

        public IMethod Raiser { get; set; }
        #endregion
    }
}