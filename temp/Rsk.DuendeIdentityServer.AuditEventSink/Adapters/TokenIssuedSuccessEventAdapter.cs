using System;
using Duende.IdentityServer.Events;
using RSK.Audit;

namespace Rsk.DuendeIdentityServer.AuditEventSink.Adapters
{
    public class TokenIssuedSuccessEventAdapter : IAuditEventArguments
    {
        private readonly TokenIssuedSuccessEvent evt;

        public TokenIssuedSuccessEventAdapter(TokenIssuedSuccessEvent evt)
        {
            this.evt = evt ?? throw new ArgumentNullException(nameof(evt));
        }

        public ResourceActor Actor => new ResourceActor(ResourceActor.MachineSubjectType, evt.ClientId, evt.ClientName);
        public string Action => evt.Name;
        public AuditableResource Resource => new AuditableResource("IdentityServer", evt.Endpoint);
        public FormattedString Description => evt.ToString().SafeForFormatted();
    }
}
