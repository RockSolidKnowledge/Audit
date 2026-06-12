using System;
using Open.IdentityServer.Events;
using RSK.Audit;

namespace Rsk.Open.IdentityServer.AuditEventSink.Adapters
{
    public class ConsentDeniedEventAdapter : IAuditEventArguments
    {
        private readonly ConsentDeniedEvent evt;

        public ConsentDeniedEventAdapter(ConsentDeniedEvent evt)
        {
            this.evt = evt ?? throw new ArgumentNullException(nameof(evt));
        }

        public ResourceActor Actor => new ResourceActor(ResourceActor.UserSubjectType, evt.SubjectId, evt.SubjectId);
        public string Action => evt.Name;
        public AuditableResource Resource => new AuditableResource("Client", evt.ClientId);
        public FormattedString Description => evt.ToString().SafeForFormatted();
    }
}
