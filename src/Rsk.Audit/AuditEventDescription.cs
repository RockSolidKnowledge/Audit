namespace RSK.Audit
{
    
    /// <summary>
    /// Used to describe an Audit Event to be recorded
    /// </summary>
    public class AuditEventArguments : IAuditEventArguments
    {
        /// <summary>
        /// Creates an instance of an Audit Event to be recorded
        /// </summary>
        /// <param name="actor">The subject responsible for the event</param>
        /// <param name="action">Action being performed</param>
        /// <param name="resource">Resource the action is being applied to</param>
        /// <param name="description">Description of the event</param>
        public AuditEventArguments(ResourceActor actor , string action , AuditableResource resource ,FormattedString description)
        {
            Actor = actor;
            Action = action;
            Resource = resource;
            Description = description;
        }

     
        /// <summary>
        /// Who performed the action
        /// </summary>
        public ResourceActor Actor { get; }
        /// <summary>
        /// The action
        /// </summary>
        public string Action { get; }
        /// <summary>
        /// The resource the action is being applied to
        /// </summary>
        public AuditableResource Resource { get; }
        /// <summary>
        /// Description of the audit event
        /// </summary>
        public  FormattedString Description { get; }
      
    }

    
}