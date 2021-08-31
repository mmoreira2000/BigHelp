using System;
using System.Collections.Generic;
using System.Text;

namespace BigHelp.FluentRegEx
{
    public class FluentRegExBuilderException : SystemException
    {
        public FluentRegExBuilderException() { }
        public FluentRegExBuilderException(string message) : base(message) { }
        public FluentRegExBuilderException(string message, Exception innerException) : base(message, innerException) { }
    }
}
