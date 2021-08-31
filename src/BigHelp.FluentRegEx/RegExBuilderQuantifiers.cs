using System.Text;

namespace BigHelp.FluentRegEx
{
    /// <summary>
    /// Fluent build quantifier for regular expressions.
    /// <see cref="https://docs.microsoft.com/en-us/dotnet/standard/base-types/quantifiers-in-regular-expressions"/>
    /// </summary>
    public class RegExBuilderQuantifiers : RegExBuilder
    {
        private readonly StringBuilder _regExStream;
        internal RegExBuilderQuantifiers(StringBuilder regExStream)
        {
            _regExStream = regExStream;
        }

        /// <summary>
        /// Matches the previous element zero or more times.
        /// </summary>
        /// <example>
        /// Pattern: \d*\.\d
        /// Expected result: matches ".0", "19.9", "219.9"
        /// </example>
        /// <returns>*</returns>
        public RegExBuilder ZeroOrMore() => AddToStream("*");

        /// <summary>
        /// Matches the previous element one or more times.	
        /// </summary>
        /// <example>
        /// Pattern: "be+"
        /// Expected result: matches "bee" in "been", "be" in "bent"
        /// </example>
        /// <returns>+</returns>
        public RegExBuilder OneOrMore() => AddToStream("+");

        /// <summary>
        /// Matches the previous element zero or one time.	
        /// </summary>
        /// <example>
        /// Pattern: "rai?n"
        /// Expected result: matches "ran", "rain"
        /// </example>
        /// <returns>?</returns>
        public RegExBuilder Optional() => AddToStream("?");

        /// <summary>
        /// Matches the previous element exactly n times.	
        /// </summary>
        /// <example>
        /// Pattern: ",\d{3}"
        /// Expected result: matches ",043" in "1,043.6", ",876", ",543", and ",210" in "9,876,543,210"
        /// </example>
        /// <param name="numberOfTimes">The exactly number of times de previous expression should match</param>
        /// <returns>{numberOfTimes}</returns>
        public RegExBuilder Times(int numberOfTimes) => AddToStream($"{{{numberOfTimes}}}");

        /// <summary>
        /// Matches the previous element at least n times.
        /// </summary>
        /// <example>
        /// Pattern: "\d{2,}"
        /// Expected result: matches "166", "29", "1930"
        /// </example>
        /// <param name="numberOfTimes">The exactly number of times that the previous expression should match</param>
        /// <returns>{numberOfTimes,}</returns>
        public RegExBuilder AtLeast(int numberOfTimes) => AddToStream($"{{{numberOfTimes},}}");

        /// <summary>
        /// Matches the previous element at least <value>minimumTimes</value> times, but no more than <value>maximumTimes</value> times.
        /// </summary>
        /// <example>
        /// Pattern: "\d{3,5}"
        /// Expected result: matches "166", "17668" and "19302" in "193024"
        /// </example>
        /// <param name="minimumTimes">The minimum number of times that the previous expression should match</param>
        /// <param name="maximumTimes">The maximum number of times that the previous expression should match</param>
        /// <returns>{min,max}</returns>
        public RegExBuilder Times(int minimumTimes, int maximumTimes) => AddToStream($"{{{minimumTimes},{maximumTimes}}}");

        /// <summary>
        /// Matches the previous element zero or more times, but as few times as possible.
        /// </summary>
        /// <example>
        /// Pattern: \d*?\.\d
        /// Expected result: matches ".0", "19.9", "219.9"
        /// </example>
        /// <returns>*?</returns>
        public RegExBuilder ZeroOrMoreNonGreed() => AddToStream("*?");

        /// <summary>
        /// Matches the previous element one or more times, but as few times as possible.
        /// </summary>
        /// <example>
        /// Pattern: "be+?"
        /// Expected result: matches "be" in "been", "be" in "bent"
        /// </example>
        /// <returns>+?</returns>
        public RegExBuilder OneOrMoreNonGreed() => AddToStream("+?");

        /// <summary>
        /// Matches the previous element zero or one time, but as few times as possible.	
        /// </summary>
        /// <example>
        /// Pattern: "rai??n"
        /// Expected result: matches "ran", "rain"
        /// </example>
        /// <returns>??</returns>
        public RegExBuilder OptionalNonGreed() => AddToStream("??");

        /// <summary>
        /// Matches the previous element at least n times, but as few times as possible.
        /// </summary>
        /// <example>
        /// Pattern: "\d{2,}?"
        /// Expected result: matches "166", "29", "1930"
        /// </example>
        /// <param name="numberOfTimes">The exactly number of times that the previous expression should match</param>
        /// <returns>{numberOfTimes,}?</returns>
        public RegExBuilder AtLeastNonGreed(int numberOfTimes) => AddToStream($"{{{numberOfTimes},}}?");

        /// <summary>
        /// Matches the previous element at least <value>minimumTimes</value> times, but no more than <value>maximumTimes</value> times, but as few times as possible.
        /// </summary>
        /// <example>
        /// Pattern: "\d{3,5}"
        /// Expected result: matches "166", "17668" and "19302" in "193024"
        /// </example>
        /// <param name="minimumTimes">The minimum number of times that the previous expression should match</param>
        /// <param name="maximumTimes">The maximum number of times that the previous expression should match</param>
        /// <returns>{min,max}?</returns>
        public RegExBuilder TimesNonGreed(int minimumTimes, int maximumTimes) => AddToStream($"{{{minimumTimes},{maximumTimes}}}?");

    }
}