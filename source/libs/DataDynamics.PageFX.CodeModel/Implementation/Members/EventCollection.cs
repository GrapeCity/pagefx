namespace DataDynamics.PageFX.CodeModel
{
    [XmlElementName("Events")]
    public sealed class EventCollection : TypeMemberCollection<IEvent>, IEventCollection
    {
        public EventCollection(IType owner) : base(owner)
        {
        }

        #region IEventCollection Members
        public IEvent this[string name]
        {
            get { return _list.Find(e => e.Name == name); }
        }
        #endregion
    }
}