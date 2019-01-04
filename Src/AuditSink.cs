using System;
using System.Collections.Generic;
using System.Reflection;
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
        private readonly IRecordAuditableActions _auditRecorder;

        internal IAdapterFactory Factory { get; set; } = new AdapterFactory();

        public AuditSink(IRecordAuditableActions auditRecorder)
        {
            _auditRecorder = auditRecorder ?? throw new ArgumentNullException();
        }

        public Task PersistAsync(Event evt)
        {
            var auditArgument = Factory.Create(evt);

            if (auditArgument != null)
            {
                if (evt.EventType == EventTypes.Success || evt.EventType == EventTypes.Information)
                {
                    return _auditRecorder.RecordSuccess(auditArgument);
                }

                return _auditRecorder.RecordFailure(auditArgument);
            }

            return Task.CompletedTask;
        }
    }
}
