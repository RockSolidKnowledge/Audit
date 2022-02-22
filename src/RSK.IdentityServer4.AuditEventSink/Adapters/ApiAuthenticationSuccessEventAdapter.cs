using System;
using IdentityServer4.Events;
using RSK.Audit;

namespace RSK.IdentityServer4.AuditEventSink.Adapters
{
    public class ApiAuthenticationSuccessEventAdapter : IAuditEventArguments
    {
        private readonly ApiAuthenticationSuccessEvent evt;

        public ApiAuthenticationSuccessEventAdapter(ApiAuthenticationSuccessEvent evt)
        {
            this.evt = evt ?? throw new ArgumentNullException(nameof(evt));
        }

        public ResourceActor Actor => new ResourceActor(ResourceActor.UserSubjectType, evt.ActivityId, evt.ApiName);
        public string Action => evt.Name;
        public AuditableResource Resource => new AuditableResource("IdentityServer", evt.Message);
        public FormattedString Description => evt.ToString().SafeForFormatted();
    }
}
