using System;
using System.Globalization;
using System.Text;

namespace BigHelp.Text
{
    public static class TextExtensions
    {
        public static string RemoveDiatrics(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();
            foreach (char c in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark) stringBuilder.Append(c);
            }
            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static T ToEnum<T>(this string value, T defaultValue = default, bool ignoreCase = true) where T : struct
        {
            if (System.Enum.TryParse<T>(value, ignoreCase, out T result))
            {
                return result;
            }
            return defaultValue;
        }
    }
}
