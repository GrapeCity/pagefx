namespace Microsoft.VisualBasic.CompilerServices
{
    using Microsoft.VisualBasic;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;

    public sealed class NewLateBinding
    {
        private NewLateBinding()
        {
        }

        private static object CallMethod(Symbols.Container BaseReference, string MethodName, object[] Arguments, string[] ArgumentNames, Type[] TypeArguments, bool[] CopyBack, BindingFlags InvocationFlags, bool ReportErrors, ref OverloadResolution.ResolutionFailure Failure)
        {
            Failure = OverloadResolution.ResolutionFailure.None;
            if ((ArgumentNames.Length > Arguments.Length) || ((CopyBack != null) && (CopyBack.Length != Arguments.Length)))
            {
                Failure = OverloadResolution.ResolutionFailure.InvalidArgument;
                if (ReportErrors)
                {
                    throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue"));
                }
                return null;
            }
            if (Symbols.HasFlag(InvocationFlags, BindingFlags.SetProperty) && (Arguments.Length < 1))
            {
                Failure = OverloadResolution.ResolutionFailure.InvalidArgument;
                if (ReportErrors)
                {
                    throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue"));
                }
                return null;
            }
            MemberInfo[] members = BaseReference.GetMembers(ref MethodName, ReportErrors);
            if ((members == null) || (members.Length == 0))
            {
                Failure = OverloadResolution.ResolutionFailure.MissingMember;
                if (ReportErrors)
                {
                    members = BaseReference.GetMembers(ref MethodName, true);
                }
                return null;
            }
            Symbols.Method targetProcedure = ResolveCall(BaseReference, MethodName, members, Arguments, ArgumentNames, TypeArguments, InvocationFlags, ReportErrors, ref Failure);
            if (Failure == OverloadResolution.ResolutionFailure.None)
            {
                return BaseReference.InvokeMethod(targetProcedure, Arguments, CopyBack, InvocationFlags);
            }
            return null;
        }

        internal static object[] ConstructCallArguments(Symbols.Method TargetProcedure, object[] Arguments, BindingFlags LookupFlags)
        {
            ParameterInfo[] parameters = GetCallTarget(TargetProcedure, LookupFlags).GetParameters();
            object[] matchedArguments = new object[(parameters.Length - 1) + 1];
            int length = Arguments.Length;
            object argument = null;
            if (Symbols.HasFlag(LookupFlags, BindingFlags.SetProperty))
            {
                object[] sourceArray = Arguments;
                Arguments = new object[(length - 2) + 1];
                Array.Copy(sourceArray, Arguments, Arguments.Length);
                argument = sourceArray[length - 1];
            }
            OverloadResolution.MatchArguments(TargetProcedure, Arguments, matchedArguments);
            if (Symbols.HasFlag(LookupFlags, BindingFlags.SetProperty))
            {
                ParameterInfo parameter = parameters[parameters.Length - 1];
                matchedArguments[parameters.Length - 1] = OverloadResolution.PassToParameter(argument, parameter, parameter.ParameterType);
            }
            return matchedArguments;
        }

        internal static MethodBase GetCallTarget(Symbols.Method TargetProcedure, BindingFlags Flags)
        {
            if (TargetProcedure.IsMethod)
            {
                return TargetProcedure.AsMethod();
            }
            if (TargetProcedure.IsProperty)
            {
                return MatchesPropertyRequirements(TargetProcedure, Flags);
            }
            return null;
        }

        internal static object InternalLateIndexGet(object Instance, object[] Arguments, string[] ArgumentNames, bool ReportErrors, ref OverloadResolution.ResolutionFailure Failure)
        {
            Failure = OverloadResolution.ResolutionFailure.None;
            if (Arguments == null)
            {
                Arguments = Symbols.NoArguments;
            }
            if (ArgumentNames == null)
            {
                ArgumentNames = Symbols.NoArgumentNames;
            }
            Symbols.Container baseReference = new Symbols.Container(Instance);
            if (baseReference.IsCOMObject)
            {
                throw new InvalidOperationException("Never expected to see a COM object in Telesto");
            }
            if (!baseReference.IsArray)
            {
                return CallMethod(baseReference, "", Arguments, ArgumentNames, Symbols.NoTypeArguments, null, BindingFlags.GetProperty | BindingFlags.InvokeMethod, ReportErrors, ref Failure);
            }
            if (ArgumentNames.Length <= 0)
            {
                return baseReference.GetArrayValue(Arguments);
            }
            Failure = OverloadResolution.ResolutionFailure.InvalidArgument;
            if (ReportErrors)
            {
                throw new ArgumentException(Utils.GetResourceString("Argument_InvalidNamedArgs"));
            }
            return null;
        }

        [DebuggerStepThrough, DebuggerHidden]
        public static object LateCall(object Instance, Type Type, string MemberName, object[] Arguments, string[] ArgumentNames, Type[] TypeArguments, bool[] CopyBack, bool IgnoreReturn)
        {
            Symbols.Container container;
            OverloadResolution.ResolutionFailure failure = 0;
            if (Arguments == null)
            {
                Arguments = Symbols.NoArguments;
            }
            if (ArgumentNames == null)
            {
                ArgumentNames = Symbols.NoArgumentNames;
            }
            if (TypeArguments == null)
            {
                TypeArguments = Symbols.NoTypeArguments;
            }
            if (Type != null)
            {
                container = new Symbols.Container(Type);
            }
            else
            {
                container = new Symbols.Container(Instance);
            }
            if (container.IsCOMObject)
            {
                throw new InvalidOperationException("Never expected to see a COM object in Telesto");
            }
            BindingFlags invocationFlags = BindingFlags.GetProperty | BindingFlags.InvokeMethod;
            if (IgnoreReturn)
            {
                invocationFlags |= BindingFlags.IgnoreReturn;
            }
            return CallMethod(container, MemberName, Arguments, ArgumentNames, TypeArguments, CopyBack, invocationFlags, true, ref failure);
        }

        [DebuggerStepThrough, DebuggerHidden]
        public static bool LateCanEvaluate(object instance, Type type, string memberName, object[] arguments, bool allowFunctionEvaluation, bool allowPropertyEvaluation)
        {
            Symbols.Container container;
            if (type != null)
            {
                container = new Symbols.Container(type);
            }
            else
            {
                container = new Symbols.Container(instance);
            }
            MemberInfo[] members = container.GetMembers(ref memberName, false);
            if (members.Length != 0)
            {
                if (members[0].MemberType == MemberTypes.Field)
                {
                    if (arguments.Length == 0)
                    {
                        return true;
                    }
                    container = new Symbols.Container(container.GetFieldValue((FieldInfo) members[0]));
                    return (container.IsArray || allowPropertyEvaluation);
                }
                if (members[0].MemberType == MemberTypes.Method)
                {
                    return allowFunctionEvaluation;
                }
                if (members[0].MemberType == MemberTypes.Property)
                {
                    return allowPropertyEvaluation;
                }
            }
            return true;
        }

        [DebuggerStepThrough, DebuggerHidden]
        public static object LateGet(object Instance, Type Type, string MemberName, object[] Arguments, string[] ArgumentNames, Type[] TypeArguments, bool[] CopyBack)
        {
            Symbols.Container container;
            OverloadResolution.ResolutionFailure failure = 0;
            if (Arguments == null)
            {
                Arguments = Symbols.NoArguments;
            }
            if (ArgumentNames == null)
            {
                ArgumentNames = Symbols.NoArgumentNames;
            }
            if (TypeArguments == null)
            {
                TypeArguments = Symbols.NoTypeArguments;
            }
            if (Type != null)
            {
                container = new Symbols.Container(Type);
            }
            else
            {
                container = new Symbols.Container(Instance);
            }
            BindingFlags lookupFlags = BindingFlags.GetProperty | BindingFlags.InvokeMethod;
            if (container.IsCOMObject)
            {
                throw new InvalidOperationException("Never expected to see a COM object in Telesto");
            }
            MemberInfo[] members = container.GetMembers(ref MemberName, true);
            if (members[0].MemberType == MemberTypes.Field)
            {
                if (TypeArguments.Length > 0)
                {
                    throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue"));
                }
                object fieldValue = container.GetFieldValue((FieldInfo) members[0]);
                if (Arguments.Length == 0)
                {
                    return fieldValue;
                }
                return LateIndexGet(fieldValue, Arguments, ArgumentNames);
            }
            if ((ArgumentNames.Length > Arguments.Length) || ((CopyBack != null) && (CopyBack.Length != Arguments.Length)))
            {
                throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue"));
            }
            Symbols.Method targetProcedure = ResolveCall(container, MemberName, members, Arguments, ArgumentNames, TypeArguments, lookupFlags, false, ref failure);
            if (failure == OverloadResolution.ResolutionFailure.None)
            {
                return container.InvokeMethod(targetProcedure, Arguments, CopyBack, lookupFlags);
            }
            if (Arguments.Length > 0)
            {
                targetProcedure = ResolveCall(container, MemberName, members, Symbols.NoArguments, Symbols.NoArgumentNames, TypeArguments, lookupFlags, false, ref failure);
                if (failure == OverloadResolution.ResolutionFailure.None)
                {
                    object instance = container.InvokeMethod(targetProcedure, Symbols.NoArguments, null, lookupFlags);
                    if (instance == null)
                    {
                        throw new MissingMemberException(Utils.GetResourceString("IntermediateLateBoundNothingResult1", new string[] { targetProcedure.ToString(), container.VBFriendlyName }));
                    }
                    instance = InternalLateIndexGet(instance, Arguments, ArgumentNames, false, ref failure);
                    if (failure == OverloadResolution.ResolutionFailure.None)
                    {
                        return instance;
                    }
                }
            }
            ResolveCall(container, MemberName, members, Arguments, ArgumentNames, TypeArguments, lookupFlags, true, ref failure);
            throw new InternalErrorException();
        }

        [DebuggerHidden, DebuggerStepThrough]
        public static object LateIndexGet(object Instance, object[] Arguments, string[] ArgumentNames)
        {
            OverloadResolution.ResolutionFailure failure = 0;
            return InternalLateIndexGet(Instance, Arguments, ArgumentNames, true, ref failure);
        }

        [DebuggerStepThrough, DebuggerHidden]
        public static void LateIndexSet(object Instance, object[] Arguments, string[] ArgumentNames)
        {
            LateIndexSetComplex(Instance, Arguments, ArgumentNames, false, false);
        }

        [DebuggerHidden, DebuggerStepThrough]
        public static void LateIndexSetComplex(object Instance, object[] Arguments, string[] ArgumentNames, bool OptimisticSet, bool RValueBase)
        {
            if (Arguments == null)
            {
                Arguments = Symbols.NoArguments;
            }
            if (ArgumentNames == null)
            {
                ArgumentNames = Symbols.NoArgumentNames;
            }
            Symbols.Container baseReference = new Symbols.Container(Instance);
            if (baseReference.IsArray)
            {
                if (ArgumentNames.Length > 0)
                {
                    throw new ArgumentException(Utils.GetResourceString("Argument_InvalidNamedArgs"));
                }
                baseReference.SetArrayValue(Arguments);
            }
            else
            {
                OverloadResolution.ResolutionFailure failure = 0;
                if (ArgumentNames.Length > Arguments.Length)
                {
                    throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue"));
                }
                if (Arguments.Length < 1)
                {
                    throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue"));
                }
                string memberName = "";
                if (baseReference.IsCOMObject)
                {
                    throw new InvalidOperationException("Never expected to see a COM object in Telesto");
                }
                BindingFlags setProperty = BindingFlags.SetProperty;
                MemberInfo[] members = baseReference.GetMembers(ref memberName, true);
                Symbols.Method targetProcedure = ResolveCall(baseReference, memberName, members, Arguments, ArgumentNames, Symbols.NoTypeArguments, setProperty, false, ref failure);
                if (failure == OverloadResolution.ResolutionFailure.None)
                {
                    if (RValueBase && baseReference.IsValueType)
                    {
                        throw new Exception(Utils.GetResourceString("RValueBaseForValueType", new string[] { baseReference.VBFriendlyName, baseReference.VBFriendlyName }));
                    }
                    baseReference.InvokeMethod(targetProcedure, Arguments, null, setProperty);
                }
                else if (!OptimisticSet)
                {
                    ResolveCall(baseReference, memberName, members, Arguments, ArgumentNames, Symbols.NoTypeArguments, setProperty, true, ref failure);
                    throw new InternalErrorException();
                }
            }
        }

        [DebuggerStepThrough, DebuggerHidden]
        public static void LateSet(object Instance, Type Type, string MemberName, object[] Arguments, string[] ArgumentNames, Type[] TypeArguments)
        {
            LateSet(Instance, Type, MemberName, Arguments, ArgumentNames, TypeArguments, false, false, (CallType) 0);
        }

        [DebuggerHidden, DebuggerStepThrough]
        public static void LateSet(object Instance, Type Type, string MemberName, object[] Arguments, string[] ArgumentNames, Type[] TypeArguments, bool OptimisticSet, bool RValueBase, CallType CallType)
        {
            Symbols.Container container;
            if (Arguments == null)
            {
                Arguments = Symbols.NoArguments;
            }
            if (ArgumentNames == null)
            {
                ArgumentNames = Symbols.NoArgumentNames;
            }
            if (TypeArguments == null)
            {
                TypeArguments = Symbols.NoTypeArguments;
            }
            if (Type != null)
            {
                container = new Symbols.Container(Type);
            }
            else
            {
                container = new Symbols.Container(Instance);
            }
            if (container.IsCOMObject)
            {
                throw new InvalidOperationException();
            }
            MemberInfo[] members = container.GetMembers(ref MemberName, true);
            if (members[0].MemberType == MemberTypes.Field)
            {
                if (TypeArguments.Length > 0)
                {
                    throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue"));
                }
                if (Arguments.Length == 1)
                {
                    if (RValueBase && container.IsValueType)
                    {
                        throw new Exception(Utils.GetResourceString("RValueBaseForValueType", new string[] { container.VBFriendlyName, container.VBFriendlyName }));
                    }
                    container.SetFieldValue((FieldInfo) members[0], Arguments[0]);
                }
                else
                {
                    LateIndexSetComplex(container.GetFieldValue((FieldInfo) members[0]), Arguments, ArgumentNames, OptimisticSet, true);
                }
            }
            else
            {
                OverloadResolution.ResolutionFailure failure = 0;
                Symbols.Method method;
                BindingFlags setProperty = BindingFlags.SetProperty;
                if (ArgumentNames.Length > Arguments.Length)
                {
                    throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue"));
                }
                if (TypeArguments.Length == 0)
                {
                    method = ResolveCall(container, MemberName, members, Arguments, ArgumentNames, Symbols.NoTypeArguments, setProperty, false, ref failure);
                    if (failure == OverloadResolution.ResolutionFailure.None)
                    {
                        if (RValueBase && container.IsValueType)
                        {
                            throw new Exception(Utils.GetResourceString("RValueBaseForValueType", new string[] { container.VBFriendlyName, container.VBFriendlyName }));
                        }
                        container.InvokeMethod(method, Arguments, null, setProperty);
                        return;
                    }
                }
                BindingFlags lookupFlags = BindingFlags.GetProperty | BindingFlags.InvokeMethod;
                switch (failure)
                {
                    case OverloadResolution.ResolutionFailure.None:
                    case OverloadResolution.ResolutionFailure.MissingMember:
                        method = ResolveCall(container, MemberName, members, Symbols.NoArguments, Symbols.NoArgumentNames, TypeArguments, lookupFlags, false, ref failure);
                        if (failure == OverloadResolution.ResolutionFailure.None)
                        {
                            object instance = container.InvokeMethod(method, Symbols.NoArguments, null, lookupFlags);
                            if (instance == null)
                            {
                                throw new MissingMemberException(Utils.GetResourceString("IntermediateLateBoundNothingResult1", new string[] { method.ToString(), container.VBFriendlyName }));
                            }
                            LateIndexSetComplex(instance, Arguments, ArgumentNames, OptimisticSet, true);
                            return;
                        }
                        break;
                }
                if (!OptimisticSet)
                {
                    if (TypeArguments.Length == 0)
                    {
                        ResolveCall(container, MemberName, members, Arguments, ArgumentNames, TypeArguments, setProperty, true, ref failure);
                    }
                    else
                    {
                        ResolveCall(container, MemberName, members, Symbols.NoArguments, Symbols.NoArgumentNames, TypeArguments, lookupFlags, true, ref failure);
                    }
                    throw new InternalErrorException();
                }
            }
        }

        [DebuggerStepThrough, DebuggerHidden]
        public static void LateSetComplex(object Instance, Type Type, string MemberName, object[] Arguments, string[] ArgumentNames, Type[] TypeArguments, bool OptimisticSet, bool RValueBase)
        {
            LateSet(Instance, Type, MemberName, Arguments, ArgumentNames, TypeArguments, OptimisticSet, RValueBase, (CallType) 0);
        }

        internal static MethodInfo MatchesPropertyRequirements(Symbols.Method TargetProcedure, BindingFlags Flags)
        {
            if (Symbols.HasFlag(Flags, BindingFlags.SetProperty))
            {
                return TargetProcedure.AsProperty().GetSetMethod();
            }
            return TargetProcedure.AsProperty().GetGetMethod();
        }

        internal static Exception ReportPropertyMismatch(Symbols.Method TargetProcedure, BindingFlags Flags)
        {
            if (Symbols.HasFlag(Flags, BindingFlags.SetProperty))
            {
                return new MissingMemberException(Utils.GetResourceString("NoSetProperty1", new string[] { TargetProcedure.AsProperty().Name }));
            }
            return new MissingMemberException(Utils.GetResourceString("NoGetProperty1", new string[] { TargetProcedure.AsProperty().Name }));
        }

        internal static Symbols.Method ResolveCall(Symbols.Container BaseReference, string MethodName, MemberInfo[] Members, object[] Arguments, string[] ArgumentNames, Type[] TypeArguments, BindingFlags LookupFlags, bool ReportErrors, ref OverloadResolution.ResolutionFailure Failure)
        {
            Failure = OverloadResolution.ResolutionFailure.None;
            if ((Members[0].MemberType != MemberTypes.Method) && (Members[0].MemberType != MemberTypes.Property))
            {
                Failure = OverloadResolution.ResolutionFailure.InvalidTarget;
                if (ReportErrors)
                {
                    throw new ArgumentException(Utils.GetResourceString("ExpressionNotProcedure", new string[] { MethodName, BaseReference.VBFriendlyName }));
                }
                return null;
            }
            int length = Arguments.Length;
            object argument = null;
            if (Symbols.HasFlag(LookupFlags, BindingFlags.SetProperty))
            {
                if (Arguments.Length == 0)
                {
                    Failure = OverloadResolution.ResolutionFailure.InvalidArgument;
                    if (ReportErrors)
                    {
                        throw new InvalidCastException(Utils.GetResourceString("PropertySetMissingArgument1", new string[] { MethodName }));
                    }
                    return null;
                }
                object[] sourceArray = Arguments;
                Arguments = new object[(length - 2) + 1];
                Array.Copy(sourceArray, Arguments, Arguments.Length);
                argument = sourceArray[length - 1];
            }
            Symbols.Method targetProcedure = OverloadResolution.ResolveOverloadedCall(MethodName, Members, Arguments, ArgumentNames, TypeArguments, LookupFlags, ReportErrors, ref Failure);
            if (Failure != OverloadResolution.ResolutionFailure.None)
            {
                return null;
            }
            if (!targetProcedure.ArgumentsValidated && !OverloadResolution.CanMatchArguments(targetProcedure, Arguments, ArgumentNames, TypeArguments, false, null))
            {
                Failure = OverloadResolution.ResolutionFailure.InvalidArgument;
                if (!ReportErrors)
                {
                    return null;
                }
                string str = "";
                List<string> list = new List<string>();
                bool flag = OverloadResolution.CanMatchArguments(targetProcedure, Arguments, ArgumentNames, TypeArguments, false, list);
                foreach (string str2 in list)
                {
                    str = str + "\r\n    " + str2;
                }
                throw new InvalidCastException(Utils.GetResourceString("MatchArgumentFailure2", new string[] { targetProcedure.ToString(), str }));
            }
            if (targetProcedure.IsProperty)
            {
                if (MatchesPropertyRequirements(targetProcedure, LookupFlags) == null)
                {
                    Failure = OverloadResolution.ResolutionFailure.InvalidTarget;
                    if (ReportErrors)
                    {
                        throw ReportPropertyMismatch(targetProcedure, LookupFlags);
                    }
                    return null;
                }
            }
            else if (Symbols.HasFlag(LookupFlags, BindingFlags.SetProperty))
            {
                Failure = OverloadResolution.ResolutionFailure.InvalidTarget;
                if (ReportErrors)
                {
                    throw new MissingMemberException(Utils.GetResourceString("MethodAssignment1", new string[] { targetProcedure.AsMethod().Name }));
                }
                return null;
            }
            if (!Symbols.HasFlag(LookupFlags, BindingFlags.SetProperty))
            {
                return targetProcedure;
            }
            ParameterInfo[] parameters = GetCallTarget(targetProcedure, LookupFlags).GetParameters();
            ParameterInfo parameter = parameters[parameters.Length - 1];
            bool requiresNarrowingConversion = false;
            bool allNarrowingIsFromObject = false;
            if (OverloadResolution.CanPassToParameter(targetProcedure, argument, parameter, false, false, null, ref requiresNarrowingConversion, ref allNarrowingIsFromObject))
            {
                return targetProcedure;
            }
            Failure = OverloadResolution.ResolutionFailure.InvalidArgument;
            if (!ReportErrors)
            {
                return null;
            }
            string str3 = "";
            List<string> errors = new List<string>();
            allNarrowingIsFromObject = false;
            requiresNarrowingConversion = false;
            bool flag2 = OverloadResolution.CanPassToParameter(targetProcedure, argument, parameter, false, false, errors, ref allNarrowingIsFromObject, ref requiresNarrowingConversion);
            foreach (string str4 in errors)
            {
                str3 = str3 + "\r\n    " + str4;
            }
            throw new InvalidCastException(Utils.GetResourceString("MatchArgumentFailure2", new string[] { targetProcedure.ToString(), str3 }));
        }
    }
}

