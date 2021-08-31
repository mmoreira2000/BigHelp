using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BigHelp.FluentRegEx
{
    /// <summary>
    /// Applies or disables the specified options within subexpression.
    /// </summary>
    /// <seealso cref="https://docs.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-options"/>
    public class RegExBuilderOptions
    {
        private readonly List<char> _enables;
        private readonly List<char> _disables;
        private readonly RegExBuilder _regExBuilder;
        private readonly StringBuilder _regExStream;
        internal RegExBuilderOptions(RegExBuilder regExBuilder, StringBuilder regExStream)
        {
            _regExBuilder = regExBuilder;
            _regExStream = regExStream;
            _enables = new List<char>();
            _disables = new List<char>();
        }

        /// <summary>
        /// Enable case-insensitive matching.
        /// </summary>
        /// <returns>i</returns>
        public RegExBuilderOptions CaseInsensitive()
        {
            _enables.Add('i');
            return this;
        }

        /// <summary>
        /// Disable case-insensitive matching.
        /// </summary>
        /// <returns>i</returns>
        public RegExBuilderOptions CaseSensitive()
        {
            _disables.Add('i');
            return this;
        }

        /// <summary>
        /// Use multiline mode, where ^ and $ match the beginning and end of each line (instead of the beginning and end of the input string).
        /// </summary>
        /// <returns>m</returns>
        public RegExBuilderOptions EnableMultiline()
        {
            _enables.Add('m');
            return this;
        }

        /// <summary>
        /// Disable multiline mode.
        /// </summary>
        /// <returns>m</returns>
        public RegExBuilderOptions DisableMultiline()
        {
            _disables.Add('m');
            return this;
        }

        /// <summary>
        /// Use single-line mode, where the period (.) matches every character (instead of every character except \n).
        /// </summary>
        /// <returns>s</returns>
        public RegExBuilderOptions EnableSingleLine()
        {
            _enables.Add('s');
            return this;
        }

        /// <summary>
        /// Disable single-line mode.
        /// </summary>
        /// <returns>m</returns>
        public RegExBuilderOptions DisableSingleLine()
        {
            _disables.Add('m');
            return this;
        }

        /// <summary>
        /// Do not capture unnamed groups.
        /// The only valid captures are explicitly named or numbered groups of the form (?&lt;name&gt; subexpression).
        /// </summary>
        /// <returns>s</returns>
        public RegExBuilderOptions EnableExplicitCapture()
        {
            _enables.Add('n');
            return this;
        }

        /// <summary>
        /// Disable explicit capture mode.
        /// </summary>
        /// <returns>m</returns>
        public RegExBuilderOptions DisableExplicitCapture()
        {
            _disables.Add('n');
            return this;
        }

        /// <summary>
        /// Exclude unescaped white space from the pattern, and enable comments after a number sign (#).
        /// </summary>
        /// <returns>x</returns>
        public RegExBuilderOptions EnableIgnorePatternWhitespace()
        {
            _enables.Add('x');
            return this;
        }

        /// <summary>
        /// Enables the pattern Ignore Whitespace, recognizing ' ' and '#' as characters. It also disables RegEx comments. 
        /// </summary>
        /// <returns>x</returns>
        public RegExBuilderOptions DisableIgnorePatternWhitespace()
        {
            _disables.Add('x');
            return this;
        }

        /// <summary>
        /// Execute the <value>subExpression</value> with this confirguration. 
        /// </summary>
        /// <returns>(?imnsx-imnsx:<value>subExpression</value>)</returns>
        public RegExBuilder Finish(RegExBuilder subExpression)
        {
            _regExStream.Append($"(?");
            _regExStream.Append(_enables);
            if (_disables.Any()) _regExStream.Append('-');
            _regExStream.Append(_disables);
            _regExStream.Append($":{subExpression})");

            return _regExBuilder;
        }

        /// <summary>
        /// Uses this configuration to evaluate the expression from this point
        /// </summary>
        /// <returns>(?imnsx-imnsx)</returns>
        public RegExBuilder Finish()
        {
            _regExStream.Append($"(?");
            _regExStream.Append(_enables);
            if (_disables.Any()) _regExStream.Append('-');
            _regExStream.Append(_disables);
            _regExStream.Append($")");

            return _regExBuilder;
        }
    }
}