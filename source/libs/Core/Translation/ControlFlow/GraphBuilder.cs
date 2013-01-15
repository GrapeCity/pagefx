//http://en.wikipedia.org/wiki/Basic_block

//Instructions that end a basic block include:
//* Unconditional and conditional branches, both direct and indirect.
//* Returns to a calling procedure
//* Instructions which may throw an exception
//* Function calls can be at the end of a basic block if they may not return, 
//  such as functions which throw exceptions or special calls like C's longjmp and exit.

//Instructions which begin a new basic block include:
//* Procedure and function entry points
//* Targets of jumps or branches
//* "Fall-through" instructions following some conditional branches
//* Instructions following ones that throw exceptions
//* Exception handlers

using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.IL;

namespace DataDynamics.PageFX.Core.Translation.ControlFlow
{
	/// <summary>
	/// Builds control flow graph.
	/// </summary>
    internal class GraphBuilder
    {
        readonly ILStream _code;
        readonly bool _removeSingleGotos;

        public GraphBuilder(IClrMethodBody body, bool removeSingleGotos)
        {
            _code = body.Code;
            _removeSingleGotos = removeSingleGotos;
        }

        /// <summary>
        /// Builds control flow graph.
        /// </summary>
        /// <returns>first entry point to CFG</returns>
        public Node Build()
        {
            Node entry = null;
            Node prevEntry = null;
            int nodeIndex = 0;
            int n = _code.Count;
            for (int i = 0; i < n; ++i)
            {
                var instruction = GetTarget(i);
                var node = instruction.BasicBlock;
                if (node == null)
                {
                    node = BuildNode(instruction);
                    node.IsEntry = true;
                    Node last = null;
                    BuildNodeList(ref last, node, ref nodeIndex);

					if (entry == null)
					{
						entry = node;
					}
					else if (prevEntry != null)
					{
						prevEntry.AppendEntry(node);
					}

                	prevEntry = node;
                }
				else if (entry == null)
                {
					entry = node;
                }
            }

            return entry;
        }

        static void BuildNodeList(ref Node last, Node node, ref int index)
        {
            if (node.Index >= 0) return;

            node.Index = index;
            ++index;

            if (last != null)
                last.Append(node);
            last = node;

            node.InStack = true;
            foreach (var e in node.OutEdges)
            {
                var to = e.To;
                if (to.Index < 0)
                {
                    e.Type = EdgeType.Tree;
                    BuildNodeList(ref last, to, ref index);
                }
                else if (to.InStack)
                {
                    e.Type = EdgeType.Back;
                }
                else if (node.Index > to.Index)
                {
                    e.Type = EdgeType.Forward;
                }
                else
                {
                    e.Type = EdgeType.Cross;
                }
            }
            node.InStack = false;
        }

        Node BuildNode(Instruction entryPoint)
        {
            var header = new Node();
            var next = BuildNode(header, entryPoint);
            if (header.CodeLength == 0)
                throw new ILTranslatorException();
            if (next != null)
            {
                var nodeStack = new Stack<Node>();
                var nextStack = new Stack<int[]>();
                nodeStack.Push(header);
                nextStack.Push(next);
                while (nodeStack.Count > 0)
                {
                    var node = nodeStack.Pop();
                    next = nextStack.Pop();
                    foreach (var index in next)
                    {
                        if (index < 0 || index >= _code.Count)
                            throw new ILTranslatorException();
                        var instruction = _code[index];
                        var nextNode = instruction.BasicBlock;
                        if (nextNode != null)
                        {
                            //Note: can be switch
                            //if (node.HasOut(nextNode))
                            //    throw new DecompileException();
                            node.AppendOut(nextNode);
                        }
                        else
                        {
                            nextNode = new Node();
                            var nextNext = BuildNode(nextNode, instruction);
                            if (nextNode.CodeLength == 0)
                                throw new ILTranslatorException();
                            //Note: can be switch
                            //if (node.HasOut(nextNode))
                            //    throw new DecompileException();
                            node.AppendOut(nextNode);
                            if (nextNext != null && nextNext.Length > 0)
                            {
                                nodeStack.Push(nextNode);
                                nextStack.Push(nextNext);
                            }
                        }
                    }
                    if (next.Length == 2)
                    {
                        node.FirstOut.Label = 0;
                        node.FirstOut.NextOut.Label = 1;
                    }
                }
            }
            return header;
        }

    	private int GetTargetIndex(int index)
        {
            if (_removeSingleGotos)
                return _code.GetTargetIndex(index);
            //FIX: to avoid skipping of finally block
            //Instruction i = _code[index];
            //if (i.IsEndOfTryFinally)
            //{
            //    return GetTargetIndex(index + 1);
            //}
            return index;
        }

        Instruction GetTarget(int index)
        {
            index = GetTargetIndex(index);
            return _code[index];
        }

        int[] Next(int index)
        {
            int target = GetTargetIndex(index);
            return new[] { target };
        }

        static bool IsReturnOrThrow(IInstruction i)
        {
            return i.IsReturn || i.IsThrow;
        }

        public bool IsVoidCallEnd { get; set; }

        int[] BuildNode(Node node, Instruction entryPoint)
        {
            var block = entryPoint.SehBlock;
            node.OwnerBlock = block;

            int n = _code.Count;
            int entryIndex = entryPoint.Index;
            for (int i = entryIndex; i < n; ++i)
            {
                var instruction = _code[i];
                var op = instruction.Code;
                if (instruction.BasicBlock != null)
                    return Next(i);

                if (instruction.IsBranchTarget)
                {
					if (i != entryIndex)
					{
						return Next(i);
					}
                }

                if (op == InstructionCode.Endfinally)
                {
                    node.AddInstruction(instruction);
                    var v = instruction.Value;
                    if (v != null)
                        return Next((int)v);
                    return null;
                }

                if (op == InstructionCode.Endfilter)
                {
                    //TODO:
                    throw new NotImplementedException();
                    //node.AddInstruction(instruction);
                    //return Next(i + 1);
                }

                if (IsReturnOrThrow(instruction))
                {
                    node.AddInstruction(instruction);
                    return null;
                }

                if (IsVoidCallEnd && instruction.IsCall && instruction.Method.IsVoid())
                {
                    node.AddInstruction(instruction);
                    return Next(i + 1);
                }

                if (instruction.SehBlock != block)
                {
                    return Next(i);
                }

                if (instruction.IsUnconditionalBranch)
                {
                    node.AddInstruction(instruction);
                    return Next((int)instruction.Value);
                }

                if (instruction.IsSwitch)
                {
                    node.AddInstruction(instruction);
                    return Switch(instruction);
                }

                //first edge is false, second - true
                if (instruction.IsConditionalBranch)
                {
                    node.AddInstruction(instruction);
                    return Branch(instruction);
                }

                node.AddInstruction(instruction);
            }

            return null;
        }

        int[] Branch(IInstruction instruction)
        {
            int index = instruction.Index;
            int next = GetTargetIndex(index + 1);
            int target = GetTargetIndex((int)instruction.Value);
            return new[] { next, target };
        }

        int[] Switch(IInstruction instruction)
        {
            int index = instruction.Index;
            int target = GetTargetIndex(index + 1);
            var targets = (int[])instruction.Value;
            int n = targets.Length;
            var next = new int[n + 1];
            next[0] = target;
            for (int k = 0; k < n; ++k)
            {
                target = GetTargetIndex(targets[k]);
                targets[k] = target;
                next[k + 1] = target;
            }
            return next;
        }
    }
}