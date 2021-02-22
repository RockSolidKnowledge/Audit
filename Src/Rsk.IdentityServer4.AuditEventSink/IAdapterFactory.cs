using IdentityServer4.Events;
using RSK.Audit;

namespace RSK.IdentityServer4.AuditEventSink
{
    public interface IAdapterFactory
    {
        IAuditEventArguments Create(Event evt);
    }
}