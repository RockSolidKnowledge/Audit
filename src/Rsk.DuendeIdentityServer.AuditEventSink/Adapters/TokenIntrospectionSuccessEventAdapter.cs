using System;
using Duende.IdentityServer.Events;
using RSK.Audit;

namespace Rsk.DuendeIdentityServer.AuditEventSink.Adapters
{
    public class TokenIntrospectionSuccessEventAdapter : IAuditEventArguments
    {
        private readonly TokenIntrospectionSuccessEvent evt;

        public TokenIntrospectionSuccessEventAdapter(TokenIntrospectionSuccessEvent evt)
        {
            this.evt = evt ?? throw new ArgumentNullException(nameof(evt));
        }

        public ResourceActor Actor => new ResourceActor(ResourceActor.MachineSubjectType, null, null);
        public string Action => evt.Name;
        public AuditableResource Resource => new AuditableResource("IdentityServer", evt.ApiName);
        public FormattedString Description => evt.ToString().SafeForFormatted();
    }
}
