using System;
using System.Xml;
using DataDynamics.PageFX.FlashLand.IL;
using DataDynamics.PageFX.FlashLand.Swf;

namespace DataDynamics.PageFX.FlashLand.Abc
{
    #region Short Description of Format
    // A MethodBody describes the method implementation.  
    // not required for native methods or interface methods.
    //MethodBody
    //{
    //    U30 method_info
    //    U30 max_stack
    //          The max_stack field is maximum number of evaluation stack slots used at any point during 
    //          the execution of this body.
    //    U30 max_regs
    //    U30 init_scope_depth
    //          The init_scope_depth field defines the minimum scope depth, relative to max_scope_depth, 
    //          that may be accessed within the method.
    //    U30 max_scope
    //          The max_scope_depth field defines the maximum scope depth that may be accessed within the method.
    //          The difference between max_scope_depth and init_scope_depth determines the size of the local scope stack.
    //    U30 code_length
    //    U8 code[code_length]
    //    U30 ex_count
    //    Exception[ex_count]
    //    Traits traits	// activation traits
    //}
    #endregion

    /// <summary>
    /// Represents body of ABC method.
    /// </summary>
    public sealed class AbcMethodBody : ISwfAtom, ISupportXmlDump, IAbcTraitProvider
    {
	    public AbcMethodBody()
        {
            _traits = new AbcTraitCollection(this);
        }

        public AbcMethodBody(AbcMethod method) : this()
        {
            Method = method;
        }

	    /// <summary>
        /// Gets or sets associated method.
        /// </summary>
        public AbcMethod Method
        {
            get { return _method; }
            set
            {
                if (value != _method)
                {
                    _method = value;
                    _method.Body = this;
                }
            }
        }
        private AbcMethod _method;

	    /// <summary>
	    /// Max capacity of evaluation stack.
	    /// </summary>
	    public int MaxStackDepth { get; set; }

	    /// <summary>
	    /// Number of local registers (this + arguments + local vars)
	    /// </summary>
	    public int LocalCount { get; set; }

	    public int MinScopeDepth { get; set; }

	    public int MaxScopeDepth { get; set; }

	    /// <summary>
        /// IL code
        /// </summary>
        public ILStream IL
        {
            get { return _il ?? (_il = new ILStream()); }
        }
        private ILStream _il;

        public AbcExceptionHandlerCollection Exceptions
        {
            get { return _exceptions; }
        }
        private readonly AbcExceptionHandlerCollection _exceptions = new AbcExceptionHandlerCollection();

        /// <summary>
        /// Activation traits.
        /// </summary>
        public AbcTraitCollection Traits
        {
            get { return _traits; }
        }
        private readonly AbcTraitCollection _traits;

        internal AbcMethodBody ImportedBody { get; set; }

	    public void Read(SwfReader reader)
        {
            _method = reader.ReadAbcMethod();
            _method.Body = this;

            MaxStackDepth = (int)reader.ReadUIntEncoded();
            LocalCount = (int)reader.ReadUIntEncoded();
            MinScopeDepth = (int)reader.ReadUIntEncoded();
            MaxScopeDepth = (int)reader.ReadUIntEncoded();

            int len = (int)reader.ReadUIntEncoded();
            var code = reader.ReadUInt8(len);

            _exceptions.Read(reader);
            _traits.Read(reader);

            _il = new ILStream();

            if (len > 0)
            {
                var codeReader = new SwfReader(code) {ABC = reader.ABC};
                _il.Read(this, codeReader);
            }
        }

        public void Write(SwfWriter writer)
        {
            writer.WriteUIntEncoded((uint)_method.Index);
            writer.WriteUIntEncoded((uint)MaxStackDepth);
            writer.WriteUIntEncoded((uint)LocalCount);
            writer.WriteUIntEncoded((uint)MinScopeDepth);
            writer.WriteUIntEncoded((uint)MaxScopeDepth);

            if (_il != null)
            {
                using (var codeWriter = new SwfWriter())
                {
                    codeWriter.ABC = writer.ABC;
                    _il.Write(codeWriter);
                    var code = codeWriter.ToByteArray();
                    writer.WriteUIntEncoded((uint)code.Length);
                    writer.Write(code);
                }
            }
            else
            {
                writer.WriteUInt8(0);
            }

            _exceptions.Write(writer);
            _traits.Write(writer);
        }

	    public void DumpXml(XmlWriter writer)
        {
            writer.WriteStartElement("method-body");
            writer.WriteElementString("max-stack", MaxStackDepth.ToString());
            writer.WriteElementString("local-count", LocalCount.ToString());
            writer.WriteElementString("min-scope-depth", MinScopeDepth.ToString());
            writer.WriteElementString("max-scope-depth", MaxScopeDepth.ToString());

            _exceptions.DumpXml(writer);
            _traits.DumpXml(writer);

            if (_il != null)
                _il.DumpXml(writer);

            writer.WriteEndElement();
        }

	    #region Finish
        private class Block
        {
            public int from; //index of entry instruction
            public int stackDepth;
            public int maxStack;
            public int scopeDepth;
            public int maxScope;
        }

        internal void Finish(AbcFile abc)
        {
	        //this + args
	        int paramCount = Method.Parameters.Count;
	        int lcount = paramCount + 1;

	        int n = IL.Count;

	        Block block = null;

	        MaxStackDepth = 0;
	        MaxScopeDepth = 0;
	        MinScopeDepth = 0;

	        IL.MarkTargets();
	        MarkSEHTargets();

	        int maxPop = 0;
	        int maxPush = 0;

	        for (int i = 0; i < n; ++i)
	        {
		        var instr = IL[i];

		        if (block == null || instr.IsSEHTarget)
		        {
			        UpdateSizeOfStacks(block);

			        block = new Block {@from = i};
			        if (instr.IsSEHTarget)
				        block.stackDepth = 1;
		        }

		        int pop = instr.StackPop;
		        int push = instr.StackPush;

		        if (pop > maxPop) maxPop = pop;
		        if (push > maxPush) maxPush = push;

		        block.stackDepth += push - pop;
		        if (block.stackDepth > block.maxStack)
			        block.maxStack = block.stackDepth;

		        block.scopeDepth += instr.ScopePush - instr.ScopePop;
		        if (block.scopeDepth > block.maxScope)
			        block.maxScope = block.scopeDepth;

		        if (instr.IsEndOfBlock && block.from != i)
		        {
			        UpdateSizeOfStacks(block);
			        block = null;
		        }

		        var c = instr.Code;

		        if (c == InstructionCode.Newactivation)
			        Method.Flags |= AbcMethodFlags.NeedActivation;

		        if (c == InstructionCode.Dxns || c == InstructionCode.Dxnslate)
			        Method.Flags |= AbcMethodFlags.SetDxns;

		        int lreg = GetLocalReg(instr);
		        if (lreg >= 0)
		        {
			        if (lreg + 1 > lcount)
				        lcount = lreg + 1;
		        }

		        instr.ImportOperands(abc);
	        }

	        UpdateSizeOfStacks(block);

	        MaxScopeDepth += MinScopeDepth;

	        //if (GlobalSettings.EmitDebugInfo)
	        //    ++lcount;

	        //NOTE: AVM constraint
	        if (lcount < paramCount + 1)
		        lcount = paramCount + 1;

	        LocalCount = lcount;

	        TranslateIndices();
	        ResolveExceptionOffsets(abc);

#if DEBUG
	        //IL.Verify();
#endif
        }

	    private void UpdateSizeOfStacks(Block block)
        {
            if (block == null) return;
            MaxStackDepth += block.maxStack;
            MaxScopeDepth += block.maxScope;
        }

        internal void Finish(AbcCode code)
        {
            if (code == null)
                throw new ArgumentNullException("code");
            CreateSEHs(code);
            IL.Add(code);
            Finish(code.Abc);
        }

        private void CreateSEHs(AbcCode code)
        {
            foreach (var tb in code.TryBlocks)
            {
                foreach (var h in tb.Handlers)
                {
                    Exceptions.Add(h);
                }
            }
        }

        internal void TranslateIndices()
        {
            IL.TranslateIndicesEnabled = true;
            IL.TranslateIndices();
        }

        internal void SetupOffsets()
        {
            IL.SetupOffsets();
        }

        /// <summary>
        /// Translates exception indecies to instruction offsets.
        /// </summary>
        internal void ResolveExceptionOffsets(AbcFile abc)
        {
            if (abc == null)
                throw new ArgumentNullException("abc");
            var code = IL;
            foreach (var e in Exceptions)
            {
                e.From = code[e.From].Offset;
                e.To = code[e.To].Offset;
                e.Target = code[e.Target].Offset;
                e.Type = abc.ImportConst(e.Type);
                e.VariableName = abc.ImportConst(e.VariableName);
            }
        }

        private void MarkSEHTargets()
        {
            foreach (var e in Exceptions)
            {
                IL[e.Target].IsSEHTarget = true;
            }
        }
        #endregion

	    private static int GetLocalReg(Instruction i)
        {
            switch (i.Code)
            {
                case InstructionCode.Getlocal:
                case InstructionCode.Setlocal:
                    return (int)i.Operands[0].Value;

                case InstructionCode.Getlocal0:
                case InstructionCode.Setlocal0:
                    return 0;

                case InstructionCode.Getlocal1:
                case InstructionCode.Setlocal1:
                    return 1;

                case InstructionCode.Getlocal2:
                case InstructionCode.Setlocal2:
                    return 2;

                case InstructionCode.Getlocal3:
                case InstructionCode.Setlocal3:
                    return 3;
            }
            return -1;
        }

	    public override string ToString()
        {
            if (_method != null)
                return _method.ToString();
            return base.ToString();
        }

        internal AbcBodyFlags Flags;
    }

	[Flags]
    internal enum AbcBodyFlags
    {
        None = 0x00,
        HasExceptions = 0x01,
        HasSlots = 0x02,
        HasSlotPointers = 0x04,
        HasFieldPointers = 0x08,
        HasElemPointers = 0x10,
        HasMetadataTokens = 0x20,
        HasNewArrayInstructions = 0x40,
    }
}