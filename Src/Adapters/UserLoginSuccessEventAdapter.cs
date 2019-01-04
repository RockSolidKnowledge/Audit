using System;
using System.Collections.Generic;
using System.Text;
using IdentityServer4.Events;
using RSK.Audit;

namespace RSK.IdentityServer4.AuditEventSink.Adapters
{
    public class UserLoginSuccessEventAdapter : IAuditEventArguments
    {
        private readonly UserLoginSuccessEvent _evt;

        public UserLoginSuccessEventAdapter(UserLoginSuccessEvent evt)
        {
            _evt = evt ?? throw new ArgumentNullException(nameof(evt));
        }

        public ResourceActor Actor => new ResourceActor(ResourceActor.UserSubjectType, _evt.SubjectId, _evt.DisplayName);

        public string Action => _evt.Name;

        public AuditableResource Resource => new AuditableResource("IdentityServer", _evt.Endpoint);

        public FormattedString Description => _evt.ToString().SafeForFormatted();
    }
}
