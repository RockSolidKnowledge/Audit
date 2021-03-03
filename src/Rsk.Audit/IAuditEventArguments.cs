namespace RSK.Audit
{
  
    /// <summary>
    /// Represents an Audit Event to be recorded
    /// </summary>
    public interface  IAuditEventArguments   
    {
        /// <summary>
        /// The subject of the action
        /// </summary>
        ResourceActor Actor { get; }

        /// <summary>
        /// The action being performed
        /// </summary>
        string Action { get; }

        /// <summary>
        /// The resource the action is being applied to
        /// </summary>
        AuditableResource Resource { get; }

        /// <summary>
        /// A textual description of the audited event
        /// </summary>
        FormattedString Description { get; }

      
    }
}