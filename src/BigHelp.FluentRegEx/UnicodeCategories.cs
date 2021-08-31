using System;
using System.Collections.Generic;
using System.Text;

namespace BigHelp.FluentRegEx
{

    public static class UnicodeCategories
    {
        /// <summary>
        /// Matches any uppercase letter.
        /// </summary>
        /// <returns>Lu</returns>
        public static UnicodeData LetterUppercase { get; } = new UnicodeData("Lu");
        /// <summary>
        /// Matches any lowercase letter.
        /// </summary>
        /// <returns>Ll</returns>
        public static UnicodeData LetterLowercase { get; } = new UnicodeData("Ll");
        /// <summary>
        /// Matches 2 letters, the first must be uppercase and the second one, lowercase.
        /// </summary>
        /// <returns>Lt</returns>
        public static UnicodeData LetterTitlecase { get; } = new UnicodeData("Lt");
        /// <summary>
        /// Matches any uppercase or lowercase letter.
        /// </summary>
        /// <returns>LC</returns>
        public static UnicodeData LetterAnycase { get; } = new UnicodeData("LC");
        /// <summary>
        /// Matches a modifier letter.
        /// </summary>
        /// <returns>LC</returns>
        public static UnicodeData LetterModifier { get; } = new UnicodeData("Lm");
        /// <summary>
        /// Matches other letters, including syllables and ideographs.
        /// </summary>
        /// <returns>Lo</returns>
        public static UnicodeData LetterOther { get; } = new UnicodeData("Lo");
        /// <summary>
        /// Matches a letters. Corresponds to Lu | Ll | Lt | Lm | Lo.
        /// </summary>
        /// <returns>L</returns>
        public static UnicodeData Letter { get; } = new UnicodeData("L");

        /// <summary>
        /// Matches a nonspacing combining mark (zero advance width).
        /// </summary>
        /// <returns>Mn</returns>
        public static UnicodeData MarkNonspacing { get; } = new UnicodeData("Mn");

        /// <summary>
        /// Matches a spacing combining mark (positive advance width).
        /// </summary>
        /// <returns>Mc</returns>
        public static UnicodeData MarkSpacing { get; } = new UnicodeData("Mc");

        /// <summary>
        /// Matches an enclosing combining mark.
        /// </summary>
        /// <returns>Me</returns>
        public static UnicodeData MarkEnclosing { get; } = new UnicodeData("Me");

        /// <summary>
        /// Matches a spacing, nonspacing or an enclosing combining mark. Corresponds to Mn | Mc | Me.
        /// </summary>
        /// <returns>M</returns>
        public static UnicodeData Mark { get; } = new UnicodeData("M");

        /// <summary>
        /// Matches a decimal digit.
        /// </summary>
        /// <returns>Nd</returns>
        public static UnicodeData NumberDigit { get; } = new UnicodeData("Nd");

        /// <summary>
        /// Matches a letterlike numeric character.
        /// </summary>
        /// <returns>Nl</returns>
        public static UnicodeData NumberLetter { get; } = new UnicodeData("Nl");

        /// <summary>
        /// Matches a numeric character of other type.
        /// </summary>
        /// <returns>No</returns>
        public static UnicodeData NumberOther { get; } = new UnicodeData("No");

        /// <summary>
        /// Matches any numeric type character. Corresponds to Nd | Nl | No.
        /// </summary>
        /// <returns>N</returns>
        public static UnicodeData Number { get; } = new UnicodeData("N");

        /// <summary>
        /// Matches a connecting punctuation mark, like a tie.
        /// </summary>
        /// <returns>Pc</returns>
        public static UnicodeData PunctuationConnector { get; } = new UnicodeData("Pc");
        /// <summary>
        /// Matches a dash or hyphen punctuation mark.
        /// </summary>
        /// <returns>Pd</returns>
        public static UnicodeData PunctuationDash { get; } = new UnicodeData("Pd");
        /// <summary>
        /// Matches an opening punctuation mark (of a pair).
        /// </summary>
        /// <returns>Ps</returns>
        public static UnicodeData PunctuationOpen { get; } = new UnicodeData("Ps");
        /// <summary>
        /// Matches an closing punctuation mark (of a pair).
        /// </summary>
        /// <returns>Pe</returns>
        public static UnicodeData PunctuationClose { get; } = new UnicodeData("Pe");
        /// <summary>
        /// Matches an initial quotation mark.
        /// </summary>
        /// <returns>Pi</returns>
        public static UnicodeData PunctuationInitial { get; } = new UnicodeData("Pi");
        /// <summary>
        /// Matches an final quotation mark.
        /// </summary>
        /// <returns>Pf</returns>
        public static UnicodeData PunctuationFinal { get; } = new UnicodeData("Pf");
        /// <summary>
        /// Matches a punctuation mark of other type.
        /// </summary>
        /// <returns>Po</returns>
        public static UnicodeData PunctuationOther { get; } = new UnicodeData("Po");
        /// <summary>
        /// Matches any punctuation mark. Corresponds to Pc | Pd | Ps | Pe | Pi | Pf | Po.
        /// </summary>
        /// <returns>P</returns>
        public static UnicodeData Punctuation { get; } = new UnicodeData("P");
        /// <summary>
        /// Matches a symbol of mathematical use.
        /// </summary>
        /// <returns>Pf</returns>
        public static UnicodeData SymbolMath { get; } = new UnicodeData("Sm");
        /// <summary>
        /// Matches a currency sign.
        /// </summary>
        /// <returns>Sc</returns>
        public static UnicodeData SymbolCurrency { get; } = new UnicodeData("Sc");
        /// <summary>
        /// Matches a non-letterlike modifier symbol.
        /// </summary>
        /// <returns>Sk</returns>
        public static UnicodeData SymbolModifier { get; } = new UnicodeData("Sk");
        /// <summary>
        /// Matches a symbol of other type.
        /// </summary>
        /// <returns>So</returns>
        public static UnicodeData SymbolOther { get; } = new UnicodeData("So");

        /// <summary>
        /// Matches any symbol. Corresponds to Sm | Sc | Sk | So.
        /// </summary>
        /// <returns>S</returns>
        public static UnicodeData Symbol { get; } = new UnicodeData("S");

        /// <summary>
        /// Matches a space character (of various non-zero widths).
        /// </summary>
        /// <returns>Zs</returns>
        public static UnicodeData SeparatorSpace { get; } = new UnicodeData("Zs");

        /// <summary>
        /// Matches a line separator (U+2028 LINE SEPARATOR only).
        /// </summary>
        /// <returns>Zl</returns>
        public static UnicodeData SeparatorLine { get; } = new UnicodeData("Zl");

        /// <summary>
        /// Matches a paragraph separator (U+2029 PARAGRAPH SEPARATOR only).
        /// </summary>
        /// <returns>Zp</returns>
        public static UnicodeData SeparatorParagraph { get; } = new UnicodeData("Zp");

        /// <summary>
        /// Matches any separator. Corresponds to Zs | Zl | Zp.
        /// </summary>
        /// <returns>Z</returns>
        public static UnicodeData Separator { get; } = new UnicodeData("Z");

        /// <summary>
        /// Matches a C0 or C1 control code.
        /// </summary>
        /// <returns>Cc</returns>
        public static UnicodeData ControlCode { get; } = new UnicodeData("Cc");

        /// <summary>
        /// Matches a format control character.
        /// </summary>
        /// <returns>Cf</returns>
        public static UnicodeData ControlFormat { get; } = new UnicodeData("Cf");

        /// <summary>
        /// Matches a surrogate code point.
        /// </summary>
        /// <returns>Cs</returns>
        public static UnicodeData ControlSurrogate { get; } = new UnicodeData("Cs");

        /// <summary>
        /// Matches a private-use character.
        /// </summary>
        /// <returns>Co</returns>
        public static UnicodeData ControlPrivateUse { get; } = new UnicodeData("Co");

        /// <summary>
        /// Matches a reserved unassigned code point or a noncharacter.
        /// </summary>
        /// <returns>Cn</returns>
        public static UnicodeData ControlUnassigned { get; } = new UnicodeData("Cn");

        /// <summary>
        /// Matches any control character. Corresponds to Cc | Cf | Cs | Co | Cn.
        /// </summary>
        /// <returns>C</returns>
        public static UnicodeData Control { get; } = new UnicodeData("C");
    }
}
