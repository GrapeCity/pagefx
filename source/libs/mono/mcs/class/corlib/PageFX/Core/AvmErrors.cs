using System;
using System.IO;

public class AvmErrors
{
    #region AVM Error Codes (0 - 1999)
    public const int kOutOfMemoryError = 1000;
    public const int kNotImplementedError = 1001;
    public const int kInvalidPrecisionError = 1002;
    public const int kInvalidRadixError = 1003;
    public const int kInvokeOnIncompatibleObjectError = 1004;
    public const int kArrayIndexNotIntegerError = 1005;
    public const int kCallOfNonFunctionError = 1006;
    public const int kConstructOfNonFunctionError = 1007;
    public const int kAmbiguousBindingError = 1008;
    public const int kConvertNullToObjectError = 1009;
    public const int kConvertUndefinedToObjectError = 1010;
    public const int kIllegalOpcodeError = 1011;
    public const int kLastInstExceedsCodeSizeError = 1012;
    public const int kFindVarWithNoScopeError = 1013;
    public const int kClassNotFoundError = 1014;
    public const int kIllegalSetDxns = 1015;
    public const int kDescendentsError = 1016;
    public const int kScopeStackOverflowError = 1017;
    public const int kScopeStackUnderflowError = 1018;
    public const int kGetScopeObjectBoundsError = 1019;
    public const int kCannotFallOffMethodError = 1020;
    public const int kInvalidBranchTargetError = 1021;
    public const int kIllegalVoidError = 1022;
    public const int kStackOverflowError = 1023;
    public const int kStackUnderflowError = 1024;
    public const int kInvalidRegisterError = 1025;
    public const int kSlotExceedsCountError = 1026;
    public const int kMethodInfoExceedsCountError = 1027;
    public const int kDispIdExceedsCountError = 1028;
    public const int kDispIdUndefinedError = 1029;
    public const int kStackDepthUnbalancedError = 1030;
    public const int kScopeDepthUnbalancedError = 1031;
    public const int kCpoolIndexRangeError = 1032;
    public const int kCpoolEntryWrongTypeError = 1033;
    public const int kCheckTypeFailedError = 1034;
    public const int kIllegalSuperCallError = 1035;
    public const int kCannotAssignToMethodError = 1037;
    public const int kRedefinedError = 1038;
    public const int kCannotVerifyUntilReferencedError = 1039;
    public const int kCantUseInstanceofOnNonObjectError = 1040;
    public const int kIsTypeMustBeClassError = 1041;
    public const int kInvalidMagicError = 1042;
    public const int kInvalidCodeLengthError = 1043;
    public const int kInvalidMethodInfoFlagsError = 1044;
    public const int kUnsupportedTraitsKindError = 1045;
    public const int kMethodInfoOrderError = 1046;
    public const int kMissingEntryPointError = 1047;
    public const int kPrototypeTypeError = 1049;
    public const int kConvertToPrimitiveError = 1050;
    public const int kIllegalEarlyBindingError = 1051;
    public const int kInvalidURIError = 1052;
    public const int kIllegalOverrideError = 1053;
    public const int kIllegalExceptionHandlerError = 1054;
    public const int kWriteSealedError = 1056;
    public const int kIllegalSlotError = 1057;
    public const int kIllegalOperandTypeError = 1058;
    public const int kClassInfoOrderError = 1059;
    public const int kClassInfoExceedsCountError = 1060;
    public const int kNumberOutOfRangeError = 1061;
    public const int kWrongArgumentCountError = 1063;
    public const int kCannotCallMethodAsConstructor = 1064;
    public const int kUndefinedVarError = 1065;
    public const int kFunctionConstructorError = 1066;
    public const int kIllegalNativeMethodBodyError = 1067;
    public const int kCannotMergeTypesError = 1068;
    public const int kReadSealedError = 1069;
    public const int kCallNotFoundError = 1070;
    public const int kAlreadyBoundError = 1071;
    public const int kZeroDispIdError = 1072;
    public const int kDuplicateDispIdError = 1073;
    public const int kConstWriteError = 1074;
    public const int kMathNotFunctionError = 1075;
    public const int kMathNotConstructorError = 1076;
    public const int kWriteOnlyError = 1077;
    public const int kIllegalOpMultinameError = 1078;
    public const int kIllegalNativeMethodError = 1079;
    public const int kIllegalNamespaceError = 1080;
    public const int kReadSealedErrorNs = 1081;
    public const int kNoDefaultNamespaceError = 1082;
    public const int kXMLPrefixNotBound = 1083;
    public const int kXMLBadQName = 1084;
    public const int kXMLUnterminatedElementTag = 1085;
    public const int kXMLOnlyWorksWithOneItemLists = 1086;
    public const int kXMLAssignmentToIndexedXMLNotAllowed = 1087;
    public const int kXMLMarkupMustBeWellFormed = 1088;
    public const int kXMLAssigmentOneItemLists = 1089;
    public const int kXMLMalformedElement = 1090;
    public const int kXMLUnterminatedCData = 1091;
    public const int kXMLUnterminatedXMLDecl = 1092;
    public const int kXMLUnterminatedDocTypeDecl = 1093;
    public const int kXMLUnterminatedComment = 1094;
    public const int kXMLUnterminatedAttribute = 1095;
    public const int kXMLUnterminatedElement = 1096;
    public const int kXMLUnterminatedProcessingInstruction = 1097;
    public const int kXMLNamespaceWithPrefixAndNoURI = 1098;
    public const int kRegExpFlagsArgumentError = 1100;
    public const int kNoScopeError = 1101;
    public const int kIllegalDefaultValue = 1102;
    public const int kCannotExtendFinalClass = 1103;
    public const int kXMLDuplicateAttribute = 1104;
    public const int kCorruptABCError = 1107;
    public const int kInvalidBaseClassError = 1108;
    public const int kDanglingFunctionError = 1109;
    public const int kCannotExtendError = 1110;
    public const int kCannotImplementError = 1111;
    public const int kCoerceArgumentCountError = 1112;
    public const int kInvalidNewActivationError = 1113;
    public const int kNoGlobalScopeError = 1114;
    public const int kNotConstructorError = 1115;
    public const int kApplyError = 1116;
    public const int kXMLInvalidName = 1117;
    public const int kXMLIllegalCyclicalLoop = 1118;
    public const int kDeleteTypeError = 1119;
    public const int kDeleteSealedError = 1120;
    public const int kDuplicateMethodBodyError = 1121;
    public const int kIllegalInterfaceMethodBodyError = 1122;
    public const int kFilterError = 1123;
    public const int kInvalidHasNextError = 1124;
    public const int kFileOpenError = 1500;
    public const int kFileWriteError = 1501;
    public const int kScriptTimeoutError = 1502;
    public const int kScriptTerminatedError = 1503;
    public const int kEndOfFileError = 1504;
    public const int kStringIndexOutOfBoundsError = 1505;
    public const int kInvalidRangeError = 1506;
    public const int kNullArgumentError = 1507;
    public const int kInvalidArgumentError = 1508;
    public const int kShellCompressedDataError = 1509;
    public const int kArrayFilterNonNullObjectError = 1510;
    #endregion

    #region Flash Error Codes (from 2000)
    public const int kFileIOError = 2038;
    #endregion

    #region ExceptionFromErrorID, ExceptionFromError
    public static Exception ExceptionFromErrorID(int errid)
    {
        switch (errid)
        {
            case kConvertNullToObjectError:
            case kConvertUndefinedToObjectError:
                return new NullReferenceException();

            case kCheckTypeFailedError:
                return new InvalidCastException();
            
            case kNotImplementedError:
                return new NotImplementedException();

            case kInvalidRangeError:
            case kNumberOutOfRangeError:
                return new IndexOutOfRangeException();

            case kStackOverflowError:
                return new StackOverflowException();

            case kOutOfMemoryError:
                return new OutOfMemoryException();

            case kIllegalOpcodeError:
            case kIllegalOperandTypeError:
            case kIllegalOpMultinameError:
            case kInvalidBranchTargetError:
            case kStackUnderflowError:
            case kScopeDepthUnbalancedError:
            case kScopeStackOverflowError:
            case kScopeStackUnderflowError:
            case kCpoolEntryWrongTypeError:
            case kCpoolIndexRangeError:
            case kInvalidMagicError:
            case kInvalidCodeLengthError:
            case kIllegalOverrideError:
            case kIllegalNativeMethodBodyError:
            case kDuplicateDispIdError:
                return new ExecutionEngineException();

            case kScriptTerminatedError:
            case kScriptTimeoutError:
                return new SystemException();

            case kFileOpenError:
            case kFileWriteError:
            case kFileIOError:
                return new IOException();

            case kArrayIndexNotIntegerError:
                return new OverflowException();
        }

        return new SystemException();
    }

    public static Exception ExceptionFromError(Avm.Error err)
    {
        Exception exc;
        if (err is Avm.RangeError)
            exc = new OverflowException();
        else
            exc = ExceptionFromErrorID(err.errorID);

        exc.avm_setMessage((string)err.message);

        if (flash.system.Capabilities.isDebugger)
            exc.stack_trace = err.getStackTrace();

        return exc;
    }
    #endregion
}