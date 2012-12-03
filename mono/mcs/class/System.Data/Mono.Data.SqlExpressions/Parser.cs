// created by jay 0.7 (c) 1998 Axel.Schreiner@informatik.uni-osnabrueck.de

#line 2 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
//
// Parser.jay
//
// Author:
//   Juraj Skripsky (juraj@hotfeet.ch)
//
// (C) 2004 HotFeet GmbH (http://www.hotfeet.ch)
//

using System;
using System.Collections;
using System.Data;

namespace Mono.Data.SqlExpressions {

	internal class Parser {
		static Parser ()
		{
			if (Environment.GetEnvironmentVariable ("MONO_DEBUG_SQLEXPRESSIONS") != null)
				yacc_verbose_flag = 2;
		}

		bool cacheAggregationResults = false;
		DataRow[] aggregationRows = null;
		static int yacc_verbose_flag;
		
		//called by DataTable.Select
		//called by DataColumn.set_Expression //FIXME: enable cache in this case?
		public Parser () {
			ErrorOutput = System.IO.TextWriter.Null;
			cacheAggregationResults = true;
		}
		
		//called by DataTable.Compute
		public Parser (DataRow[] aggregationRows)
		{
			ErrorOutput = System.IO.TextWriter.Null;
			this.aggregationRows = aggregationRows;
		}
		
		public IExpression Compile (string sqlExpr)
		{
			try {
				Tokenizer tokenizer = new Tokenizer (sqlExpr);
				if (yacc_verbose_flag > 1)
					return (IExpression) yyparse (tokenizer,
						new yydebug.yyDebugSimple ());
				else
					return (IExpression) yyparse (tokenizer);
			} catch (yyParser.yyException e) {
				throw new SyntaxErrorException (String.Format ("Expression '{0}' is invalid.", sqlExpr));
			}
		}
#line default

  /** error output stream.
      It should be changeable.
    */
  public System.IO.TextWriter ErrorOutput = System.Console.Out;

  /** simplified error message.
      @see <a href="#yyerror(java.lang.String, java.lang.String[])">yyerror</a>
    */
  public void yyerror (string message) {
    yyerror(message, null);
  }

  /** (syntax) error message.
      Can be overwritten to control message format.
      @param message text to be displayed.
      @param expected vector of acceptable tokens, if available.
    */
  public void yyerror (string message, string[] expected) {
    if ((yacc_verbose_flag > 0) && (expected != null) && (expected.Length  > 0)) {
      ErrorOutput.Write (message+", expecting");
      for (int n = 0; n < expected.Length; ++ n)
        ErrorOutput.Write (" "+expected[n]);
        ErrorOutput.WriteLine ();
    } else
      ErrorOutput.WriteLine (message);
  }

  /** debugging support, requires the package jay.yydebug.
      Set to null to suppress debugging messages.
    */
  internal yydebug.yyDebug debug;

  protected static  int yyFinal = 24;
 // Put this array into a separate class so it is only initialized if debugging is actually used
 // Use MarshalByRefObject to disable inlining
 class YYRules
#if NOT_PFX
: MarshalByRefObject
#endif
 {
  public static  string [] yyRule = {
    "$accept : Expr",
    "Expr : BoolExpr",
    "Expr : ArithExpr",
    "BoolExpr : PAROPEN BoolExpr PARCLOSE",
    "BoolExpr : BoolExpr AND BoolExpr",
    "BoolExpr : BoolExpr OR BoolExpr",
    "BoolExpr : NOT BoolExpr",
    "BoolExpr : Predicate",
    "Predicate : CompPredicate",
    "Predicate : IsPredicate",
    "Predicate : LikePredicate",
    "Predicate : InPredicate",
    "CompPredicate : ArithExpr CompOp ArithExpr",
    "CompOp : EQ",
    "CompOp : NE",
    "CompOp : LT",
    "CompOp : GT",
    "CompOp : LE",
    "CompOp : GE",
    "LE : LT EQ",
    "NE : LT GT",
    "GE : GT EQ",
    "ArithExpr : PAROPEN ArithExpr PARCLOSE",
    "ArithExpr : ArithExpr MUL ArithExpr",
    "ArithExpr : ArithExpr DIV ArithExpr",
    "ArithExpr : ArithExpr MOD ArithExpr",
    "ArithExpr : ArithExpr PLUS ArithExpr",
    "ArithExpr : ArithExpr MINUS ArithExpr",
    "ArithExpr : MINUS ArithExpr",
    "ArithExpr : Function",
    "ArithExpr : Value",
    "Value : LiteralValue",
    "Value : SingleColumnValue",
    "LiteralValue : StringLiteral",
    "LiteralValue : NumberLiteral",
    "LiteralValue : DateLiteral",
    "LiteralValue : BoolLiteral",
    "BoolLiteral : TRUE",
    "BoolLiteral : FALSE",
    "SingleColumnValue : LocalColumnValue",
    "SingleColumnValue : ParentColumnValue",
    "MultiColumnValue : LocalColumnValue",
    "MultiColumnValue : ChildColumnValue",
    "LocalColumnValue : ColumnName",
    "ParentColumnValue : PARENT DOT ColumnName",
    "ParentColumnValue : PARENT PAROPEN RelationName PARCLOSE DOT ColumnName",
    "ChildColumnValue : CHILD DOT ColumnName",
    "ChildColumnValue : CHILD PAROPEN RelationName PARCLOSE DOT ColumnName",
    "ColumnName : Identifier",
    "ColumnName : ColumnName DOT Identifier",
    "RelationName : Identifier",
    "Function : CalcFunction",
    "Function : AggFunction",
    "Function : StringFunction",
    "AggFunction : AggFunctionName PAROPEN MultiColumnValue PARCLOSE",
    "AggFunctionName : COUNT",
    "AggFunctionName : SUM",
    "AggFunctionName : AVG",
    "AggFunctionName : MAX",
    "AggFunctionName : MIN",
    "AggFunctionName : STDEV",
    "AggFunctionName : VAR",
    "StringExpr : PAROPEN StringExpr PARCLOSE",
    "StringExpr : SingleColumnValue",
    "StringExpr : StringLiteral",
    "StringExpr : StringFunction",
    "StringFunction : TRIM PAROPEN StringExpr PARCLOSE",
    "StringFunction : SUBSTRING PAROPEN StringExpr COMMA ArithExpr COMMA ArithExpr PARCLOSE",
    "StringFunction : StringExpr PLUS StringExpr",
    "CalcFunction : IIF PAROPEN Expr COMMA Expr COMMA Expr PARCLOSE",
    "CalcFunction : ISNULL PAROPEN Expr COMMA Expr PARCLOSE",
    "CalcFunction : LEN PAROPEN Expr PARCLOSE",
    "CalcFunction : CONVERT PAROPEN Expr COMMA TypeSpecifier PARCLOSE",
    "TypeSpecifier : StringLiteral",
    "TypeSpecifier : Identifier",
    "IsPredicate : ArithExpr IS NULL",
    "IsPredicate : ArithExpr IS NOT NULL",
    "LikePredicate : StringExpr LIKE StringExpr",
    "LikePredicate : StringExpr NOT LIKE StringExpr",
    "InPredicate : ArithExpr IN InPredicateValue",
    "InPredicate : ArithExpr NOT IN InPredicateValue",
    "InPredicateValue : PAROPEN InValueList PARCLOSE",
    "InValueList : LiteralValue",
    "InValueList : InValueList COMMA LiteralValue",
  };
 public static string getRule (int index) {
    return yyRule [index];
 }
}
  protected static  string [] yyNames = {    
    "end-of-file",null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,null,null,null,null,null,null,null,
    null,null,null,null,null,null,null,"PAROPEN","PARCLOSE","AND","OR",
    "NOT","TRUE","FALSE","NULL","PARENT","CHILD","EQ","LT","GT","PLUS",
    "MINUS","MUL","DIV","MOD","DOT","COMMA","IS","IN","LIKE","COUNT",
    "SUM","AVG","MAX","MIN","STDEV","VAR","IIF","SUBSTRING","ISNULL",
    "LEN","TRIM","CONVERT","StringLiteral","NumberLiteral","DateLiteral",
    "Identifier","FunctionName","UMINUS",
  };

  /** index-checked interface to yyNames[].
      @param token single character or %token value.
      @return token name or [illegal] or [unknown].
    */
  public static string yyname (int token) {
    if ((token < 0) || (token > yyNames.Length)) return "[illegal]";
    string name;
    if ((name = yyNames[token]) != null) return name;
    return "[unknown]";
  }

  /** computes list of expected tokens on error by tracing the tables.
      @param state for which to compute the list.
      @return list of token names.
    */
  protected string[] yyExpecting (int state) {
    int token, n, len = 0;
    bool[] ok = new bool[yyNames.Length];

    if ((n = yySindex[state]) != 0)
      for (token = n < 0 ? -n : 0;
           (token < yyNames.Length) && (n+token < yyTable.Length); ++ token)
        if (yyCheck[n+token] == token && !ok[token] && yyNames[token] != null) {
          ++ len;
          ok[token] = true;
        }
    if ((n = yyRindex[state]) != 0)
      for (token = n < 0 ? -n : 0;
           (token < yyNames.Length) && (n+token < yyTable.Length); ++ token)
        if (yyCheck[n+token] == token && !ok[token] && yyNames[token] != null) {
          ++ len;
          ok[token] = true;
        }

    string [] result = new string[len];
    for (n = token = 0; n < len;  ++ token)
      if (ok[token]) result[n++] = yyNames[token];
    return result;
  }

  /** the generated parser, with debugging messages.
      Maintains a state and a value stack, currently with fixed maximum size.
      @param yyLex scanner.
      @param yydebug debug message writer implementing yyDebug, or null.
      @return result of the last reduction, if any.
      @throws yyException on irrecoverable parse error.
    */
  internal Object yyparse (yyParser.yyInput yyLex, Object yyd)
				 {
    this.debug = (yydebug.yyDebug)yyd;
    return yyparse(yyLex);
  }

  /** initial size and increment of the state/value stack [default 256].
      This is not final so that it can be overwritten outside of invocations
      of yyparse().
    */
  protected int yyMax;

  /** executed at the beginning of a reduce action.
      Used as $$ = yyDefault($1), prior to the user-specified action, if any.
      Can be overwritten to provide deep copy, etc.
      @param first value for $1, or null.
      @return first.
    */
  protected Object yyDefault (Object first) {
    return first;
  }

  /** the generated parser.
      Maintains a state and a value stack, currently with fixed maximum size.
      @param yyLex scanner.
      @return result of the last reduction, if any.
      @throws yyException on irrecoverable parse error.
    */
  internal Object yyparse (yyParser.yyInput yyLex)
  {
    if (yyMax <= 0) yyMax = 256;			// initial size
    int yyState = 0;                                   // state stack ptr
    int [] yyStates = new int[yyMax];	                // state stack 
    Object yyVal = null;                               // value stack ptr
    Object [] yyVals = new Object[yyMax];	        // value stack
    int yyToken = -1;					// current input
    int yyErrorFlag = 0;				// #tks to shift

    /*yyLoop:*/ for (int yyTop = 0;; ++ yyTop) {
      if (yyTop >= yyStates.Length) {			// dynamically increase
        int[] i = new int[yyStates.Length+yyMax];
        yyStates.CopyTo (i, 0);
        yyStates = i;
        Object[] o = new Object[yyVals.Length+yyMax];
        yyVals.CopyTo (o, 0);
        yyVals = o;
      }
      yyStates[yyTop] = yyState;
      yyVals[yyTop] = yyVal;
      if (debug != null) debug.push(yyState, yyVal);

      /*yyDiscarded:*/ for (;;) {	// discarding a token does not change stack
        int yyN;
        if ((yyN = yyDefRed[yyState]) == 0) {	// else [default] reduce (yyN)
          if (yyToken < 0) {
            yyToken = yyLex.advance() ? yyLex.token() : 0;
            if (debug != null)
              debug.lex(yyState, yyToken, yyname(yyToken), yyLex.value());
          }
          if ((yyN = yySindex[yyState]) != 0 && ((yyN += yyToken) >= 0)
              && (yyN < yyTable.Length) && (yyCheck[yyN] == yyToken)) {
            if (debug != null)
              debug.shift(yyState, yyTable[yyN], yyErrorFlag-1);
            yyState = yyTable[yyN];		// shift to yyN
            yyVal = yyLex.value();
            yyToken = -1;
            if (yyErrorFlag > 0) -- yyErrorFlag;
            goto continue_yyLoop;
          }
          if ((yyN = yyRindex[yyState]) != 0 && (yyN += yyToken) >= 0
              && yyN < yyTable.Length && yyCheck[yyN] == yyToken)
            yyN = yyTable[yyN];			// reduce (yyN)
          else
            switch (yyErrorFlag) {
  
            case 0:
              // yyerror(String.Format ("syntax error, got token `{0}'", yyname (yyToken)), yyExpecting(yyState));
              if (debug != null) debug.error("syntax error");
              goto case 1;
            case 1: case 2:
              yyErrorFlag = 3;
              do {
                if ((yyN = yySindex[yyStates[yyTop]]) != 0
                    && (yyN += Token.yyErrorCode) >= 0 && yyN < yyTable.Length
                    && yyCheck[yyN] == Token.yyErrorCode) {
                  if (debug != null)
                    debug.shift(yyStates[yyTop], yyTable[yyN], 3);
                  yyState = yyTable[yyN];
                  yyVal = yyLex.value();
                  goto continue_yyLoop;
                }
                if (debug != null) debug.pop(yyStates[yyTop]);
              } while (-- yyTop >= 0);
              if (debug != null) debug.reject();
              throw new yyParser.yyException("irrecoverable syntax error");
  
            case 3:
              if (yyToken == 0) {
                if (debug != null) debug.reject();
                throw new yyParser.yyException("irrecoverable syntax error at end-of-file");
              }
              if (debug != null)
                debug.discard(yyState, yyToken, yyname(yyToken),
  							yyLex.value());
              yyToken = -1;
              goto continue_yyDiscarded;		// leave stack alone
            }
        }
        int yyV = yyTop + 1-yyLen[yyN];
        if (debug != null)
          debug.reduce(yyState, yyStates[yyV-1], yyN, YYRules.getRule (yyN), yyLen[yyN]);
        yyVal = yyDefault(yyV > yyTop ? null : yyVals[yyV]);
        switch (yyN) {
case 3:
#line 105 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = (IExpression)yyVals[-1+yyTop];
	}
  break;
case 4:
#line 109 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = new BoolOperation (Operation.AND, (IExpression)yyVals[-2+yyTop], (IExpression)yyVals[0+yyTop]);
	}
  break;
case 5:
#line 113 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = new BoolOperation (Operation.OR, (IExpression)yyVals[-2+yyTop], (IExpression)yyVals[0+yyTop]);
	}
  break;
case 6:
#line 117 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = new Negation ((IExpression)yyVals[0+yyTop]);
	}
  break;
case 12:
#line 132 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = new Comparison ((Operation)yyVals[-1+yyTop], (IExpression)yyVals[-2+yyTop], (IExpression)yyVals[0+yyTop]);
	}
  break;
case 13:
#line 138 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  { yyVal = Operation.EQ; }
  break;
case 14:
#line 139 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  { yyVal = Operation.NE; }
  break;
case 15:
#line 140 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  { yyVal = Operation.LT; }
  break;
case 16:
#line 141 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  { yyVal = Operation.GT; }
  break;
case 17:
#line 142 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  { yyVal = Operation.LE; }
  break;
case 18:
#line 143 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  { yyVal = Operation.GE; }
  break;
case 22:
#line 152 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = (IExpression)yyVals[-1+yyTop];
	}
  break;
case 23:
#line 156 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = new ArithmeticOperation (Operation.MUL, (IExpression)yyVals[-2+yyTop], (IExpression)yyVals[0+yyTop]);
	}
  break;
case 24:
#line 160 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = new ArithmeticOperation (Operation.DIV, (IExpression)yyVals[-2+yyTop], (IExpression)yyVals[0+yyTop]);
	}
  break;
case 25:
#line 164 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = new ArithmeticOperation (Operation.MOD, (IExpression)yyVals[-2+yyTop], (IExpression)yyVals[0+yyTop]);
	}
  break;
case 26:
#line 168 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = new ArithmeticOperation (Operation.ADD, (IExpression)yyVals[-2+yyTop], (IExpression)yyVals[0+yyTop]);
	}
  break;
case 27:
#line 172 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = new ArithmeticOperation (Operation.SUB, (IExpression)yyVals[-2+yyTop], (IExpression)yyVals[0+yyTop]);
	}
  break;
case 28:
#line 176 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = new Negative ((IExpression)yyVals[0+yyTop]);
	}
  break;
case 33:
#line 189 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  { yyVal = new Literal (yyVals[0+yyTop]); }
  break;
case 34:
#line 190 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  { yyVal = new Literal (yyVals[0+yyTop]); }
  break;
case 35:
#line 191 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  { yyVal = new Literal (yyVals[0+yyTop]); }
  break;
case 37:
#line 196 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  { yyVal = new Literal (true); }
  break;
case 38:
#line 197 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  { yyVal = new Literal (false); }
  break;
case 43:
#line 212 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = new ColumnReference ((string)yyVals[0+yyTop]);
	}
  break;
case 44:
#line 219 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = new ColumnReference (ReferencedTable.Parent, null, (string)yyVals[0+yyTop]);
	}
  break;
case 45:
#line 223 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = new ColumnReference (ReferencedTable.Parent, (string)yyVals[-3+yyTop], (string)yyVals[0+yyTop]);
	}
  break;
case 46:
#line 230 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = new ColumnReference (ReferencedTable.Child, null, (string)yyVals[0+yyTop]);
	}
  break;
case 47:
#line 234 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = new ColumnReference (ReferencedTable.Child, (string)yyVals[-3+yyTop], (string)yyVals[0+yyTop]);
	}
  break;
case 49:
#line 242 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = (string)yyVals[-2+yyTop] + "." + (string)yyVals[0+yyTop];
	}
  break;
case 54:
#line 258 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = new Aggregation (cacheAggregationResults, aggregationRows, (AggregationFunction)yyVals[-3+yyTop], (ColumnReference)yyVals[-1+yyTop]);
	}
  break;
case 55:
#line 264 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  { yyVal = AggregationFunction.Count; }
  break;
case 56:
#line 265 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  { yyVal = AggregationFunction.Sum; }
  break;
case 57:
#line 266 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  { yyVal = AggregationFunction.Avg; }
  break;
case 58:
#line 267 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  { yyVal = AggregationFunction.Max; }
  break;
case 59:
#line 268 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  { yyVal = AggregationFunction.Min; }
  break;
case 60:
#line 269 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  { yyVal = AggregationFunction.StDev; }
  break;
case 61:
#line 270 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  { yyVal = AggregationFunction.Var; }
  break;
case 62:
#line 275 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = (IExpression)yyVals[-1+yyTop];
	}
  break;
case 64:
#line 279 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  { yyVal = new Literal (yyVals[0+yyTop]); }
  break;
case 66:
#line 285 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = new TrimFunction ((IExpression)yyVals[-1+yyTop]);
	}
  break;
case 67:
#line 289 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = new SubstringFunction ((IExpression)yyVals[-5+yyTop], (IExpression)yyVals[-3+yyTop], (IExpression)yyVals[-1+yyTop]);
	}
  break;
case 68:
#line 293 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = new ConcatFunction ((IExpression)yyVals[-2+yyTop], (IExpression)yyVals[0+yyTop]);
	}
  break;
case 69:
#line 300 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = new IifFunction ((IExpression)yyVals[-5+yyTop], (IExpression)yyVals[-3+yyTop], (IExpression)yyVals[-1+yyTop]);
	}
  break;
case 70:
#line 304 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = new IsNullFunction ((IExpression)yyVals[-3+yyTop], (IExpression)yyVals[-1+yyTop]);
	}
  break;
case 71:
#line 308 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = new LenFunction ((IExpression)yyVals[-1+yyTop]);
	}
  break;
case 72:
#line 312 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = new ConvertFunction ((IExpression)yyVals[-3+yyTop], (string)yyVals[-1+yyTop]);
	}
  break;
case 75:
#line 324 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = new Comparison (Operation.EQ, (IExpression)yyVals[-2+yyTop], new Literal (null));
	}
  break;
case 76:
#line 328 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = new Comparison (Operation.NE, (IExpression)yyVals[-3+yyTop], new Literal (null));
	}
  break;
case 77:
#line 335 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = new Like ((IExpression)yyVals[-2+yyTop], (IExpression)yyVals[0+yyTop]);
	}
  break;
case 78:
#line 339 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = new Negation (new Like ((IExpression)yyVals[-3+yyTop], (IExpression)yyVals[0+yyTop]));
	}
  break;
case 79:
#line 346 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = new In ((IExpression)yyVals[-2+yyTop], (IList)yyVals[0+yyTop]);
	}
  break;
case 80:
#line 350 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = new Negation (new In ((IExpression)yyVals[-3+yyTop], (IList)yyVals[0+yyTop]));
	}
  break;
case 81:
#line 356 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  { yyVal = yyVals[-1+yyTop]; }
  break;
case 82:
#line 361 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		yyVal = new ArrayList();
		((IList)yyVal).Add (yyVals[0+yyTop]);
	}
  break;
case 83:
#line 366 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
  {
		((IList)(yyVal = yyVals[-2+yyTop])).Add (yyVals[0+yyTop]);
	}
  break;
#line default
        }
        yyTop -= yyLen[yyN];
        yyState = yyStates[yyTop];
        int yyM = yyLhs[yyN];
        if (yyState == 0 && yyM == 0) {
          if (debug != null) debug.shift(0, yyFinal);
          yyState = yyFinal;
          if (yyToken < 0) {
            yyToken = yyLex.advance() ? yyLex.token() : 0;
            if (debug != null)
               debug.lex(yyState, yyToken,yyname(yyToken), yyLex.value());
          }
          if (yyToken == 0) {
            if (debug != null) debug.accept(yyVal);
            return yyVal;
          }
          goto continue_yyLoop;
        }
        if (((yyN = yyGindex[yyM]) != 0) && ((yyN += yyState) >= 0)
            && (yyN < yyTable.Length) && (yyCheck[yyN] == yyState))
          yyState = yyTable[yyN];
        else
          yyState = yyDgoto[yyM];
        if (debug != null) debug.shift(yyStates[yyTop], yyState);
	 goto continue_yyLoop;
      continue_yyDiscarded: continue;	// implements the named-loop continue: 'continue yyDiscarded'
      }
    continue_yyLoop: continue;		// implements the named-loop continue: 'continue yyLoop'
    }
  }

   static  short [] yyLhs  = {              -1,
    0,    0,    1,    1,    1,    1,    1,    3,    3,    3,
    3,    4,    8,    8,    8,    8,    8,    8,   10,    9,
   11,    2,    2,    2,    2,    2,    2,    2,    2,    2,
   13,   13,   14,   14,   14,   14,   16,   16,   15,   15,
   19,   19,   17,   18,   18,   20,   20,   21,   21,   22,
   12,   12,   12,   24,   26,   26,   26,   26,   26,   26,
   26,   27,   27,   27,   27,   25,   25,   25,   23,   23,
   23,   23,   28,   28,    5,    5,    6,    6,    7,    7,
   29,   30,   30,
  };
   static  short [] yyLen = {           2,
    1,    1,    3,    3,    3,    2,    1,    1,    1,    1,
    1,    3,    1,    1,    1,    1,    1,    1,    2,    2,
    2,    3,    3,    3,    3,    3,    3,    2,    1,    1,
    1,    1,    1,    1,    1,    1,    1,    1,    1,    1,
    1,    1,    1,    3,    6,    3,    6,    1,    3,    1,
    1,    1,    1,    4,    1,    1,    1,    1,    1,    1,
    1,    3,    1,    1,    1,    4,    8,    3,    8,    6,
    4,    6,    1,    1,    3,    4,    3,    4,    3,    4,
    3,    1,    3,
  };
   static  short [] yyDefRed = {            0,
    0,    0,   37,   38,    0,    0,   55,   56,   57,   58,
   59,   60,   61,    0,    0,    0,    0,    0,    0,    0,
   34,   35,   48,    0,    0,    0,    7,    8,    9,   10,
   11,   29,   30,   31,    0,   36,   39,   40,    0,   51,
   52,    0,    0,    0,    0,    0,    0,    6,    0,    0,
    0,    0,   28,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,   13,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,   14,   17,   18,    0,    0,    0,
    0,    0,    3,   22,   62,   50,    0,    0,    0,    0,
    0,    0,   64,   63,   65,    0,    0,    0,    0,    0,
    4,    0,    0,   19,   20,   21,    0,    0,   23,   24,
   25,    0,   75,    0,   79,    0,   49,    0,   41,    0,
   42,    0,   68,    0,    0,    0,    0,    0,   71,   66,
    0,   80,   76,   33,   82,    0,    0,    0,   54,    0,
    0,    0,    0,    0,   73,   74,    0,   81,    0,    0,
    0,    0,    0,    0,   70,   72,   83,    0,    0,    0,
    0,   69,   67,    0,
  };
  protected static  short [] yyDgoto  = {            24,
   25,   26,   27,   28,   29,   30,   31,   74,   75,   76,
   77,   32,   33,   34,   35,   36,   37,   38,  120,  121,
   39,   87,   40,   41,   42,   43,   44,  147,  115,  136,
  };
  protected static  short [] yySindex = {         -108,
 -108, -108,    0,    0, -250,  -67,    0,    0,    0,    0,
    0,    0,    0, -253, -243, -238, -228, -224, -217,    0,
    0,    0,    0,    0, -169,  -31,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0, -219,    0,
    0,    0, -182, -215, -128, -151, -211,    0,  -31, -197,
 -188,  -67,    0, -193, -108, -231, -108, -108, -231, -108,
 -108, -108, -164,    0, -121, -142,  -67,  -67,  -67,  -67,
  -67, -213, -172,  -67,    0,    0,    0, -157, -264, -141,
 -231, -231,    0,    0,    0,    0, -113, -219, -130, -192,
 -126, -231,    0,    0,    0, -196, -125, -106, -158, -118,
    0, -100, -172,    0,    0,    0, -138, -138,    0,    0,
    0, -104,    0, -251,    0,  -71,    0, -247,    0,  -94,
    0, -231,    0, -193, -110, -108,  -67, -108,    0,    0,
 -195,    0,    0,    0,    0, -252, -197, -188,    0, -193,
 -188, -109,  -22,  -92,    0,    0,  -89,    0, -251,  -88,
 -219, -219, -108,  -67,    0,    0,    0,  -84,  -66,  -65,
 -188,    0,    0, -219,
  };
  protected static  short [] yyRindex = {            0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    1,
    0,    0,    0,    0,    9,   31,    0,    0,    0,    0,
    0,    0,    0,    0,   23,    0,    0,    0,   45,    0,
    0,   67,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,  177,  217,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,   89,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
    0,  171,    0,    0,    0,    0,  133,  147,    0,    0,
    0,    0,    0,    0,    0,  137,    0,    0,    0,    0,
    0,    0,    0,  161,    0,    0,    0,    0,    0,    0,
    0,    0,    0,    0,    0,    0,    0,    0,    0,  168,
    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,
  -64,  111,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0,  -61,
  };
  protected static  short [] yyGindex = {          -40,
   20,   35,    0,    0,    0,    0,    0,    0,    0,    0,
    0,    0,    0, -114,  -43,    0,  131,    0,    0,    0,
  -46,   74,    0,    0,  -29,    0,    2,    0,  109,    0,
  };
  protected static  short [] yyTable = {           135,
   33,  118,   47,   55,   88,  148,   50,   54,    1,  137,
    3,    4,   94,   56,   91,   94,   97,   98,   57,  100,
   45,   48,   32,  149,   51,   92,   95,  138,   58,   95,
    2,   23,   59,    5,  157,   46,   49,   94,   94,   60,
   53,  134,   21,   22,   43,   80,   85,  112,   94,   80,
  113,   95,   95,   90,   81,   78,   15,   96,   81,   18,
   99,   93,   95,   82,   23,   85,   53,   82,   54,   54,
   54,   54,   54,   81,   79,   54,   81,   81,   94,  127,
  101,  102,  123,  124,  114,  142,   89,  144,   44,   61,
   62,  151,   95,   90,  152,   49,   49,  145,   86,  130,
  146,  107,  108,  109,  110,  111,   84,   23,  116,   63,
   45,   81,  159,  103,  164,   64,   65,   66,   67,   68,
   69,   70,   71,  140,  106,   72,   73,   84,   54,   83,
   61,   62,   26,   69,   70,   71,   12,  122,  117,   67,
   68,   69,   70,   71,  125,  104,   27,  105,    1,  126,
  128,  129,    2,    3,    4,   54,    5,  131,   61,  133,
   77,  143,    6,  139,  141,  155,  153,   78,  156,  158,
    5,    7,    8,    9,   10,   11,   12,   13,   14,   15,
   16,   17,   18,   19,   20,   21,   22,   23,  160,   52,
  161,  162,  163,   46,    3,    4,   47,    5,   67,   68,
   69,   70,   71,    6,   67,   68,   69,   70,   71,  119,
  150,  132,    7,    8,    9,   10,   11,   12,   13,   14,
   15,   16,   17,   18,   19,   20,   21,   22,   23,   63,
    0,    0,    0,    0,    0,   64,   65,   66,   67,   68,
   69,   70,   71,    0,    0,   72,   73,   67,   68,   69,
   70,   71,    0,  154,    0,    0,    0,    0,   33,   33,
   33,   33,    0,    0,    0,    0,    1,   33,   33,   33,
   33,   33,   33,   33,   33,    0,   33,   33,   33,   64,
   32,   32,   32,   32,    1,    0,    0,    0,    2,   32,
   32,   32,   32,   32,   32,   32,   32,    0,   32,   32,
   32,   63,   43,   43,   43,   43,    2,    0,    0,    0,
    0,   43,   43,   43,   43,   43,   43,   43,   43,    0,
   43,   43,   43,   43,   53,   53,   53,   53,    0,    0,
    0,    0,    0,   53,   53,   53,   53,   53,   53,   53,
   53,    0,   53,   53,   53,   65,   44,   44,   44,   44,
    0,    0,    0,    0,    0,   44,   44,   44,   44,   44,
   44,   44,   44,    0,   44,   44,   44,   44,   45,   45,
   45,   45,    0,    0,    0,    0,    0,   45,   45,   45,
   45,   45,   45,   45,   45,    0,   45,   45,   45,   45,
   26,   26,   26,   26,   12,   12,   12,    0,    0,   26,
   26,   26,   26,   26,   27,   27,   27,   27,   26,   26,
   26,    0,   12,   27,   27,   27,   27,   27,   77,   77,
   77,    0,   27,   27,   27,   78,   78,   78,    5,    0,
    5,    0,    0,   15,    0,    0,   77,    0,   15,   15,
    0,   15,    0,   78,    0,    0,    5,   15,    0,    0,
    0,    0,    0,    0,    0,    0,   15,   15,   15,   15,
   15,   15,   15,   15,   15,   15,   15,   15,   15,   15,
   15,   15,   15,   16,    0,    0,    0,    0,   16,   16,
    0,   16,    0,    0,    0,    0,    0,   16,    0,    0,
    0,    0,    0,    0,    0,    0,   16,   16,   16,   16,
   16,   16,   16,   16,   16,   16,   16,   16,   16,   16,
   16,   16,   16,
  };
  protected static  short [] yyCheck = {           114,
    0,  266,    1,  257,   51,  258,  257,    6,    0,  257,
  262,  263,   56,  257,   55,   59,   57,   58,  257,   60,
    1,    2,    0,  276,  275,  257,   56,  275,  257,   59,
    0,  296,  257,  265,  149,    1,    2,   81,   82,  257,
    6,  293,  294,  295,    0,  261,  258,  261,   92,  261,
  264,   81,   82,   52,  270,  275,  288,   56,  270,  291,
   59,  293,   92,  279,  296,  258,    0,  279,   67,   68,
   69,   70,   71,  270,  257,   74,  270,  270,  122,  276,
   61,   62,   81,   82,  257,  126,   52,  128,    0,  259,
  260,  138,  122,   92,  141,   61,   62,  293,  296,  258,
  296,   67,   68,   69,   70,   71,  258,  296,   74,  261,
    0,  270,  153,  278,  161,  267,  268,  269,  270,  271,
  272,  273,  274,  122,  267,  277,  278,  258,  127,  258,
  259,  260,    0,  272,  273,  274,    0,  279,  296,  270,
  271,  272,  273,  274,  258,  267,    0,  269,  257,  276,
  276,  258,  261,  262,  263,  154,  265,  276,  259,  264,
    0,  127,  271,  258,  275,  258,  276,    0,  258,  258,
    0,  280,  281,  282,  283,  284,  285,  286,  287,  288,
  289,  290,  291,  292,  293,  294,  295,  296,  154,  257,
  275,  258,  258,  258,  262,  263,  258,  265,  270,  271,
  272,  273,  274,  271,  270,  271,  272,  273,  274,   79,
  137,  103,  280,  281,  282,  283,  284,  285,  286,  287,
  288,  289,  290,  291,  292,  293,  294,  295,  296,  261,
   -1,   -1,   -1,   -1,   -1,  267,  268,  269,  270,  271,
  272,  273,  274,   -1,   -1,  277,  278,  270,  271,  272,
  273,  274,   -1,  276,   -1,   -1,   -1,   -1,  258,  259,
  260,  261,   -1,   -1,   -1,   -1,  258,  267,  268,  269,
  270,  271,  272,  273,  274,   -1,  276,  277,  278,  279,
  258,  259,  260,  261,  276,   -1,   -1,   -1,  258,  267,
  268,  269,  270,  271,  272,  273,  274,   -1,  276,  277,
  278,  279,  258,  259,  260,  261,  276,   -1,   -1,   -1,
   -1,  267,  268,  269,  270,  271,  272,  273,  274,   -1,
  276,  277,  278,  279,  258,  259,  260,  261,   -1,   -1,
   -1,   -1,   -1,  267,  268,  269,  270,  271,  272,  273,
  274,   -1,  276,  277,  278,  279,  258,  259,  260,  261,
   -1,   -1,   -1,   -1,   -1,  267,  268,  269,  270,  271,
  272,  273,  274,   -1,  276,  277,  278,  279,  258,  259,
  260,  261,   -1,   -1,   -1,   -1,   -1,  267,  268,  269,
  270,  271,  272,  273,  274,   -1,  276,  277,  278,  279,
  258,  259,  260,  261,  258,  259,  260,   -1,   -1,  267,
  268,  269,  270,  271,  258,  259,  260,  261,  276,  277,
  278,   -1,  276,  267,  268,  269,  270,  271,  258,  259,
  260,   -1,  276,  277,  278,  258,  259,  260,  258,   -1,
  260,   -1,   -1,  257,   -1,   -1,  276,   -1,  262,  263,
   -1,  265,   -1,  276,   -1,   -1,  276,  271,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  280,  281,  282,  283,
  284,  285,  286,  287,  288,  289,  290,  291,  292,  293,
  294,  295,  296,  257,   -1,   -1,   -1,   -1,  262,  263,
   -1,  265,   -1,   -1,   -1,   -1,   -1,  271,   -1,   -1,
   -1,   -1,   -1,   -1,   -1,   -1,  280,  281,  282,  283,
  284,  285,  286,  287,  288,  289,  290,  291,  292,  293,
  294,  295,  296,
  };

#line 372 "E:\src\mono\svn\mcs\class\System.Data\Mono.Data.SqlExpressions\Parser.jay"
}
#line default
namespace yydebug {
        using System;
	 internal interface yyDebug {
		 void push (int state, Object value);
		 void lex (int state, int token, string name, Object value);
		 void shift (int from, int to, int errorFlag);
		 void pop (int state);
		 void discard (int state, int token, string name, Object value);
		 void reduce (int from, int to, int rule, string text, int len);
		 void shift (int from, int to);
		 void accept (Object value);
		 void error (string message);
		 void reject ();
	 }
	 
	 class yyDebugSimple : yyDebug {
		 void println (string s){
			 Console.Error.WriteLine (s);
		 }
		 
		 public void push (int state, Object value) {
			 println ("push\tstate "+state+"\tvalue "+value);
		 }
		 
		 public void lex (int state, int token, string name, Object value) {
			 println("lex\tstate "+state+"\treading "+name+"\tvalue "+value);
		 }
		 
		 public void shift (int from, int to, int errorFlag) {
			 switch (errorFlag) {
			 default:				// normally
				 println("shift\tfrom state "+from+" to "+to);
				 break;
			 case 0: case 1: case 2:		// in error recovery
				 println("shift\tfrom state "+from+" to "+to
					     +"\t"+errorFlag+" left to recover");
				 break;
			 case 3:				// normally
				 println("shift\tfrom state "+from+" to "+to+"\ton error");
				 break;
			 }
		 }
		 
		 public void pop (int state) {
			 println("pop\tstate "+state+"\ton error");
		 }
		 
		 public void discard (int state, int token, string name, Object value) {
			 println("discard\tstate "+state+"\ttoken "+name+"\tvalue "+value);
		 }
		 
		 public void reduce (int from, int to, int rule, string text, int len) {
			 println("reduce\tstate "+from+"\tuncover "+to
				     +"\trule ("+rule+") "+text);
		 }
		 
		 public void shift (int from, int to) {
			 println("goto\tfrom state "+from+" to "+to);
		 }
		 
		 public void accept (Object value) {
			 println("accept\tvalue "+value);
		 }
		 
		 public void error (string message) {
			 println("error\t"+message);
		 }
		 
		 public void reject () {
			 println("reject");
		 }
		 
	 }
}
// %token constants
 class Token {
  public const int PAROPEN = 257;
  public const int PARCLOSE = 258;
  public const int AND = 259;
  public const int OR = 260;
  public const int NOT = 261;
  public const int TRUE = 262;
  public const int FALSE = 263;
  public const int NULL = 264;
  public const int PARENT = 265;
  public const int CHILD = 266;
  public const int EQ = 267;
  public const int LT = 268;
  public const int GT = 269;
  public const int PLUS = 270;
  public const int MINUS = 271;
  public const int MUL = 272;
  public const int DIV = 273;
  public const int MOD = 274;
  public const int DOT = 275;
  public const int COMMA = 276;
  public const int IS = 277;
  public const int IN = 278;
  public const int LIKE = 279;
  public const int COUNT = 280;
  public const int SUM = 281;
  public const int AVG = 282;
  public const int MAX = 283;
  public const int MIN = 284;
  public const int STDEV = 285;
  public const int VAR = 286;
  public const int IIF = 287;
  public const int SUBSTRING = 288;
  public const int ISNULL = 289;
  public const int LEN = 290;
  public const int TRIM = 291;
  public const int CONVERT = 292;
  public const int StringLiteral = 293;
  public const int NumberLiteral = 294;
  public const int DateLiteral = 295;
  public const int Identifier = 296;
  public const int FunctionName = 297;
  public const int UMINUS = 298;
  public const int yyErrorCode = 256;
 }
 namespace yyParser {
  using System;
  /** thrown for irrecoverable syntax errors and stack overflow.
    */
  internal class yyException : System.Exception {
    public yyException (string message) : base (message) {
    }
  }

  /** must be implemented by a scanner object to supply input to the parser.
    */
  internal interface yyInput {
    /** move on to next token.
        @return false if positioned beyond tokens.
        @throws IOException on input error.
      */
    bool advance (); // throws java.io.IOException;
    /** classifies current token.
        Should not be called if advance() returned false.
        @return current %token or single character.
      */
    int token ();
    /** associated with current token.
        Should not be called if advance() returned false.
        @return value for token().
      */
    Object value ();
  }
 }
} // close outermost namespace, that MUST HAVE BEEN opened in the prolog
