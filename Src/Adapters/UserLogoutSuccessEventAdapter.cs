using System;
using System.Collections.Generic;
using System.Text;
using IdentityServer4.Events;
using RSK.Audit;

namespace RSK.IdentityServer4.AuditEventSink.Adapters
{
    public class UserLogoutSuccessEventAdapter : IAuditEventArguments
    {
        private readonly UserLogoutSuccessEvent _evt;

        public UserLogoutSuccessEventAdapter(UserLogoutSuccessEvent evt)
        {
            _evt = evt;
        }

        public ResourceActor Actor => new ResourceActor(ResourceActor.UserSubjectType, _evt.SubjectId, _evt.DisplayName);
        public string Action => _evt.Name;
        public AuditableResource Resource => new AuditableResource("IdentityServer", string.Empty);
        public FormattedString Description => _evt.ToString().SafeForFormatted();
    }
}
