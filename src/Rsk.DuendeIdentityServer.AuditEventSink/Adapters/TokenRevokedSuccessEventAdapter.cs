using System;
using Duende.IdentityServer.Events;
using RSK.Audit;

namespace Rsk.DuendeIdentityServer.AuditEventSink.Adapters
{
    public class TokenRevokedSuccessEventAdapter : IAuditEventArguments
    {
        private readonly TokenRevokedSuccessEvent evt;

        public TokenRevokedSuccessEventAdapter(TokenRevokedSuccessEvent evt)
        {
            this.evt = evt ?? throw new ArgumentNullException(nameof(evt));
        }

        public ResourceActor Actor => new ResourceActor(ResourceActor.MachineSubjectType, evt.ClientId, evt.ClientName);
        public string Action => evt.Name;
        public AuditableResource Resource => new AuditableResource("IdentityServer");
        public FormattedString Description => evt.ToString().SafeForFormatted();
    }
}
