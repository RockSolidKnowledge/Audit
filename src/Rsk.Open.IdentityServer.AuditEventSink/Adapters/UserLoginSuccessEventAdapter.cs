using System;
using Open.IdentityServer.Events;
using RSK.Audit;

namespace Rsk.Open.IdentityServer.AuditEventSink.Adapters
{
    public class UserLoginSuccessEventAdapter : IAuditEventArguments
    {
        private readonly UserLoginSuccessEvent evt;

        public UserLoginSuccessEventAdapter(UserLoginSuccessEvent evt)
        {
            this.evt = evt ?? throw new ArgumentNullException(nameof(evt));
        }

        public ResourceActor Actor => new ResourceActor(ResourceActor.UserSubjectType, evt.SubjectId, evt.DisplayName);

        public string Action => evt.Name;

        public AuditableResource Resource => new AuditableResource("IdentityServer", evt.Endpoint);

        public FormattedString Description => evt.ToString().SafeForFormatted();
    }
}
