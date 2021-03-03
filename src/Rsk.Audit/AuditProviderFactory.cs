namespace RSK.Audit
{

    /// <summary>
    /// Used to create an Audit Source to generate audit events from, or to create an initial audit query to find audit events
    /// </summary>
    public abstract class  AuditProviderFactory
    {
        /// <summary>
        /// Creates an IRecordAuditableActions object used to generate events from the specified event source
        /// </summary>
        /// <param name="source">name of the event source</param>
        /// <returns>The event source used to record events from this source</returns>
        public abstract IRecordAuditableActions CreateAuditSource(string source);

        /// <summary>
        /// Creates an initial query object as a starting point to query historic audit events
        /// </summary>
        /// <returns>Initial query object</returns>
        public abstract IQueryableAuditableActions CreateAuditQuery();
    }
}