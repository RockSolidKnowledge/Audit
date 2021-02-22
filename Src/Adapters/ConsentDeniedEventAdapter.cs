using System;
using IdentityServer4.Events;
using RSK.Audit;

namespace RSK.IdentityServer4.AuditEventSink.Adapters
{
    public class ConsentDeniedEventAdapter : IAuditEventArguments
    {
        private readonly ConsentDeniedEvent _evt;

        public ConsentDeniedEventAdapter(ConsentDeniedEvent evt)
        {
            _evt = evt ?? throw new ArgumentNullException(nameof(evt));
        }

        public ResourceActor Actor => new ResourceActor(ResourceActor.MachineSubjectType, _evt.ClientId, _evt.ClientId);
        public string Action => _evt.Name;
        public AuditableResource Resource => new AuditableResource("User", _evt.SubjectId);
        public FormattedString Description => _evt.ToString().SafeForFormatted();
    }
}
