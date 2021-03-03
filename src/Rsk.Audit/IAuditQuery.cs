using System.Threading.Tasks;

namespace RSK.Audit
{

    /// <summary>
    /// Matching styles for strings
    /// </summary>
    public enum Matches
    {
        Exactly,
        StartsWith,
        EndsWith,  // WARNING  could result in poor performance on large date ranges
        SomethingLike // WARNING  could result in poor performance on large date ranges
    }

    /// <summary>
    /// How to sort the query results
    /// </summary>
    public enum AuditQuerySort
    {
        When,
        Subject,
        Source,
        Action,
        Description,
        Success
    };

    /// <summary>
    /// Sort direction of the query results.
    /// </summary>
    public enum AuditQuerySortDirection
    {
        Ascending = 0,
        Descending = 1
    };

    /// <summary>
    /// A Audit Event Query
    /// </summary>
    public interface IAuditQuery
    {

        /// <summary>
        /// The query result should match the supplied subject
        /// </summary>
        /// <param name="resourceActor">Subject of the audit actions</param>
        /// <returns>A new query</returns>
        IAuditQuery AndSubject(ResourceActor resourceActor);

        /// <summary>
        /// The query result should match the supplied subject
        /// </summary>
        /// <param name="matchType">String match type to use</param>
        /// <param name="nameOrIdentifier">the identifier or name of the subject to match</param>
        /// <returns>A new query</returns>
        IAuditQuery AndSubject(Matches matchType,string nameOrIdentifier);

        /// <summary>
        /// The query result should match the supplied resource
        /// </summary>
        /// <param name="matchType">String match type to use</param>
        /// <param name="nameOrIdentifier">the identifier or name of the resource match</param>
        /// <returns>A new query</returns>
        IAuditQuery AndResource(Matches matchType, string nameOrIdentifier);
        /// <summary>
        /// The query result should match the supplied resource
        /// </summary>
        /// <param name="resource">Resource to match on</param>
        /// <returns>A new query</returns>
        IAuditQuery AndResource(AuditableResource resource);
        /// <summary>
        /// The query result should match the supplied subject
        /// </summary>
        /// <param name="matchType">String match type to use</param>
        /// <param name="subjectIdentifier">the identifier of the subject to match</param>
        /// <returns>A new query</returns>
        IAuditQuery AndSubjectIdentifier(Matches matchType, string subjectIdentifier);
        /// <summary>
        /// The query result should match the supplied subject
        /// </summary>
        /// <param name="matchType">String match type to use</param>
        /// <param name="subjectType">the identifier of the subject  type to match</param>
        /// <returns>A new query</returns>
        IAuditQuery AndSubjectType(Matches matchType, string subjectType);
        /// <summary>
        /// The query result should match the supplied subject
        /// </summary>
        /// <param name="matchType">String match type to use</param>
        /// <param name="subject">the name of the subject to match</param>
        /// <returns>A new query</returns>
        IAuditQuery AndSubjectKnownAs(Matches matchType, string subject);


        /// <summary>
        /// The query result should match the supplied source
        /// </summary>
        /// <param name="matchType">String match type to use</param>
        /// <param name="source">the source of the audit event to match</param>
        /// <returns>A new query</returns>
        IAuditQuery AndSource(Matches matchType, string source);
        /// <summary>
        /// The query result should match the supplied action
        /// </summary>
        /// <param name="matchType">String match type to use</param>
        /// <param name="action">the action of the audit event to match</param>
        /// <returns>A new query</returns>
        IAuditQuery AndAction(Matches matchType, string action);
        /// <summary>
        /// The query result should match the supplied resource Identifier
        /// </summary>
        /// <param name="matchType">String match type to use</param>
        /// <param name="resourceIdentifier">the resource Identifier of the audit event to match</param>
        /// <returns>A new query</returns>
        IAuditQuery AndResourceIdentifier(Matches matchType, string resourceIdentifier);
        /// <summary>
        /// The query result should match the supplied resource type
        /// </summary>
        /// <param name="matchType">String match type to use</param>
        /// <param name="resourceType">the resource type of the audit event to match</param>
        /// <returns>A new query</returns>
        IAuditQuery AndResourceType(Matches matchType, string resourceType);
        /// <summary>
        /// The query result should match the supplied description
        /// </summary>
        /// <param name="matchType">String match type to use</param>
        /// <param name="description">the desc   of the audit event to match</param>
        /// <returns>A new query</returns>
        IAuditQuery AndDescription(Matches matchType, string description);
        /// <summary>
        /// The query result should match any of the following ( subject , description , action , resource , source )
        /// </summary>
        /// <param name="matchType">String match type to use</param>
        /// <param name="text">the text of the audit event to match</param>
        /// <returns>A new query</returns>
        IAuditQuery AndAnyText(Matches matchType, string text);

        /// <summary>
        /// The query result should match only succeeded events
        /// </summary>
        /// <returns>A new query</returns>
        IAuditQuery AndActionSucceeded();
        /// <summary>
        /// The query result should match only failed events
        /// </summary>
        /// <returns>A new query</returns>
        IAuditQuery AndActionFailed();
        
        /// <summary>
        /// Executes the query sorted date ascending
        /// </summary>
        /// <returns>The results</returns>
        Task<IAuditQueryResult> ExecuteAscending();
        /// <summary>
        /// Executes the query sorted date descending
        /// </summary>
        /// <returns>The results</returns>
        Task<IAuditQueryResult> ExecuteDescending();

        /// <summary>
        /// Executes the query sorted by the supplied column ascending
        /// </summary>
        /// <param name="sortColumn">Column to sort by</param>
        /// <returns>The results</returns>
        Task<IAuditQueryResult> ExecuteAscending(AuditQuerySort sortColumn);
        /// <summary>
        /// Executes the query sorted by the supplied column descending
        /// </summary>
        /// <param name="sortColumn">Column to sort by</param>
        /// <returns>The results</returns>
        Task<IAuditQueryResult> ExecuteDescending(AuditQuerySort sortColumn);

    }
}