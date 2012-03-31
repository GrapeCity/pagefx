using System;

namespace DataDynamics.PageFX.FLI.ABC
{
    #region enum AbcMethodFlags
    [Flags]
    public enum AbcMethodFlags
    {
        None = 0,

        /// <summary>
        /// Suggests to the run-time that an “arguments” object (as specified by the
        /// ActionScript 3.0 Language Reference) be created. Must not be used
        /// together with NeedRest. See Chapter 3.
        /// </summary>
        NeedArguments = 0x01,

        /// <summary>
        /// Must be set if this method uses the newactivation opcode.
        /// </summary>
        NeedActivation = 0x02,

        /// <summary>
        /// This flag creates an ActionScript 3.0 rest arguments array. Must not be
        /// used with NeedArguments. See Chapter 3.
        /// </summary>
        NeedRest = 0x04,

        /// <summary>
        /// Must be set if this method has optional parameters and the options
        /// field is present in this method_info structure.
        /// </summary>
        HasOptional = 0x08,

        /// <summary>
        /// 
        /// </summary>
        IgnoreRest = 0x10,

        /// <summary>
        /// Specifies whether method is native (implementation provided by AVM+)
        /// </summary>
        Native = 0x20,

        /// <summary>
        /// Must be set if this method uses the dxns or dxnslate opcodes.
        /// </summary>
        SetDxns = 0x40,

        /// <summary>
        /// Must be set when the param_names field is present in this method_info structure.
        /// </summary>
        HasParamNames = 0x80,
    }
    #endregion

    #region enum AbcMethodSemantics
    [Flags]
    public enum AbcMethodSemantics
    {
        Default = 0,
        Static = 1,
        Virtual = 2,
        Override = 4,
        VirtualOverride = Virtual | Override,
    }
    #endregion

    [Flags]
    public enum AbcTraitOwner
    {
        None,
        Instance = 1,
        Script = 2,
        MethodBody = 4,
        All = Instance | Script | MethodBody,
    }
}