using System;
using System.Collections.Generic;
using System.Text;

namespace BigHelp.FluentRegEx
{
    public static class CommonTemplates
    {
        /// <summary>
        /// Matches a new line with an optional carriage return.
        /// </summary>
        /// <returns>\r?\n</returns>
        public static RegExBuilder NewlineWithOptionalCarriageReturn(this RegExBuilder regex) => regex.AddToStreamReturnQuantifiers(@"\r?\n");
        /// <summary>
        /// Matches any letter (A to Z), case insensitive.
        /// </summary>
        /// <returns>[a-zA-Z]</returns>
        public static RegExBuilder LetterCaseInsensitive(this RegExBuilder regex) => regex.AddToStreamReturnQuantifiers(@"[a-zA-Z]");
        /// <summary>
        /// Matches any upper case letter (A to Z).
        /// </summary>
        /// <param name="regex"></param>
        /// <returns>[A-Z]</returns>
        public static RegExBuilder LetterUpperCase(this RegExBuilder regex) => regex.AddToStreamReturnQuantifiers(@"[A-Z]");
        /// <summary>
        /// Matches any lower case letter (A to Z).
        /// </summary>
        /// <param name="regex"></param>
        /// <returns>[a-z]</returns>
        public static RegExBuilder LetterLowerCase(this RegExBuilder regex) => regex.AddToStreamReturnQuantifiers(@"[a-z]");
    }
}
