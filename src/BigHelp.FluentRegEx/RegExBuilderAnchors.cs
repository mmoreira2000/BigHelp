using System.Text;

namespace BigHelp.FluentRegEx
{
    /// <summary>
    /// Anchors, or atomic zero-width assertions, cause a match to succeed or fail depending on the current position in the string,
    /// but they do not cause the engine to advance through the string or consume characters. 
    /// </summary>
    public class RegExBuilderAnchors : RegExBuilder
    {
        private readonly StringBuilder _regExStream;
        internal RegExBuilderAnchors(StringBuilder regExStream)
        {
            _regExStream = regExStream;
        }

        /// <summary>
        /// By default, the match must start at the beginning of the string; in multiline mode, it must start at the beginning of the line.
        /// </summary>
        /// <example>
        /// Pattern: ^\d{3}
        /// Expected result: matches "901" in "901-333-"
        /// </example>
        /// <returns>^</returns>
        public RegExBuilder LineBegin() => AddToStreamReturnQuantifiers("^");

        /// <summary>
        /// By default, the match must occur at the end of the string or before \n at the end of the string; in multiline mode,
        /// it must occur before the end of the line or before \n at the end of the line.
        /// </summary>
        /// <example>
        /// Pattern: -\d{3}$
        /// Expected result: matches "-333" in "-901-333"
        /// </example>
        /// <returns>$</returns>
        public RegExBuilder LineEnd() => AddToStreamReturnQuantifiers("$");

        /// <summary>
        /// The match must occur at the start of the string.
        /// </summary>
        /// <example>
        /// Pattern: \A\d{3}
        /// Expected result: matches "901" in "901-333-"
        /// </example>
        /// <returns>\A</returns>
        public RegExBuilder TextBegin() => AddToStreamReturnQuantifiers(@"\A");

        /// <summary>
        /// The match must occur at the end of the string or before \n at the end of the string.
        /// </summary>
        /// <example>
        /// Pattern: -\d{3}\Z
        /// Expected result: matches "-333" in "-901-333"
        /// </example>
        /// <returns>\Z</returns>
        public RegExBuilder TextEndOrLineBreakAtEnd() => AddToStreamReturnQuantifiers(@"\Z");

        /// <summary>
        /// The match must occur at the end of the string.
        /// </summary>
        /// <example>
        /// Pattern: -\d{3}\z
        /// Expected result: matches "-333" in "-901-333"
        /// </example>
        /// <returns>\z</returns>
        public RegExBuilder TextEnd() => AddToStreamReturnQuantifiers(@"\z");

        /// <summary>
        /// The match must occur at the point where the previous match ended.
        /// </summary>
        /// <example>
        /// Pattern: \G\(\d\)
        /// Expected result: matches "(1)", "(3)", "(5)" in "(1)(3)(5)[7](9)"
        /// </example>
        /// <returns>\G</returns>
        public RegExBuilder BeginOnPreviusMatch() => AddToStreamReturnQuantifiers(@"\G");

        /// <summary>
        /// The match must occur on a boundary between the two "\b".
        /// </summary>
        /// <example>
        /// Pattern: \b\w+\s\w+\b
        /// Expected behavior: The match must occur on a boundary between a \w (alphanumeric) and another \w (alphanumeric) characters.
        /// Expected result: "them theme", "them them" in "them theme them them"
        /// </example>
        /// <param name="regExBuilder">The regular expression pattern that needs to be matched inside de boundary</param>
        /// <returns>\b<value>regExBuilder</value>\b</returns>
        public RegExBuilder Boundary(RegExBuilder regExBuilder) => AddToStreamReturnQuantifiers($@"\b{regExBuilder.Build()}\b");

        /// <summary>
        /// The match must occur on a boundary between the two "\B" but does not match the anchor characters at the begin and end.
        /// </summary>
        /// <example>
        /// Pattern: \B\w+\s\w+\B
        /// Expected behavior: The match must occur on a boundary between a \w (alphanumeric) and another \w (alphanumeric) characters.
        /// Expected result: "hem theme them the" in "them theme them them"
        /// </example>
        /// <param name="regExBuilder">The regular expression pattern that needs to be matched inside de boundary</param>
        /// <returns>\B<value>regExBuilder</value>\B</returns>
        public RegExBuilder BoundaryExcludeStartAndEndCharacters(RegExBuilder regExBuilder) => AddToStreamReturnQuantifiers($@"\B{regExBuilder.Build()}\B");

        /// <summary>
        /// The match must occur on a boundary between a "\B" and "\b" but does not match the anchor character at the begin.
        /// </summary>
        /// <example>
        /// Pattern: \B\w+\s\w+\b
        /// Expected behavior: The match must occur on a boundary between a \w (alphanumeric) and another \w (alphanumeric) characters.
        /// Expected result: "hem theme", "hem them" in "them theme them them"
        /// </example>
        /// <param name="regExBuilder">The regular expression pattern that needs to be matched inside de boundary</param>
        /// <returns>\B<value>regExBuilder</value>\b</returns>
        public RegExBuilder BoundaryExcludeStartCharacter(RegExBuilder regExBuilder) => AddToStreamReturnQuantifiers($@"\B{regExBuilder.Build()}\b");

        /// <summary>
        /// The match must occur on a boundary between a "\b" and "\B" but does not match the anchor character at the end.
        /// </summary>
        /// <example>
        /// Pattern: \b\w+\s\w+\B
        /// Expected behavior: The match must occur on a boundary between a \w (alphanumeric) and another \w (alphanumeric) characters.
        /// Expected result: "them them", "them the" in "them theme them them"
        /// </example>
        /// <param name="regExBuilder">The regular expression pattern that needs to be matched inside de boundary</param>
        /// <returns>\b<value>regExBuilder</value>\B</returns>
        public RegExBuilder BoundaryExcludeEndCharacter(RegExBuilder regExBuilder) => AddToStreamReturnQuantifiers($@"\b{regExBuilder.Build()}\B");
    }
}