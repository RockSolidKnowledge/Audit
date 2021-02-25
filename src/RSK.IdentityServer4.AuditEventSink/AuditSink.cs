using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using IdentityServer4.Events;
using IdentityServer4.Services;
using RSK.Audit;

[assembly:InternalsVisibleTo("RSK.IdentityServer4.AuditEventSink.Tests")]

namespace RSK.IdentityServer4.AuditEventSink
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
