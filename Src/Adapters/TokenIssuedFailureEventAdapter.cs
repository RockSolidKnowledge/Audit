using System;
using IdentityServer4.Events;
using RSK.Audit;

namespace RSK.IdentityServer4.AuditEventSink.Adapters
{
    public class TokenIssuedFailureEventAdapter : IAuditEventArguments
    {
        private readonly TokenIssuedFailureEvent _evt;

        public TokenIssuedFailureEventAdapter(TokenIssuedFailureEvent evt)
        {
            _evt = evt ?? throw new ArgumentNullException(nameof(evt));
        }

        public ResourceActor Actor => new ResourceActor(ResourceActor.MachineSubjectType, _evt.ClientId, _evt.ClientName);
        public string Action => _evt.Name;
        public AuditableResource Resource => new AuditableResource("IdentityServer", _evt.Endpoint);
        public FormattedString Description => _evt.ToString().SafeForFormatted();
    }
}
