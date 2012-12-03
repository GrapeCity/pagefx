using System.Xml;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.PageFX.FLI.SWF
{
    [TODO]
    [SwfTag(SwfTagCode.PlaceObject3)]
    public class SwfTagPlaceObject3 : SwfTagPlaceObject2
    {
        public SwfFilterList Filters
        {
            get { return _filters; }
        }
        private readonly SwfFilterList _filters = new SwfFilterList();

        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.PlaceObject3; }
        }

        protected override void ReadBeforeClipActions(SwfReader reader)
        {
            if ((Flags & SwfPlaceFlags.HasFilterList) != 0)
                _filters.Read(reader);
        }

        protected override void WriteBeforeClipActions(SwfWriter writer)
        {
            if ((Flags & SwfPlaceFlags.HasFilterList) != 0)
                _filters.Write(writer);
        }

        public override void DumpBody(XmlWriter writer)
        {
            if (SwfDumpService.DumpDisplayListTags)
            {
                base.DumpBody(writer);

                if (_filters.Count > 0)
                    _filters.Dump(writer);
            }
        }
    }
}