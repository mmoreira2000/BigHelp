using System.Collections.Generic;

namespace BigHelp.FluentRegEx
{
    public class UnicodeData
    {
        internal UnicodeData(string code)
        {
            Code = code;
        }
        public string Code { get; }

        public override string ToString()
        {
            return Code;
        }

        #region Equality Members
        protected bool Equals(UnicodeData other)
        {
            return Code == other.Code;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UnicodeData)obj);
        }

        public override int GetHashCode()
        {
            return (Code != null ? Code.GetHashCode() : 0);
        }
        #endregion
    }
}