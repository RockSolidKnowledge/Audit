using System;

namespace RSK.Audit
{
    public interface IAuditEntry
    {
        /// <summary>
        /// When the audit entry occured
        /// </summary>
        DateTime When { get; }
        /// <summary>
        /// The source of the audit entry, typically an application
        /// </summary>
        string Source { get; }
        
        /// <summary>
        /// Who generated the action, typically a user
        /// </summary>
        ResourceActor Subject { get; }
        
        /// <summary>
        /// The action being audited, e.g. Login, Logout , Add User
        /// </summary>
        string Action { get; }
        
        /// <summary>
        /// The resource the action is applied to, e.g. Action=Login, Resource=https://ids.acme.com
        /// </summary>
        AuditableResource Resource { get; }

        /// <summary>
        /// Was the action successful
        /// </summary>
        bool Succeeded { get; }

        /// <summary>
        /// Additional information about the action, e.g. reason why it failed or User fred added
        /// </summary>
        string Description { get; }
    }
}