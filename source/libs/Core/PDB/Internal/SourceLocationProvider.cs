//-----------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All Rights Reserved.
//
//-----------------------------------------------------------------------------

namespace DataDynamics.PageFX.Core.Pdb.Internal {

  internal sealed class PdbIteratorScope : ILocalScope {

    internal PdbIteratorScope(uint offset, uint length) {
      this.offset = offset;
      this.length = length;
    }

    public uint Offset {
      get { return this.offset; }
    }
    uint offset;

    public uint Length {
      get { return this.length; }
    }
    uint length;
  }
}