using System;
using IdentityServer4.Events;
using RSK.Audit;

namespace RSK.IdentityServer4.AuditEventSink.Adapters
{
    public class GrantsRevokedEventAdapter : IAuditEventArguments
    {
        private readonly GrantsRevokedEvent evt;

        public GrantsRevokedEventAdapter(GrantsRevokedEvent evt)
        {
            this.evt = evt ?? throw new ArgumentNullException(nameof(evt));
        }
        public ResourceActor Actor => new ResourceActor(ResourceActor.UserSubjectType, evt.SubjectId, evt.SubjectId);
        public string Action => evt.Name;
        public AuditableResource Resource => new AuditableResource("Client", evt.ClientId);
        public FormattedString Description => evt.ToString().SafeForFormatted();
    }
}
