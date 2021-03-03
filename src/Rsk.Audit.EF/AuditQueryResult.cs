using System.Collections.Generic;

namespace RSK.Audit.EF
{
    /// <summary>
    /// A result of an audit event query
    /// </summary>
    internal class AuditQueryResult : IAuditQueryResult
    {
        public AuditQueryResult(IEnumerable<IAuditEntry> rows , long totalNumberOfRows)
        {
            Rows = rows;
            TotalNumberOfRows = totalNumberOfRows;
        }

        
        public IEnumerable<IAuditEntry> Rows { get; }
        public long TotalNumberOfRows { get; }
        public int Page { get; set; }
        public int TotalNumberOfPages { get; set; }
    }
}