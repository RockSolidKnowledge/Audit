using System;

namespace RSK.Audit
{
    /// <summary>
    /// Used to define the range of audit records to query
    /// </summary>
    public interface IQueryableAuditableActions
    {
        /// <summary>
        /// Defines the base query for audit records
        /// </summary>
        /// <param name="from">The date/time from which to find audit records from</param>
        /// <param name="to">The date/time from which to find audit records upto</param>
        /// <returns>A query that will return all the audit records between the defined dates</returns>
        IAuditQuery Between(DateTime from, DateTime to);

        /// <summary>
        /// Defines the base query for audit records
        /// </summary>
        /// <param name="from">The date/time from which to find audit records from</param>
        /// <param name="to">The date/time from which to find audit records upto</param>
        /// <param name="page">The page of results to fetch, starting from 1</param>
        /// <param name="pageSize">The number of results per page</param>
        /// <returns>A query that will return a page of audit records between the defined dates</returns>
        IAuditQuery Between(DateTime from, DateTime to, int page , int pageSize);
    }

    
}

/*
 * var query = source.Between( DateTime.Now.AddDays(-1),DateTime.Now,page:1,pageSize:25)
 *                   .AndSubject( Matches.Exactly , "andy" )
 *                   .AndDescription(Matches.SomethingLike , "logged in")
 *                   .AndActionSucceeded()
 *                   .ExecuteAscending(AuditQuerySort.Source)
 * 
 * 
 * 
 */
