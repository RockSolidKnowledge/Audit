using System;
using Duende.IdentityServer.Events;
using RSK.Audit;

namespace Rsk.DuendeIdentityServer.AuditEventSink.Adapters
{
    public class InvalidIdentityProviderConfigurationAdapter : IAuditEventArguments
    {
        private readonly InvalidIdentityProviderConfiguration evt;

        public InvalidIdentityProviderConfigurationAdapter(InvalidIdentityProviderConfiguration evt)
        {
            this.evt = evt ?? throw new ArgumentNullException(nameof(evt));
        }

        public ResourceActor Actor => new ResourceActor(ResourceActor.MachineSubjectType, evt.Name, evt.DisplayName);
        public string Action => evt.Name;
        public AuditableResource Resource => new AuditableResource("IdentityServer", evt.Type);
        public FormattedString Description => evt.ToString().SafeForFormatted();
    }
}
