using System;
using System.Collections.Generic;
using System.Text;
using IdentityServer4.Events;
using RSK.Audit;

namespace RSK.IdentityServer4.AuditEventSink.Adapters
{
    public class GrantsRevokedEventAdapter : IAuditEventArguments
    {
        private readonly GrantsRevokedEvent _evt;

        public GrantsRevokedEventAdapter(GrantsRevokedEvent evt)
        {
            _evt = evt ?? throw new ArgumentNullException(nameof(evt));
        }
        public ResourceActor Actor => new ResourceActor(ResourceActor.UserSubjectType, _evt.SubjectId, _evt.SubjectId);
        public string Action => _evt.Name;
        public AuditableResource Resource => new AuditableResource("Client", _evt.ClientId);
        public FormattedString Description => _evt.ToString().SafeForFormatted();
    }
}
