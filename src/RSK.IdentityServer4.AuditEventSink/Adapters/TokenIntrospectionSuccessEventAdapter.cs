using System;
using IdentityServer4.Events;
using RSK.Audit;

namespace RSK.IdentityServer4.AuditEventSink.Adapters
{
    public class TokenIntrospectionSuccessEventAdapter : IAuditEventArguments
    {
        private readonly TokenIntrospectionSuccessEvent evt;

        public TokenIntrospectionSuccessEventAdapter(TokenIntrospectionSuccessEvent evt)
        {
            this.evt = evt ?? throw new ArgumentNullException(nameof(evt));
        }

        public ResourceActor Actor => new ResourceActor(ResourceActor.UserSubjectType, evt.ActivityId, evt.ApiName);
        public string Action => evt.Name;
        public AuditableResource Resource => new AuditableResource("IdentityServer", evt.ActivityId);
        public FormattedString Description => evt.ToString().SafeForFormatted();
    }
}
