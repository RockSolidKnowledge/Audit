namespace Rsk.DuendeIdentityServer.AuditEventSink
{
    public static class StringExtension
    {
        public static string SafeForFormatted(this string value)
        {
            return value.Replace("{", "{{").Replace("}", "}}");
        }
    }
}
