namespace DataDynamics.PageFX.CodeModel.TypeSystem
{
    public sealed class Event : TypeMember, IEvent
    {
        /// <summary>
        /// Gets the kind of this member.
        /// </summary>
        public override MemberType MemberType
        {
            get { return MemberType.Event; }
        }

        public bool IsFlash { get; set; }

	    public IMethod Adder { get; set; }

        public IMethod Remover { get; set; }

        public IMethod Raiser { get; set; }

		protected override IType ResolveDeclaringType()
		{
			return Adder != null
				       ? Adder.DeclaringType
				       : (Remover != null
					          ? Remover.DeclaringType
					          : (Raiser != null
						             ? Raiser.DeclaringType
						             : null));
		}
    }
}