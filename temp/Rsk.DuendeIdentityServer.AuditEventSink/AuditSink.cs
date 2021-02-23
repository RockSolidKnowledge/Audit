using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;
using RSK.Audit;

[assembly:InternalsVisibleTo("RSK.DuendeIdentityServer.AuditEventSink.Tests")]

namespace Rsk.DuendeIdentityServer.AuditEventSink
{
    public class AuditSink : IEventSink
    {
        private readonly IRecordAuditableActions auditRecorder;

        internal IAdapterFactory Factory { get; set; } = new AdapterFactory();

        public AuditSink(IRecordAuditableActions auditRecorder)
        {
            this.auditRecorder = auditRecorder ?? throw new ArgumentNullException();
        }

        public Task PersistAsync(Event evt)
        {
            var auditArgument = Factory.Create(evt);

            if (auditArgument != null)
            {
                if (evt.EventType == EventTypes.Success || evt.EventType == EventTypes.Information)
                {
                    return auditRecorder.RecordSuccess(auditArgument);
                }

                return auditRecorder.RecordFailure(auditArgument);
            }

            return Task.CompletedTask;
        }
    }
}
