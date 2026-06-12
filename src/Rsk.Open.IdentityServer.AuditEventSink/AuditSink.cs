using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Open.IdentityServer.Events;
using Open.IdentityServer.Services;
using RSK.Audit;

[assembly:InternalsVisibleTo("RSK.Open.IdentityServer.AuditEventSink.Tests")]

namespace Rsk.Open.IdentityServer.AuditEventSink;

public class AuditSink(
    IRecordAuditableActions auditRecorder,
    IDictionary<Type, Func<Event, IAuditEventArguments>> customEventAdapters = null)
    : IEventSink
{
    private readonly IRecordAuditableActions auditRecorder = auditRecorder ?? throw new ArgumentNullException();

    internal IAdapterFactory Factory { get; init; } = new AdapterFactory(customEventAdapters);

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