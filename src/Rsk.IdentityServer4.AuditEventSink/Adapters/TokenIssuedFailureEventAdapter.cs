using System;
using IdentityServer4.Events;
using RSK.Audit;

namespace RSK.IdentityServer4.AuditEventSink.Adapters
{
    public class TokenIssuedFailureEventAdapter : IAuditEventArguments
    {
        private readonly TokenIssuedFailureEvent evt;

        public TokenIssuedFailureEventAdapter(TokenIssuedFailureEvent evt)
        {
            this.evt = evt ?? throw new ArgumentNullException(nameof(evt));
        }

        public ResourceActor Actor => new ResourceActor(ResourceActor.MachineSubjectType, evt.ClientId, evt.ClientName);
        public string Action => evt.Name;
        public AuditableResource Resource => new AuditableResource("IdentityServer", evt.Endpoint);
        public FormattedString Description => evt.ToString().SafeForFormatted();
    }
}
