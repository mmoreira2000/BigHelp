using System.Text;

namespace BigHelp.FluentRegEx
{
    public class RegExBuilderBackReference : RegExBuilder
    {
        private readonly StringBuilder _regExStream;
        internal RegExBuilderBackReference(StringBuilder regExStream)
        {
            _regExStream = regExStream;
        }

        /// <summary>
        /// Backreference. Matches the value of a numbered subexpression.
        /// </summary>
        /// <example>
        /// Pattern: (\w)\1
        /// Expected result: matches "ee" in "seek"
        /// </example>
        /// <param name="number">The exactly number of times that the previous expression should match</param>
        /// <returns>\number</returns>
        public RegExBuilderQuantifiers RefersToUnamedGroup(int number) => AddToStreamReturnQuantifiers($"\\{number}");

        /// <summary>
        /// Named backreference. Matches the value of a named expression.
        /// </summary>
        /// <example>
        /// Pattern: (\w)\1
        /// Expected result: matches (?&lt;char&gt;\w)\k&lt;char&gt;
        /// </example>
        /// <param name="name">The exactly number of times that the previous expression should match</param>
        /// <returns>\k&lt;{name}&gt;</returns>
        public RegExBuilderQuantifiers RefersToNamedGroup(string name) => AddToStreamReturnQuantifiers($"\\k<{name}>");

    }
}