using System;
using System.Collections.Generic;
using System.Text;

namespace RSK.IdentityServer4.AuditEventSink
{
    public static class StringExtension
    {
        public static string SafeForFormatted(this string value)
        {
            return value.Replace("{", "{{").Replace("}", "}}");
        }
    }
}
