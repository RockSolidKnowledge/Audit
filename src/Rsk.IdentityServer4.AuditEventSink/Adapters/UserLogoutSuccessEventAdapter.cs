using IdentityServer4.Events;
using RSK.Audit;

namespace RSK.IdentityServer4.AuditEventSink.Adapters
{
    public class UserLogoutSuccessEventAdapter : IAuditEventArguments
    {
        private readonly UserLogoutSuccessEvent evt;

        public UserLogoutSuccessEventAdapter(UserLogoutSuccessEvent evt)
        {
            this.evt = evt;
        }

        public ResourceActor Actor => new ResourceActor(ResourceActor.UserSubjectType, evt.SubjectId, evt.DisplayName);
        public string Action => evt.Name;
        public AuditableResource Resource => new AuditableResource("IdentityServer", string.Empty);
        public FormattedString Description => evt.ToString().SafeForFormatted();
    }
}
