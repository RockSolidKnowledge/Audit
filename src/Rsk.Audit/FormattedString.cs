using System;

namespace RSK.Audit
{
    /// <summary>
    /// Represents a formatting template, and the parts to be placed into the template
    /// 
    /// </summary>
    public class FormattedString
    {
        private readonly string format;
        private readonly object[] args;

        public FormattedString(string format , params object[] args)
        {
            this.format = format;
            this.args = args;
        }

        public override string ToString()
        {
            return String.Format(format, args);
        }

        public string Format => format;
        public object[] Arguments => args;

        public static implicit operator FormattedString(string format)
        {
            return new FormattedString(format);
        }
    }
}