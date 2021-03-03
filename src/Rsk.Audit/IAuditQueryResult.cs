using System.Collections.Generic;

namespace RSK.Audit
{
    /// <summary>
    /// The results of an audit events query
    /// </summary>
    public interface IAuditQueryResult
    {
        /// <summary>
        /// Matching rows
        /// </summary>
        IEnumerable<IAuditEntry> Rows { get; }

        /// <summary>
        /// The page number of the results
        /// </summary>
        int Page { get; }

        /// <summary>
        /// Total number of pages in the query
        /// </summary>
        int TotalNumberOfPages { get; }

        /// <summary>
        /// Total number of rows in the query
        /// </summary>
        long TotalNumberOfRows { get; }
    }
}