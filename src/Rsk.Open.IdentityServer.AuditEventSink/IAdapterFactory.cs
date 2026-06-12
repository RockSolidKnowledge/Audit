using Open.IdentityServer.Events;
using RSK.Audit;

namespace Rsk.Open.IdentityServer.AuditEventSink
{
    public interface IAdapterFactory
    {
        IAuditEventArguments Create(Event evt);
    }
}