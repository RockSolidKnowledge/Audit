using System;
using Duende.IdentityServer.Events;
using RSK.Audit;

namespace Rsk.DuendeIdentityServer.AuditEventSink.Adapters
{
    public class ClientAuthenticationFailureEventAdapter : IAuditEventArguments
    {
        private readonly ClientAuthenticationFailureEvent evt;

        public ClientAuthenticationFailureEventAdapter(ClientAuthenticationFailureEvent evt)
        {
            this.evt = evt ?? throw new ArgumentNullException(nameof(evt));
        }

        public ResourceActor Actor => new ResourceActor(ResourceActor.MachineSubjectType, evt.ClientId, evt.ClientId);
        public string Action => evt.Name;
        public AuditableResource Resource => new AuditableResource("IdentityServer");
        public FormattedString Description => evt.ToString().SafeForFormatted();
    }
}
