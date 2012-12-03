using System;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.PageFX.FLI.SWF
{
    [TODO]
    [SwfTag(SwfTagCode.DefineFontAlignZones)]
    public class SwfTagDefineFontAlignZones : SwfTag
    {
        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DefineFontAlignZones; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void WriteTagData(SwfWriter writer)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}