using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BigHelp.FluentRegEx
{
    public class RegExBuilderClasses : RegExBuilder
    {
        private readonly StringBuilder _regExStream;

        internal RegExBuilderClasses(StringBuilder regExStream)
        {
            _regExStream = regExStream;
        }

        /// <summary>
        /// Matches any of the characters in the <value>options</value> collection.
        /// </summary>
        /// <param name="options">The character set to be included in the set</param>
        /// <returns>[<value>options</value>]</returns>
        public RegExBuilderQuantifiers InSet(IEnumerable<Char> options) => InSet(new string(options.ToArray()));

        /// <summary>
        /// Matches any of the characters in the <value>options</value> string.
        /// </summary>
        /// <param name="options">The character set, range or the combination of both to be included in the set</param>
        /// <returns>[<value>value</value>]</returns>
        public RegExBuilderQuantifiers InSet(string options) => AddToStreamReturnQuantifiers($"[{options}]");

        /// <summary>
        /// Matches any characters that's not in the <value>options</value> string.
        /// </summary>
        /// <param name="options">The character set to be included in the negation set</param>
        /// <returns>[^<value>options</value>]</returns>
        public RegExBuilderQuantifiers NotInSet(IEnumerable<Char> options) => NotInSet(new string(options.ToArray()));

        /// <summary>
        /// Matches any characters that's not in the <value>options</value> string.
        /// </summary>
        /// <param name="options">The character set, range or the combination of both to be included in the negation set</param>
        /// <returns>[^<value>options</value>]</returns>
        public RegExBuilderQuantifiers NotInSet(string options) => AddToStreamReturnQuantifiers($"[^{options}]");

        /// <summary>
        /// Matches any characters between the <value>start</value> and <value>end</value> values, inclusive.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>[<value>start</value>-<value>end</value>]</returns>
        public RegExBuilderQuantifiers Range(char start, char end) => AddToStreamReturnQuantifiers($"[{start}-{end}]");

        /// <summary>
        /// Matches any characters that's not between the <value>start</value> and <value>end</value> values, inclusive.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>[^<value>start</value>-<value>end</value>]</returns>
        public RegExBuilderQuantifiers NotInRange(char start, char end) => AddToStreamReturnQuantifiers($"[^{start}-{end}]");

    }
}