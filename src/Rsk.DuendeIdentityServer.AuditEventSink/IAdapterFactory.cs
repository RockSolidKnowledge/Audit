using Duende.IdentityServer.Events;
using RSK.Audit;

namespace Rsk.DuendeIdentityServer.AuditEventSink
{
    public interface IAdapterFactory
    {
        IAuditEventArguments Create(Event evt);
    }
}