using System;
using System.Collections.Generic;
using System.Text;

namespace $RootNamespace$.Text
{
    public static class StringExtensions
    {
        public static string AfterDelimiter(this string sourceString, string delimiter, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
        {
            int startPos = sourceString.IndexOf(delimiter, stringComparison);
            if (startPos < 0) return null;

            startPos += delimiter.Length;
            return sourceString.Substring(startPos);
        }

        public static string BetweenDelimiters(this string sourceString, string startDelimiter, string endDelimiter, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
        {
            int startPos = sourceString.IndexOf(startDelimiter, stringComparison);
            if (startPos < 0) return null;

            startPos += startDelimiter.Length;

            string m = sourceString.Substring(startPos);

            int endPos = m.IndexOf(endDelimiter, stringComparison);

            return m.Substring(0, endPos);
        }

        public static bool IsMatch(this string sourceString,
                                   string searchText,
                                   MatchTypeEnum matchType = MatchTypeEnum.ExactMatch,
                                   StringComparison comparer = StringComparison.OrdinalIgnoreCase)
        {
            if (sourceString == null || searchText == null) return false;

            switch (matchType)
            {
                case MatchTypeEnum.ExactMatch:
                    return string.Equals(sourceString, searchText, comparer);
                case MatchTypeEnum.BeginsWith:
                    return sourceString.StartsWith(searchText, comparer);
                case MatchTypeEnum.EndsWith:
                    return sourceString.EndsWith(searchText, comparer);
                case MatchTypeEnum.Contains:
                    return sourceString.IndexOf(searchText, comparer) >= 0;
                default:
                    throw new ArgumentOutOfRangeException(nameof(matchType), matchType, null);
            }
        }

        public static string ToFormattedString<T>(this IEnumerable<T> enumerable, Func<T, string> itemFormat = null)
        {
            if (itemFormat == null) itemFormat = item => item.ToString();

            var sb = new StringBuilder();

            if (enumerable is IList<T> l)
            {
                sb.AppendLine($"Total Itens: {l.Count}");
            }

            int counter = 0;
            foreach (var item in enumerable)
            {
                sb.AppendLine($"    {counter++}: {itemFormat(item)}");
            }
            return sb.ToString();
        }

        public static string Mask(this string str, char maskChar = '*', int numCharsToPreserveBefore = 3, int numCharsToPreserveAfter = 3, int? fixedMaskSize = null)
        {
            if (string.IsNullOrEmpty(str)) return str;
            if (str.Length <= numCharsToPreserveBefore + numCharsToPreserveAfter) return str;

            int numFill = fixedMaskSize.HasValue ? numCharsToPreserveBefore + fixedMaskSize.Value : str.Length - numCharsToPreserveAfter;

            var maskedString = str.Substring(0, numCharsToPreserveBefore).PadRight(numFill, maskChar)
                               + str.Substring(str.Length - numCharsToPreserveAfter, numCharsToPreserveAfter);

            return maskedString;

        }
    }

    public enum MatchTypeEnum
    {
        ExactMatch,
        BeginsWith,
        EndsWith,
        Contains
    }

}

