using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IdentityServer4.Events;
using RSK.Audit;

namespace RSK.IdentityServer4.AuditEventSink.Adapters
{
    public class TokenIssuedSuccessEventAdapter : IAuditEventArguments
    {
        private readonly TokenIssuedSuccessEvent _evt;

        public TokenIssuedSuccessEventAdapter(TokenIssuedSuccessEvent evt)
        {
            _evt = evt ?? throw new ArgumentNullException(nameof(evt));
        }

        public ResourceActor Actor => new ResourceActor(ResourceActor.MachineSubjectType, _evt.ClientId, _evt.ClientName);
        public string Action => _evt.Name;
        public AuditableResource Resource => new AuditableResource("IdentityServer", _evt.Endpoint);
        public FormattedString Description => _evt.ToString().SafeForFormatted();
    }
}
