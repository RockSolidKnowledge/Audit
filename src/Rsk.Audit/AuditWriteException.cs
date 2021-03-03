using System;

namespace RSK.Audit
{
    /// <summary>
    /// Represents a failure to commit the audit record
    /// </summary>
    public class AuditWriteException : Exception
    {
        public AuditWriteException(string reason) : base(reason)
        {
            
        }

        public AuditWriteException(string reason, Exception innerException) : base(reason,innerException)
        {
                
        }

    }
}