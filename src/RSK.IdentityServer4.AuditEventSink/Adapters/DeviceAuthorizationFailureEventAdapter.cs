using System;
using IdentityServer4.Events;
using RSK.Audit;

namespace RSK.IdentityServer4.AuditEventSink.Adapters
{
    public class DeviceAuthorizationFailureEventAdapter : IAuditEventArguments
    {
        private readonly DeviceAuthorizationFailureEvent evt;

        public DeviceAuthorizationFailureEventAdapter(DeviceAuthorizationFailureEvent evt)
        {
            this.evt = evt ?? throw new ArgumentNullException(nameof(evt));
        }

        public ResourceActor Actor => new ResourceActor(ResourceActor.UserSubjectType, evt.ClientId, evt.ClientName);
        public string Action => evt.Name;
        public AuditableResource Resource => new AuditableResource("IdentityServer", evt.ClientId);
        public FormattedString Description => evt.ToString().SafeForFormatted();
    }
}
