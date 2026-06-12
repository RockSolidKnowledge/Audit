using System;
using Open.IdentityServer.Events;
using RSK.Audit;

namespace Rsk.Open.IdentityServer.AuditEventSink.Adapters
{
    public class ApiAuthenticationFailureEventAdapter : IAuditEventArguments
    {
        private readonly ApiAuthenticationFailureEvent evt;

        public ApiAuthenticationFailureEventAdapter(ApiAuthenticationFailureEvent evt)
        {
            this.evt = evt ?? throw new ArgumentNullException(nameof(evt));
        }

        public ResourceActor Actor => new ResourceActor(ResourceActor.MachineSubjectType, null, null);
        public string Action => evt.Name;
        public AuditableResource Resource => new AuditableResource("IdentityServer", evt.ApiName);
        public FormattedString Description => evt.ToString().SafeForFormatted();
    }
}
