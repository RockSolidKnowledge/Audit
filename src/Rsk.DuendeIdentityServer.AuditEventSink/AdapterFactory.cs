using Duende.IdentityServer.Events;
using RSK.Audit;
using Rsk.DuendeIdentityServer.AuditEventSink.Adapters;

namespace Rsk.DuendeIdentityServer.AuditEventSink
{
    public class AdapterFactory : IAdapterFactory
    {
        public IAuditEventArguments Create(Event evt)
        {
            if (evt != null)
            {
                switch (evt)
                {
                    case TokenIssuedSuccessEvent e:
                        return new TokenIssuedSuccessEventAdapter(e);
                    case UserLoginSuccessEvent e:
                        return new UserLoginSuccessEventAdapter(e);
                    case UserLoginFailureEvent e:
                        return new UserLoginFailureEventAdapter(e);
                    case UserLogoutSuccessEvent e:
                        return new UserLogoutSuccessEventAdapter(e);
                    case ConsentGrantedEvent e:
                        return new ConsentGrantedEventAdapter(e);
                    case ConsentDeniedEvent e:
                        return new ConsentDeniedEventAdapter(e);
                    case TokenIssuedFailureEvent e:
                        return new TokenIssuedFailureEventAdapter(e);
                    case GrantsRevokedEvent e:
                        return new GrantsRevokedEventAdapter(e);
                    case DeviceAuthorizationFailureEvent e:
                        return new DeviceAuthorizationFailureEventAdapter(e);
                    case DeviceAuthorizationSuccessEvent e:
                        return new DeviceAuthorizationSuccessEventAdapter(e);
                    case TokenRevokedSuccessEvent e:
                        return new TokenRevokedSuccessEventAdapter(e);
                    case InvalidClientConfigurationEvent e:
                        return new InvalidClientConfigurationEventAdapter(e);
                    case TokenIntrospectionFailureEvent e:
                        return new TokenIntrospectionFailureEventAdapter(e);
                    case TokenIntrospectionSuccessEvent e:
                        return new TokenIntrospectionSuccessEventAdapter(e);
                    case ClientAuthenticationFailureEvent e:
                        return new ClientAuthenticationFailureEventAdapter(e);
                    case ClientAuthenticationSuccessEvent e:
                        return new ClientAuthenticationSuccessEventAdapter(e);
                    case ApiAuthenticationFailureEvent e:
                        return new ApiAuthenticationFailureEventAdapter(e);
                    case ApiAuthenticationSuccessEvent e:
                        return new ApiAuthenticationSuccessEventAdapter(e);
                    case UnhandledExceptionEvent e:
                        return new UnhandledExceptionEventAdapter(e);
                    case BackchannelAuthenticationSuccessEvent e:
                        return new BackchannelAuthenticationSuccessEventAdapter(e);
                    case BackchannelAuthenticationFailureEvent e:
                              return new BackchannelAuthenticationFailureEventAdapter(e);
                    case InvalidIdentityProviderConfiguration e:
                        return new InvalidIdentityProviderConfigurationAdapter(e);
                }
            }

            return null;
        }
    }
}
