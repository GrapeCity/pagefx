using System;

enum Category : ushort
{
    None,

    // canonical classes

    Any,			// any character except newline		.
    AnySingleline,		// any character			. (s option)
    Word,			// any word character			\w
    Digit,			// any digit character			\d
    WhiteSpace,		// any whitespace character		\s

    // ECMAScript classes


    EcmaAny,
    EcmaAnySingleline,
    EcmaWord,		// [a-zA-Z_0-9]
    EcmaDigit,		// [0-9]
    EcmaWhiteSpace,		// [ \f\n\r\t\v]

    // unicode categories

    UnicodeL,		// Letter
    UnicodeM,		// Mark
    UnicodeN,		// Number
    UnicodeZ,		// Separator
    UnicodeP,		// Punctuation
    UnicodeS,		// Symbol
    UnicodeC,		// Other

    UnicodeLu,		// UppercaseLetter
    UnicodeLl,		// LowercaseLetter
    UnicodeLt,		// TitlecaseLetter
    UnicodeLm,		// ModifierLetter
    UnicodeLo,		// OtherLetter
    UnicodeMn,		// NonspacingMark
    UnicodeMe,		// EnclosingMark
    UnicodeMc,		// SpacingMark
    UnicodeNd,		// DecimalNumber
    UnicodeNl,		// LetterNumber
    UnicodeNo,		// OtherNumber
    UnicodeZs,		// SpaceSeparator
    UnicodeZl,		// LineSeparator
    UnicodeZp,		// ParagraphSeparator
    UnicodePd,		// DashPunctuation
    UnicodePs,		// OpenPunctuation
    UnicodePi,		// InitialPunctuation
    UnicodePe,		// ClosePunctuation
    UnicodePf,		// FinalPunctuation
    UnicodePc,		// ConnectorPunctuation
    UnicodePo,		// OtherPunctuation
    UnicodeSm,		// MathSymbol
    UnicodeSc,		// CurrencySymbol
    UnicodeSk,		// ModifierSymbol
    UnicodeSo,		// OtherSymbol
    UnicodeCc,		// Control
    UnicodeCf,		// Format
    UnicodeCo,		// PrivateUse
    UnicodeCs,		// Surrogate
    UnicodeCn,		// Unassigned

    // unicode block ranges

    // notes: the categories marked with a star are valid unicode block ranges,
    // but don't seem to be accepted by the MS parser using the /p{...} format.
    // any ideas?

    UnicodeBasicLatin,
    UnicodeLatin1Supplement,			// *
    UnicodeLatinExtendedA,				// *
    UnicodeLatinExtendedB,				// *
    UnicodeIPAExtensions,
    UnicodeSpacingModifierLetters,
    UnicodeCombiningDiacriticalMarks,
    UnicodeGreek,
    UnicodeCyrillic,
    UnicodeArmenian,
    UnicodeHebrew,
    UnicodeArabic,
    UnicodeSyriac,
    UnicodeThaana,
    UnicodeDevanagari,
    UnicodeBengali,
    UnicodeGurmukhi,
    UnicodeGujarati,
    UnicodeOriya,
    UnicodeTamil,
    UnicodeTelugu,
    UnicodeKannada,
    UnicodeMalayalam,
    UnicodeSinhala,
    UnicodeThai,
    UnicodeLao,
    UnicodeTibetan,
    UnicodeMyanmar,
    UnicodeGeorgian,
    UnicodeHangulJamo,
    UnicodeEthiopic,
    UnicodeCherokee,
    UnicodeUnifiedCanadianAboriginalSyllabics,
    UnicodeOgham,
    UnicodeRunic,
    UnicodeKhmer,
    UnicodeMongolian,
    UnicodeLatinExtendedAdditional,
    UnicodeGreekExtended,
    UnicodeGeneralPunctuation,
    UnicodeSuperscriptsandSubscripts,
    UnicodeCurrencySymbols,
    UnicodeCombiningMarksforSymbols,
    UnicodeLetterlikeSymbols,
    UnicodeNumberForms,
    UnicodeArrows,
    UnicodeMathematicalOperators,
    UnicodeMiscellaneousTechnical,
    UnicodeControlPictures,
    UnicodeOpticalCharacterRecognition,
    UnicodeEnclosedAlphanumerics,
    UnicodeBoxDrawing,
    UnicodeBlockElements,
    UnicodeGeometricShapes,
    UnicodeMiscellaneousSymbols,
    UnicodeDingbats,
    UnicodeBraillePatterns,
    UnicodeCJKRadicalsSupplement,
    UnicodeKangxiRadicals,
    UnicodeIdeographicDescriptionCharacters,
    UnicodeCJKSymbolsandPunctuation,
    UnicodeHiragana,
    UnicodeKatakana,
    UnicodeBopomofo,
    UnicodeHangulCompatibilityJamo,
    UnicodeKanbun,
    UnicodeBopomofoExtended,
    UnicodeEnclosedCJKLettersandMonths,
    UnicodeCJKCompatibility,
    UnicodeCJKUnifiedIdeographsExtensionA,
    UnicodeCJKUnifiedIdeographs,
    UnicodeYiSyllables,
    UnicodeYiRadicals,
    UnicodeHangulSyllables,
    UnicodeHighSurrogates,
    UnicodeHighPrivateUseSurrogates,
    UnicodeLowSurrogates,
    UnicodePrivateUse,
    UnicodeCJKCompatibilityIdeographs,
    UnicodeAlphabeticPresentationForms,
    UnicodeArabicPresentationFormsA,		// *
    UnicodeCombiningHalfMarks,
    UnicodeCJKCompatibilityForms,
    UnicodeSmallFormVariants,
    UnicodeArabicPresentationFormsB,		// *
    UnicodeSpecials,
    UnicodeHalfwidthandFullwidthForms,

    UnicodeOldItalic,
    UnicodeGothic,
    UnicodeDeseret,
    UnicodeByzantineMusicalSymbols,
    UnicodeMusicalSymbols,
    UnicodeMathematicalAlphanumericSymbols,
    UnicodeCJKUnifiedIdeographsExtensionB,
    UnicodeCJKCompatibilityIdeographsSupplement,
    UnicodeTags,

    LastValue // Keep this with the higher value in the enumeration
}

class X
{
    static void Main()
    {
        Console.WriteLine(Category.None);
        Console.WriteLine("<%END%>");
    }
}