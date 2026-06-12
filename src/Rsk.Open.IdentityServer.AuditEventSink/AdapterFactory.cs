using System;
using System.Collections.Generic;
using Open.IdentityServer.Events;
using RSK.Audit;
using Rsk.Open.IdentityServer.AuditEventSink.Adapters;

namespace Rsk.Open.IdentityServer.AuditEventSink
{
    public class AdapterFactory : IAdapterFactory
    {
        private readonly Dictionary<Type, Func<Event, IAuditEventArguments>> eventAdapters;

        public AdapterFactory(IDictionary<Type, Func<Event, IAuditEventArguments>> customEventAdapters = null)
        {
            eventAdapters = CreateDefaultEventAdapters();

            if (customEventAdapters == null) return;

            foreach (var mapping in customEventAdapters)
            {
                eventAdapters[mapping.Key] = mapping.Value;
            }
        }

        public IAuditEventArguments Create(Event evt)
        {
            if (evt == null)
            {
                return null;
            }

            return eventAdapters.TryGetValue(evt.GetType(), out var adapterFactory)
                ? adapterFactory(evt)
                : null;
        }

        private static Dictionary<Type, Func<Event, IAuditEventArguments>> CreateDefaultEventAdapters()
        {
            return new Dictionary<Type, Func<Event, IAuditEventArguments>>
            {
                [typeof(TokenIssuedSuccessEvent)] = e => new TokenIssuedSuccessEventAdapter((TokenIssuedSuccessEvent)e),
                [typeof(UserLoginSuccessEvent)] = e => new UserLoginSuccessEventAdapter((UserLoginSuccessEvent)e),
                [typeof(UserLoginFailureEvent)] = e => new UserLoginFailureEventAdapter((UserLoginFailureEvent)e),
                [typeof(UserLogoutSuccessEvent)] = e => new UserLogoutSuccessEventAdapter((UserLogoutSuccessEvent)e),
                [typeof(ConsentGrantedEvent)] = e => new ConsentGrantedEventAdapter((ConsentGrantedEvent)e),
                [typeof(ConsentDeniedEvent)] = e => new ConsentDeniedEventAdapter((ConsentDeniedEvent)e),
                [typeof(TokenIssuedFailureEvent)] = e => new TokenIssuedFailureEventAdapter((TokenIssuedFailureEvent)e),
                [typeof(GrantsRevokedEvent)] = e => new GrantsRevokedEventAdapter((GrantsRevokedEvent)e),
                [typeof(DeviceAuthorizationFailureEvent)] = e => new DeviceAuthorizationFailureEventAdapter((DeviceAuthorizationFailureEvent)e),
                [typeof(DeviceAuthorizationSuccessEvent)] = e => new DeviceAuthorizationSuccessEventAdapter((DeviceAuthorizationSuccessEvent)e),
                [typeof(TokenRevokedSuccessEvent)] = e => new TokenRevokedSuccessEventAdapter((TokenRevokedSuccessEvent)e),
                [typeof(InvalidClientConfigurationEvent)] = e => new InvalidClientConfigurationEventAdapter((InvalidClientConfigurationEvent)e),
                [typeof(TokenIntrospectionFailureEvent)] = e => new TokenIntrospectionFailureEventAdapter((TokenIntrospectionFailureEvent)e),
                [typeof(TokenIntrospectionSuccessEvent)] = e => new TokenIntrospectionSuccessEventAdapter((TokenIntrospectionSuccessEvent)e),
                [typeof(ClientAuthenticationFailureEvent)] = e => new ClientAuthenticationFailureEventAdapter((ClientAuthenticationFailureEvent)e),
                [typeof(ClientAuthenticationSuccessEvent)] = e => new ClientAuthenticationSuccessEventAdapter((ClientAuthenticationSuccessEvent)e),
                [typeof(ApiAuthenticationFailureEvent)] = e => new ApiAuthenticationFailureEventAdapter((ApiAuthenticationFailureEvent)e),
                [typeof(ApiAuthenticationSuccessEvent)] = e => new ApiAuthenticationSuccessEventAdapter((ApiAuthenticationSuccessEvent)e),
                [typeof(UnhandledExceptionEvent)] = e => new UnhandledExceptionEventAdapter((UnhandledExceptionEvent)e)
            };
        }
    }
}
