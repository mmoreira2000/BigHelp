using System.Text;

namespace BigHelp.FluentRegEx
{
    /// <summary>
    /// Grouping constructs delineate subexpressions of a regular expression and typically capture substrings of an input string.
    /// </summary>
    public class RegExBuilderGroups : RegExBuilder
    {
        private readonly StringBuilder _regExStream;
        internal RegExBuilderGroups(StringBuilder regExStream)
        {
            _regExStream = regExStream;
        }

        /// <summary>
        /// Captures the matched subexpression and assigns it a one-based ordinal number.
        /// </summary>
        /// <example>
        /// Pattern: (\w)\1
        /// Expected result: matches "ee" in "deep"
        /// </example>
        /// <param name="subExpression">The subexpression to be captured</param>
        /// <returns>({subExpression})</returns>
        public RegExBuilderQuantifiers NonNamedGroup(RegExBuilder subExpression) => AddToStreamReturnQuantifiers($"({subExpression.Build()})");

        /// <summary>
        /// Captures the matched subexpression into a named group.
        /// </summary>
        /// <example>
        /// Pattern: (?<double>\w)\k<double>	
        /// Expected result: matches "ee" in "deep"
        /// </example>
        /// <param name="subExpression">The subexpression to be captured</param>
        /// <param name="name">The name of the capture group</param>
        /// <param name="useSingleQuotesInsteadOfBrackets">If <value>true</value> will use single quotes instead of brackets to define the group name</param>
        /// <returns>(?<{name}>{subExpression}) or (?'{name}'{subExpression}) if <value>useSingleQuotesInsteadOfBrackets</value> is <value>true</value></returns>
        public RegExBuilderQuantifiers NamedGroup(RegExBuilder subExpression, string name, bool useSingleQuotesInsteadOfBrackets = false)
            => AddToStreamReturnQuantifiers(useSingleQuotesInsteadOfBrackets
                ? $"(?<{name}>{subExpression.Build()})"
                : $"(?'{name}'{subExpression.Build()})");

        /// <summary>
        /// A balancing group definition deletes the definition of a previously defined group and stores, in the current group,
        /// the interval between the previously defined group and the current group.
        /// The balancing group definition deletes the definition of name2 and stores the interval between name2 and name1 in name1.
        /// If no name2 group is defined, the match backtracks. Because deleting the last definition of name2 reveals the previous definition
        /// of name2, this construct lets you use the stack of captures for group name2 as a counter for keeping track of nested constructs
        /// such as parentheses or opening and closing brackets.
        /// The balancing group definition uses name2 as a stack.
        /// The beginning character of each nested construct is placed in the group and in its Group.Captures collection.
        /// When the closing character is matched, its corresponding opening character is removed from the group, and the Captures
        /// collection is decreased by one.After the opening and closing characters of all nested constructs have been matched, name2 is empty.
        /// </summary>
        /// <seealso cref="https://docs.microsoft.com/en-us/dotnet/standard/base-types/grouping-constructs-in-regular-expressions#balancing_group_definition"/>
        /// <example>
        /// Pattern: (((?'Open'\()[^\(\)]*)+((?'Close-Open'\))[^\(\)]*)+)*(?(Open)(?!))$
        /// Expected result: matches "((1-3)*(3-1))" in "3+2^((1-3)*(3-1))"
        /// </example>
        /// <param name="subExpression">The subexpression to be captured</param>
        /// <param name="name1">The name of the current group.</param>
        /// <param name="name2">The name of a previously defined group.</param>
        /// <param name="useSingleQuotesInsteadOfBrackets">If <value>true</value> will use single quotes instead of brackets to define the group name</param>
        /// <returns>(?<{name1}-{name2}>{subExpression}) or (?'{name1}-{name2}'{subExpression}) if <value>useSingleQuotesInsteadOfBrackets</value> is <value>true</value></returns>
        public RegExBuilderQuantifiers BalancingGroup(RegExBuilder subExpression, string name1, string name2, bool useSingleQuotesInsteadOfBrackets = false)
            => AddToStreamReturnQuantifiers(useSingleQuotesInsteadOfBrackets
                ? $"(?<{name1}-{name2}>{subExpression.Build()})"
                : $"(?'{name1}-{name2}'{subExpression.Build()})");


        /// <summary>
        /// Defines a noncapturing group.
        /// </summary>
        /// <example>
        /// Pattern: Write(?:Line)?
        /// Expected result: matches "WriteLine" in "Console.WriteLine()" and "Write" in "Console.Write(value)"
        /// </example>
        /// <param name="subExpression">The subexpression that must be matched but not captured</param>
        /// <returns>(?:{subExpression})</returns>
        public RegExBuilderQuantifiers NonCaptureGroup(RegExBuilder subExpression) => AddToStreamReturnQuantifiers($"(?:{subExpression.Build()})");

        /// <summary>
        /// Zero-width positive lookahead assertion.
        /// </summary>
        /// <example>
        /// Pattern: \b\w+\b(?=.+and.+)	
        /// Expected result: matches "cats", "dogs" in "cats, dogs and some mice."
        /// </example>
        /// <param name="subExpression">The subexpression to be matched</param>
        /// <returns>(?={subExpression})</returns>
        public RegExBuilderQuantifiers PositiveLookAhead(RegExBuilder subExpression) => AddToStreamReturnQuantifiers($"(?={subExpression.Build()})");

        /// <summary>
        /// Zero-width negative lookahead assertion.
        /// </summary>
        /// <example>
        /// Pattern: \b\w+\b(?!.+and.+)	
        /// Expected result: matches "and", "some", "mice" in "cats, dogs and some mice."
        /// </example>
        /// <param name="subExpression">The subexpression to be matched</param>
        /// <returns>(?!{subExpression})</returns>
        public RegExBuilderQuantifiers NegativeLookAhead(RegExBuilder subExpression) => AddToStreamReturnQuantifiers($"(?!{subExpression.Build()})");

        /// <summary>
        /// Zero-width positive lookbehind assertion.
        /// </summary>
        /// <example>
        /// Pattern 1: \b\w+\b(?<=.+and.+)
        /// Expected result 1: matches "some", "mice" in "cats, dogs and some mice."
        /// 
        /// Pattern 2: \b\w+\b(?<=.+and.*)
        /// Expected result 2: matches "and", "some", "mice" in "cats, dogs and some mice."
        /// </example>
        /// <param name="subExpression">The subexpression to be matched</param>
        /// <returns>(?<={subExpression})</returns>
        public RegExBuilderQuantifiers PositiveLookBehind(RegExBuilder subExpression) => AddToStreamReturnQuantifiers($"(?<={subExpression.Build()})");

        /// <summary>
        /// Zero-width negative lookbehind assertion.
        /// </summary>
        /// <example>
        /// Pattern 1: \b\w+\b(?<!.+and.+)
        /// Expected result 1: matches "cats", "dogs", "and" in "cats, dogs and some mice."
        /// 
        /// Pattern 2: \b\w+\b(?<!.+and.*)
        /// Expected result 2: matches "cats", "dogs" in "cats, dogs and some mice."
        /// </example>
        /// <param name="subExpression">The subexpression to be matched</param>
        /// <returns>(?<!{subExpression})</returns>
        public RegExBuilderQuantifiers NegativeLookBehind(RegExBuilder subExpression) => AddToStreamReturnQuantifiers($"(?<!{subExpression.Build()})");

        /// <summary>
        /// Atomic group.
        /// </summary>
        /// <example>
        /// Pattern 1: (?>a|ab)c
        /// Expected result 1: matches "ac" in"ac". Matches nothing in"abc".
        /// 
        /// Pattern 2: (?>ab|a)c
        /// Expected result 2: matches "ac" in"ac" and "abc" in "abc". Matches nothing in"ab".
        /// </example>
        /// <param name="subExpression">The subexpression to be captured</param>
        /// <returns>(?>{subExpression})</returns>
        public RegExBuilderQuantifiers AtomicGroup(RegExBuilder subExpression) => AddToStreamReturnQuantifiers($"(?>{subExpression.Build()})");

    }
}