using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BigHelp.FluentRegEx
{
    public class RegExBuilder
    {
        // ReSharper disable once InconsistentNaming
        private readonly char[] CHARACTERS_THAT_NEED_TO_BE_SCAPED = @"^$.|{}[]()*+?\".ToArray();
        private readonly StringBuilder _regExStream;
        protected readonly RegExBuilderQuantifiers QuantifiersBuilder;

        internal RegExBuilder()
        {
            _regExStream = new StringBuilder();
            QuantifiersBuilder = new RegExBuilderQuantifiers(_regExStream);
            Classes = new RegExBuilderClasses(_regExStream);
            Anchors = new RegExBuilderAnchors(_regExStream);
            Groups = new RegExBuilderGroups(_regExStream);
            BackReference = new RegExBuilderBackReference(_regExStream);
            Options = new RegExBuilderOptions(this, _regExStream);
        }

        /// <summary>
        /// Access Regular Expression inline options.
        /// </summary>
        public RegExBuilderOptions Options { get; }

        /// <summary>
        /// Access Regular Expression back reference options.
        /// </summary>
        public RegExBuilderBackReference BackReference { get; }

        /// <summary>
        /// Access Regular Expression group options.
        /// </summary>
        public RegExBuilderGroups Groups { get; }

        /// <summary>
        /// Access Regular Expression class options.
        /// </summary>
        public RegExBuilderClasses Classes { get; }

        /// <summary>
        /// Access Regular Expression anchors.
        /// </summary>
        public RegExBuilderAnchors Anchors { get; }

        /// <summary>
        /// Matches a specific text. It automatically escapes special characters if necessary.
        /// </summary>
        /// <example>
        ///		The text
        /// "I'm a John Doe? (o.O)"
        ///		will result in the following expression:
        /// "I'm\sa\sJohn\sDoe\?\s\(o\.O\)"
        /// </example>
        /// <param name="text">the text to be matched</param>
        /// <returns>The <value>text</value> with characters escaped if necessary</returns>
        public RegExBuilderQuantifiers Text(string text)
        {
            //scape special chars
            foreach (char c in text)
            {
                if (CHARACTERS_THAT_NEED_TO_BE_SCAPED.Contains(c))
                    _regExStream.Append($@"\{c}");
                else
                    _regExStream.Append(c);
            }
            return QuantifiersBuilder;
        }

        /// <summary>
        /// Matches a bell character "\u0007".
        /// </summary>
        /// <returns>\a</returns>
        public RegExBuilderQuantifiers Bell() => AddToStreamReturnQuantifiers(@"\a");

        /// <summary>
        /// Matches a backspace "\u0008".
        /// </summary>
        /// <returns>\b</returns>
        public RegExBuilderQuantifiers Backspace() => AddToStreamReturnQuantifiers(@"\b");
        /// <summary>
        /// Matches a tab "\u0009".
        /// </summary>
        /// <returns>\t</returns>
        public RegExBuilderQuantifiers Tab() => AddToStreamReturnQuantifiers(@"\t");

        /// <summary>
        /// Matches a carriage return "\u000D".
        /// </summary>
        /// <returns>\r</returns>
        public RegExBuilderQuantifiers CarriageReturn() => AddToStreamReturnQuantifiers(@"\r");

        /// <summary>
        /// Matches a new line "\u000A".
        /// </summary>
        /// <returns>\n</returns>
        public RegExBuilderQuantifiers Newline() => AddToStreamReturnQuantifiers(@"\n");

        /// <summary>
        /// Matches a vertial tab "\u000B".
        /// </summary>
        /// <returns>\v</returns>
        public RegExBuilderQuantifiers VerticalTab() => AddToStreamReturnQuantifiers(@"\v");

        /// <summary>
        /// Matches a form feed "\u000C".
        /// </summary>
        /// <returns>\f</returns>
        public RegExBuilderQuantifiers FormFeed() => AddToStreamReturnQuantifiers(@"\f");

        /// <summary>
        /// Matches a escape "\u001B".
        /// </summary>
        /// <returns>\e</returns>
        public RegExBuilderQuantifiers Escape() => AddToStreamReturnQuantifiers(@"\e");

        /// <summary>
        /// Matches a character expressed in a octal notation.
        /// </summary>
        /// <param name="value">octal representation of a character</param>
        /// <returns>\<value>value</value></returns>
        public RegExBuilderQuantifiers Octal(int value)
        {
            if (value < 10 || value > 999)
                throw new FluentRegExBuilderException(
                    $"Octal representation must contain only 2 or 3 digits, but the value provided was '{value}");
            return AddToStreamReturnQuantifiers($@"\{value}");
        }

        /// <summary>
        /// Matches a character expressed in a hexadecimal notation.
        /// </summary>
        /// <param name="value">hexadecimal representation of a character</param>
        /// <returns>\x<value>value</value></returns>
        public RegExBuilderQuantifiers Hex(string value)
        {
            if (value.Length != 2)
                throw new FluentRegExBuilderException(
                    $"The value provided '{value}' must be exactly 2 digits, but it has '{value.Length}'.");

            try
            {
                _ = Convert.ToInt32(value, 16);
                return AddToStreamReturnQuantifiers($@"\x{value}");
            }
            catch (Exception e)
            {
                throw new FluentRegExBuilderException(
                    $"The value provided '{value}' is not a valid hexadecimal number.");
            }
        }

        /// <summary>
        /// Matches the ASCII control character that is specified by <value>value</value>,
        /// where <value>value</value> is the letter of the control character.
        /// </summary>
        /// <param name="value">The ASCII character of the control key</param>
        /// <returns>\c<value>value</value></returns>
        public RegExBuilderQuantifiers Control(char value)
        {
            if (value > 127)
                throw new FluentRegExBuilderException(
                    $"The value provided '{value}' is not a ASCII character. ASCII characters ranges from 0 to 127, but the value provided is '{Convert.ToInt32(value)}");

            return AddToStreamReturnQuantifiers($@"\c{value}");
        }

        /// <summary>
        /// Matches a character expressed in a unicode notation.
        /// </summary>
        /// <param name="value">A 4 digits unicode hexadecimal representation of a character</param>
        /// <returns>\u<value>value</value></returns>
        public RegExBuilderQuantifiers Unicode(string value)
        {
            if (value.Length != 4)
                throw new FluentRegExBuilderException(
                    $"The value provided '{value}' must be exactly 4 digits, but it has '{value.Length}'.");

            try
            {
                _ = Convert.ToInt32(value, 16);
                return AddToStreamReturnQuantifiers($@"\x{value}");
            }
            catch (Exception e)
            {
                throw new FluentRegExBuilderException(
                    $"The value provided '{value}' is not a valid hexadecimal number.");
            }
        }

        /// <summary>
        /// Matches anything.
        /// </summary>
        /// <returns>.</returns>
        public RegExBuilderQuantifiers Anything() => AddToStreamReturnQuantifiers(".");

        /// <summary>
        /// Matches the specified unicode category.
        /// </summary>
        /// <param name="unicodeInfo">The unicode catagory to be matched.</param>
        /// <returns>\p{<value>category</value>}</returns>
        public RegExBuilderQuantifiers UnicodeCategoryOrBlockOrScript(UnicodeData unicodeInfo) => AddToStreamReturnQuantifiers($@"\p{unicodeInfo.Code}");

        /// <summary>
        /// Matches anything that does not belong to the specified unicode category.
        /// </summary>
        /// <param name="unicodeInfo">The unicode catagory not to be matched.</param>
        /// <returns>\P{<value>category</value>}</returns>
        public RegExBuilderQuantifiers NonUnicodeCategoryOrBlockOrScript(UnicodeData unicodeInfo) => AddToStreamReturnQuantifiers($@"\P{unicodeInfo.Code}");

        /// <summary>
        /// Matches any word character.
        /// </summary>
        /// <example>"I", "D", "A", "1", "3" in "ID A1.3"</example>
        /// <returns>\w</returns>
        public RegExBuilderQuantifiers Word() => AddToStreamReturnQuantifiers(@"\w");

        /// <summary>
        /// Matches any non-word character.
        /// </summary>
        /// <example>" ", "." in "ID A1.3"</example>
        /// <returns>\W</returns>
        public RegExBuilderQuantifiers NonWord() => AddToStreamReturnQuantifiers(@"\W");

        /// <summary>
        /// Matches any white-space character.
        /// </summary>
        /// <returns>\s</returns>
        public RegExBuilderQuantifiers Whitespace() => AddToStreamReturnQuantifiers(@"\s");

        /// <summary>
        /// Matches any non-white-space character.
        /// </summary>
        /// <returns>\S</returns>
        public RegExBuilderQuantifiers NonWhitespace() => AddToStreamReturnQuantifiers(@"\S");

        /// <summary>
        /// Matches any decimal digit.
        /// </summary>
        /// <returns>\d</returns>
        public RegExBuilderQuantifiers Digit() => AddToStreamReturnQuantifiers(@"\d");

        /// <summary>
        /// Matches any character other than a decimal digit.
        /// </summary>
        /// <returns>\D</returns>
        public RegExBuilderQuantifiers NonDigit() => AddToStreamReturnQuantifiers(@"\D");
        /// <summary>
        /// Matches a bell character "\u0007". output=\a
        /// </summary>

        public RegExBuilder Choice(RegExBuilder firstChoice, RegExBuilder secondChoice, params RegExBuilder[] additionalChoices)
        {
            AddToStreamReturnQuantifiers($"({firstChoice}|{secondChoice}");

            foreach (RegExBuilder choice in additionalChoices)
                AddToStreamReturnQuantifiers($"|{choice}");

            AddToStreamReturnQuantifiers(")");
            return this;
        }

        /// <summary>
        /// Adds the contents provided in regExRawData directly to the stream that produces the final regular expression.
        /// Use if you need to add raw regular expression snippets directly to the regex stream builder. 
        /// </summary>
        /// <param name="regExRawData">The raw regular expression snippet to be added to the build stream.</param>
        public RegExBuilder AddRaw(string regExRawData) => AddToStreamReturnQuantifiers(regExRawData);

        internal RegExBuilderQuantifiers AddToStreamReturnQuantifiers(string rawData)
        {
            _regExStream.Append(rawData);
            return QuantifiersBuilder;
        }
        internal RegExBuilder AddToStream(string rawData)
        {
            _regExStream.Append(rawData);
            return this;
        }

        //bool IsLetterWithDiacritics(char c)
        //{
        //    var s = c.ToString().Normalize(NormalizationForm.FormD);
        //    return (s.Length > 1) &&
        //           char.IsLetter(s[0]) &&
        //           s.Skip(1).All(c2 => CharUnicodeInfo.GetUnicodeCategory(c2) == UnicodeCategory.NonSpacingMark);
        //}

        /// <summary>
        /// Produces the regular expression text representation of the builder.
        /// </summary>
        /// <returns>Returns the regular expression string.</returns>
        public string Build() => _regExStream.ToString();
        /// <summary>
        /// Produces the regular expression text representation of the builder.
        /// </summary>
        /// <returns>Returns the regular expression string.</returns>
        public override string ToString() => Build();
    }
}