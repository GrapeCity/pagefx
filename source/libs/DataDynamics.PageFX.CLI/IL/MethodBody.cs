//BODY FORMAT:
/// Header Byte (Specifies Format)
/// Code Section
/// SEH Sections

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CLI.PDB;
using DataDynamics.PageFX.CLI.Translation;
using DataDynamics.PageFX.Common;
using DataDynamics.PageFX.Common.Statements;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.CLI.IL
{
	/// <summary>
    /// Reads method body
    /// </summary>
    internal sealed class MethodBody : IClrMethodBody
	{
		private readonly IMethod _method;
		private readonly int _maxStackSize;
		private IVariableCollection _vars;
		private readonly IReadOnlyList<TryCatchBlock> _protectedBlocks;
		private readonly ILStream _code;

		private readonly bool _hasGenericVars;
		private bool _hasGenericInstructions;
		private bool _hasGenericExceptions;

		private readonly List<int> _tokens = new List<int>();
		private readonly Hashtable _tokenCache = new Hashtable();

        #region ctor
        public MethodBody(IMethod method, IMethodContext context, BufferedBinaryReader reader)
        {
	        if (method == null)
				throw new ArgumentNullException("method");
	        if (context == null)
				throw new ArgumentNullException("context");
	        if (reader == null)
				throw new ArgumentNullException("reader");

	        _method = method;
            _method.Body = this;

            int lsb = reader.ReadUInt8();
            var flags = (MethodBodyFlags)lsb;
            _maxStackSize = 8;

            var format = flags & MethodBodyFlags.FormatMask;
            List<SEHBlock> sehBlocks = null;

            switch (format)
            {
                case MethodBodyFlags.FatFormat:
                    {
                        byte msb = reader.ReadUInt8();
                        int dwordMultipleSize = (msb & 0xF0) >> 4;
                        Debug.Assert(dwordMultipleSize == 3); // the fat header is 3 dwords
                        _maxStackSize = reader.ReadUInt16();
                        int codeSize = reader.ReadInt32();
                        int localSig = reader.ReadInt32();
                        flags = (MethodBodyFlags)((msb & 0x0F) << 8 | lsb);
                        var code = reader.ReadBytes(codeSize);

                        if ((flags & MethodBodyFlags.MoreSects) != 0)
                        {
                            sehBlocks = ReadSehBlocks(reader);
                        }

                        _code = ReadCode(method, context, code);
                        _vars = context.ResolveLocalVariables(method, localSig, out _hasGenericVars);
                    }
                    break;

                case MethodBodyFlags.TinyFormat:
                case MethodBodyFlags.TinyFormat1:
                    {
                        int codeSize = (lsb >> 2);
                        var code = reader.ReadBytes(codeSize);
                        _code = ReadCode(method, context, code);
                    }
                    break;

				default:
					throw new NotSupportedException("Not supported method body format!");
            }

            TranslateOffsets();

            if (sehBlocks != null)
            {
                _protectedBlocks = TranslateSehBlocks(method, context, sehBlocks, _code);
            }

            context.LinkDebugInfo(this);
        }
        #endregion

        #region Public Properties
        public IMethod Method
        {
            get { return _method; }
        }

        public int MaxStackSize
        {
            get { return _maxStackSize; }
        }

        public IVariableCollection LocalVariables
        {
            get { return _vars ?? (_vars = new VariableCollection()); }
        }
        
        public IStatementCollection Statements { get; set; }

        /// <summary>
        /// Provides translator that can be used to translate this method body using specific <see cref="ICodeProvider"/>.
        /// </summary>
        /// <returns><see cref="ITranslator"/></returns>
        public ITranslator CreateTranslator()
        {
            return new Translator();
        }

        public bool HasProtectedBlocks
        {
            get { return _protectedBlocks != null && _protectedBlocks.Count > 0; }
        }

        public IReadOnlyList<TryCatchBlock> ProtectedBlocks
        {
            get { return _protectedBlocks; }
        }
        
        public ILStream Code
        {
            get { return _code; }
        }
        
        /// <summary>
        /// Gets all calls that can be invocated in the method.
        /// </summary>
        /// <returns></returns>
        public IMethod[] GetCalls()
        {
            if (_code == null)
                return new IMethod[0];
			return _code.Where<Instruction>(x => x.FlowControl == FlowControl.Call).Select(x => x.Method).ToArray();
        }

        /// <summary>
        /// Gets all metadata tokens referenced by the method.
        /// </summary>
        /// <returns></returns>
        public int[] GetReferencedMetadataTokens()
        {
            return _tokens.ToArray();
        }
        
        void AddToken(int token)
        {
            if (_tokenCache.ContainsKey(token))
                return;
            _tokens.Add(token);
            _tokenCache[token] = this;
        }

        public bool HasGenerics
        {
            get { return _hasGenericInstructions || _hasGenericVars || _hasGenericExceptions; }
        }

    	public bool HasGenericVars
    	{
    		get { return _hasGenericVars; }
    	}

    	public bool HasGenericInstructions
    	{
    		get { return _hasGenericInstructions; }
    	}

    	public bool HasGenericExceptions
    	{
    		get { return _hasGenericExceptions; }
    	}

        //Number of compilations
    	public int InstanceCount { get; set; }

		public ControlFlowGraph ControlFlowGraph { get; set; }

        #endregion

        #region DebuInfo
        public void LinkSequencePoints(IEnumerable<SequencePoint> points)
        {
            if (points == null) return;
            var code = Code;
            foreach (var p in points)
            {
                if (p == null) continue;
                var instr = code.FindByOffset(p.Offset);
                if (instr != null)
                    instr.SequencePoint = p;
            }
        }

        public void LinkSequencePoints(ISymbolMethod symMethod)
        {
            if (symMethod == null) return;
            var points = symMethod.ReadSequencePoints();
            LinkSequencePoints(points);
        }
        #endregion

        #region Private Members
        #region ReadCode
        private ILStream ReadCode(IMethod method, IMethodContext context, byte[] code)
        {
            var list = new ILStream();
            var reader = new BufferedBinaryReader(code);
            while (reader.Position < reader.Length)
            {
                var instr = ReadInstruction(method, context, reader);
                instr.Index = list.Count;
                list.Add(instr);

                if (!_hasGenericInstructions && instr.IsGenericContext)
                    _hasGenericInstructions = true;
            }
            return list;
        }

        private Instruction ReadInstruction(IMethod method, IMethodContext context, BufferedBinaryReader reader)
        {
	        var instr = new Instruction
		        {
			        Offset = ((int)reader.Position),
			        OpCode = OpCodes.Nop
		        };

            byte op = reader.ReadUInt8();
            OpCode? opCode;
            if (op != CIL.MultiBytePrefix)
            {
                opCode = CIL.GetShortOpCode(op);
            }
            else
            {
                op = reader.ReadUInt8();
                opCode = CIL.GetLongOpCode(op);
            }

            if (!opCode.HasValue)
                throw new BadImageFormatException(string.Format("The format of instruction with code {0} is invalid", op));

            instr.OpCode = opCode.Value;

            //Read operand
            switch (instr.OpCode.OperandType)
            {
                case OperandType.InlineI:
                    instr.Value = reader.ReadInt32();
                    break;

                case OperandType.ShortInlineI:
                    instr.Value = (int)reader.ReadSByte();
                    break;

                case OperandType.InlineI8:
                    instr.Value = reader.ReadInt64();
                    break;

                case OperandType.InlineR:
                    instr.Value = reader.ReadDouble();
                    break;

                case OperandType.ShortInlineR:
                    instr.Value = reader.ReadSingle();
                    break;

                case OperandType.InlineBrTarget:
                    {
                        int offset = reader.ReadInt32();
                        instr.Value = (int)(offset + reader.Position);
                    }
                    break;

                case OperandType.ShortInlineBrTarget:
                    {
                        int offset = reader.ReadSByte();
                        instr.Value = (int)(offset + reader.Position);
                    }
                    break;

                case OperandType.InlineSwitch:
                    {
                        int casesCount = reader.ReadInt32();
                        var switchBranches = new int[casesCount];
                        for (int k = 0; k < casesCount; k++)
                            switchBranches[k] = reader.ReadInt32();
                        int pos = (int)reader.Position;
                        for (int k = 0; k < casesCount; k++)
                            switchBranches[k] += pos;
                        instr.Value = switchBranches;
                    }
                    break;

                case OperandType.InlineVar:
                    instr.Value = (int)reader.ReadUInt16();
                    break;

                case OperandType.ShortInlineVar:
                    instr.Value = reader.ReadByte();
                    break;

                case OperandType.InlineString:
                    {
                        int token = reader.ReadInt32();
                        instr.Value = context.ResolveMetadataToken(method, token);
                    }
                    break;

                case OperandType.InlineField:
                case OperandType.InlineMethod:
                case OperandType.InlineSig:
                case OperandType.InlineTok:
                case OperandType.InlineType:
                    {
                        int token = reader.ReadInt32();
                        instr.MetadataToken = token;
                        
                        object val = context.ResolveMetadataToken(method, token);
                        if (val is ITypeMember)
                            AddToken(token);

                        if (val == null)
                        {
#if DEBUG
                            if (DebugHooks.BreakInvalidMetadataToken)
                            {
                                Debugger.Break();
                                val = context.ResolveMetadataToken(method, token);
                            }
#endif
                            throw new BadTokenException(token);
                        }

                        instr.Value = val;
                    }
                    break;

				case OperandType.InlineNone:
					// no operand
					break;

				case OperandType.InlinePhi:
					throw new BadImageFormatException(@"Obsolete. The InlinePhi operand is reserved and should not be used!");
            }

            return instr;
        }
        #endregion

        #region ReadSehBlocks
        [Flags]
        enum SectionFlags
        {
            EHTable = 0x01,
            OptILTable = 0x02,
            FatFormat = 0x40,
            MoreSects = 0x80,
        }

        static List<SEHBlock> ReadSehBlocks(BufferedBinaryReader reader)
        {
            const int FatSize = 24;
            const int TinySize = 12;
            var blocks = new List<SEHBlock>();
            bool next = true;
            while (next)
            {
                // Goto 4 byte boundary (each section has to start at 4 byte boundary)
                reader.Align4();

                uint header = reader.ReadUInt32();
                var sf = (SectionFlags)(header & 0xFF);
                int size = (int)(header >> 8); //in bytes
                if ((sf & SectionFlags.OptILTable) != 0)
                {
                    
                }
                else if ((sf & SectionFlags.FatFormat) == 0)
                {
                    // tiny header
                    size &= 0xFF; // 1 byte size (filter out the padding)
                    int n = size / TinySize;
                    for (int i = 0; i < n; ++i)
                    {
                        var block = new SEHBlock(reader, false);
                        blocks.Add(block);
                    }
                }
                else
                {
                    //make sure this is an exception block , otherwise skip
                    if ((sf & SectionFlags.EHTable) != 0)
                    {
                        int n = size / FatSize;
                        for (int i = 0; i < n; ++i)
                        {
                            var block = new SEHBlock(reader, true);
                            blocks.Add(block);
                        }
                    }
                    else
                    {
                        reader.Position += size;
                    }
                }

                next = (sf & SectionFlags.MoreSects) != 0;
            }

            return blocks;
        }
        #endregion

        #region TranslateOffsets
        //Translates branch offsets to instruction indicies
        void TranslateOffsets()
        {
            var list = Code;
//#if DEBUG
//            CIL.UpdateCoverage(list);
//#endif
            list.TranslateOffsets();
        }
        #endregion

        #region TranslateSehBlocks
        private HandlerBlock CreateHandlerBlock(IMethod method, IMethodContext context, IInstructionList code, SEHBlock block)
        {
            switch (block.Type)
            {
                case SEHFlags.Catch:
                    {
                        int token = block.Value;
                        AddToken(token);
                        var type = context.ResolveType(method, token);
                        if (!_hasGenericExceptions && GenericType.IsGenericContext(type))
                            _hasGenericExceptions = true;
                        var h = new HandlerBlock(BlockType.Catch)
                                    {
                                        ExceptionType = type
                                    };
                        return h;
                    }

                case SEHFlags.Filter:
                    {
                        var h = new HandlerBlock(BlockType.Filter)
                                    {
                                        FilterIndex = code.GetOffsetIndex(block.Value)
                                    };
                        return h;
                    }

                case SEHFlags.Finally:
                    return new HandlerBlock(BlockType.Finally);

                case SEHFlags.Fault:
                    return new HandlerBlock(BlockType.Fault);

                default:
                    throw new IndexOutOfRangeException();
            }
        }

        private static Block FindParent(IEnumerable<TryCatchBlock> list, Block block)
        {
			return list.FirstOrDefault(parent => parent != block && (block.EntryIndex >= parent.EntryIndex && block.ExitIndex <= parent.ExitIndex));
        }

    	private static void SetupInstructions(ILStream code, Block block)
        {
            foreach (var kid in block.Kids)
            {
                SetupInstructions(code, kid);
            }

            var tryBlock = block as TryCatchBlock;
            if (tryBlock != null)
            {
                foreach (var h in tryBlock.Handlers.Cast<HandlerBlock>())
                {
                    SetupInstructions(code, h);
                }
            }

            block.SetupInstructions(code);
        }

        static int GetIndex(ILStream code, int offset, int length)
        {
			int index = code.GetOffsetIndex(offset + length);
			if (index == code.Count - 1)
			{
				return index;
			}

            index--;
        	return index >= 0 ? index : code.Count - 1;
        }

        static TryCatchBlock CreateTryBlock(ILStream code, int tryOffset, int tryLength)
        {
            int entryIndex = code.GetOffsetIndex(tryOffset);
            int exitIndex = GetIndex(code, tryOffset, tryLength);
            var tryBlock = new TryCatchBlock
                               {
                                   EntryPoint = code[entryIndex],
                                   ExitPoint = code[exitIndex]
                               };
            return tryBlock;
        }

        private IReadOnlyList<TryCatchBlock> TranslateSehBlocks(IMethod method, IMethodContext context, IList<SEHBlock> blocks, ILStream code)
        {
        	var list = new List<TryCatchBlock>();
            var handlers = new BlockList();
            TryCatchBlock tryBlock = null;
            int n = blocks.Count;
            for (int i = 0; i < n; ++i)
            {
                var block = blocks[i];
                tryBlock = EnshureTryBlock(blocks, i, tryBlock, code, block, list);
                var handler = CreateHandlerBlock(method, context, code, block);
                int entryIndex = code.GetOffsetIndex(block.HandlerOffset);
                int exitIndex = GetIndex(code, block.HandlerOffset, block.HandlerLength);
                handler.EntryPoint = code[entryIndex];
                handler.ExitPoint = code[exitIndex];
                tryBlock.Handlers.Add(handler);
                handlers.Add(handler);
            }

            //set parents
            for (int i = 0; i < list.Count; ++i)
            {
                var block = list[i];
                var parent = FindParent(list, block);
                if (parent != null)
                {
                    parent.Add(block);
                    list.RemoveAt(i);
                    --i;
                }
            }

            foreach (var block in list)
            {
                SetupInstructions(code, block);
            }

            return list.AsReadOnlyList();
        }

        static TryCatchBlock EnshureTryBlock(IList<SEHBlock> blocks, int i, TryCatchBlock tryBlock, ILStream code, SEHBlock block, ICollection<TryCatchBlock> list)
        {
            if (tryBlock == null)
            {
                tryBlock = CreateTryBlock(code, block.TryOffset, block.TryLength);
                list.Add(tryBlock);
            }
            else /*if (block.Type == SEHFlags.Catch)*/
            {
                var prev = blocks[i - 1];
                if (prev.TryOffset != block.TryOffset
                    || prev.TryLength != block.TryLength)
                {
                    tryBlock = CreateTryBlock(code, block.TryOffset, block.TryLength);
                    list.Add(tryBlock);
                }
            }
            //else if (block.Type == SEHFlags.Finally || block.Type == SEHFlags.Fault)
            //{
            //    SEHBlock prev = blocks[i - 1];
            //    if (block.HandlerOffset - prev.HandlerEnd > 2)
            //    {
            //        tryBlock = CreateTryBlock(code, block.TryOffset, block.TryLength);
            //        list.Add(tryBlock);
            //    }
            //}
            return tryBlock;
        }
        #endregion
        #endregion

        #region Object Override Members
        public override string ToString()
        {
            return _method.DeclaringType.FullName + "." + _method.Name;
        }
        #endregion
    }

    #region enum MethodBodyFlags
    /// <summary>
    /// IL method body flags
    /// </summary>
    [Flags]
    internal enum MethodBodyFlags
    {
        None = 0,

        /// <summary>
        /// Small Code 
        /// </summary>
        SmallFormat = 0x00,

        /// <summary>
        /// Tiny code format (use this code if the code size is even)
        /// </summary>
        TinyFormat = 0x02,

        /// <summary>
        /// Fat code format
        /// </summary>
        FatFormat = 0x03,

        /// <summary>
        /// Use this code if the code size is odd 
        /// </summary>
        TinyFormat1 = 0x06,

        /// <summary>
        /// Mask for extract code type
        /// </summary>
        FormatMask = 0x07,

        /// <summary>
        /// Runtime call default constructor on all local vars
        /// </summary>
        InitLocals = 0x10,

        /// <summary>
        /// There is another attribute after this one
        /// </summary>
        MoreSects = 0x08,
    }
    #endregion
}