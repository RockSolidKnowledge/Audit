using System;
using System.Collections.Generic;
using System.Text;
using IdentityServer4.Events;
using RSK.Audit;

namespace RSK.IdentityServer4.AuditEventSink.Adapters
{
    public class ConsentGrantedEventAdapter : IAuditEventArguments
    {
        private readonly ConsentGrantedEvent _evt;

        public ConsentGrantedEventAdapter(ConsentGrantedEvent evt)
        {
            _evt = evt;
        }

        public ResourceActor Actor => new ResourceActor(ResourceActor.MachineSubjectType, _evt.ClientId, _evt.ClientId);
        public string Action => _evt.Name;
        public AuditableResource Resource => new AuditableResource("User", _evt.SubjectId);
        public FormattedString Description => _evt.ToString().SafeForFormatted();
    }
}
