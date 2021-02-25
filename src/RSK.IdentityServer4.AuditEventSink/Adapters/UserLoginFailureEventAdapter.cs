using System;
using IdentityServer4.Events;
using RSK.Audit;

namespace RSK.IdentityServer4.AuditEventSink.Adapters
{
    public class UserLoginFailureEventAdapter : IAuditEventArguments
    {
        private readonly UserLoginFailureEvent evt;

        public UserLoginFailureEventAdapter(UserLoginFailureEvent evt)
        {
            this.evt = evt ?? throw new ArgumentNullException(nameof(evt));
        }

        public ResourceActor Actor => new ResourceActor(ResourceActor.UserSubjectType, evt.Username, evt.Username);
        public string Action => evt.Name;
        public AuditableResource Resource => new AuditableResource("IdentityServer", evt.Endpoint);
        public FormattedString Description => evt.ToString().SafeForFormatted();
    }
}
