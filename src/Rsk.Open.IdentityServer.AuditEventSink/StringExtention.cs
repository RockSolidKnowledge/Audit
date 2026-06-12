namespace Rsk.Open.IdentityServer.AuditEventSink
{
    public static class StringExtension
    {
        public static string SafeForFormatted(this string value)
        {
            return value.Replace("{", "{{").Replace("}", "}}");
        }
    }
}
