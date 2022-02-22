using IdentityServer4.Events;
using RSK.Audit;
using RSK.IdentityServer4.AuditEventSink.Adapters;

namespace RSK.IdentityServer4.AuditEventSink
{
    public class AdapterFactory : IAdapterFactory
    {
        public IAuditEventArguments Create(Event evt)
        {
            if (evt != null)
            {
                switch (evt)
                {
                    case TokenIssuedSuccessEvent tokenIssuedSuccessEvent:
                        return new TokenIssuedSuccessEventAdapter(tokenIssuedSuccessEvent);
                    case UserLoginSuccessEvent userLoginSuccess:
                        return new UserLoginSuccessEventAdapter(userLoginSuccess);
                    case UserLoginFailureEvent userLoginFailure:
                        return new UserLoginFailureEventAdapter(userLoginFailure);
                    case UserLogoutSuccessEvent userLogoutSuccess:
                        return new UserLogoutSuccessEventAdapter(userLogoutSuccess);
                    case ConsentGrantedEvent consentGranted:
                        return new ConsentGrantedEventAdapter(consentGranted);
                    case ConsentDeniedEvent consentDenied:
                        return new ConsentDeniedEventAdapter(consentDenied);
                    case TokenIssuedFailureEvent tokenIssuedFailure:
                        return new TokenIssuedFailureEventAdapter(tokenIssuedFailure);
                    case GrantsRevokedEvent grantsRevoked:
                        return new GrantsRevokedEventAdapter(grantsRevoked);
                    case DeviceAuthorizationFailureEvent deviceAuthorizationFailureEvent:
                        return new DeviceAuthorizationFailureEventAdapter(deviceAuthorizationFailureEvent);
                    case DeviceAuthorizationSuccessEvent deviceAuthorizationSuccessEvent:
                        return new DeviceAuthorizationSuccessEventAdapter(deviceAuthorizationSuccessEvent);
                    case TokenRevokedSuccessEvent tokenRevokedSuccessEvent:
                        return new TokenRevokedSuccessEventAdapter(tokenRevokedSuccessEvent);
                    case InvalidClientConfigurationEvent invalidClientConfiguration:
                        return new InvalidClientConfigurationEventAdapter(invalidClientConfiguration);
                    case TokenIntrospectionFailureEvent tokenIntrospectionFailureEvent:
                        return new TokenIntrospectionFailureEventAdapter(tokenIntrospectionFailureEvent);
                    case TokenIntrospectionSuccessEvent tokenIntrospectionSuccessEvent:
                        return new TokenIntrospectionSuccessEventAdapter(tokenIntrospectionSuccessEvent);
                    case ClientAuthenticationFailureEvent clientAuthenticationFailureEvent:
                        return new ClientAuthenticationFailureEventAdapter(clientAuthenticationFailureEvent);
                    case ClientAuthenticationSuccessEvent clientAuthenticationSuccessEvent:
                        return new ClientAuthenticationSuccessEventAdapter(clientAuthenticationSuccessEvent);
                    case ApiAuthenticationFailureEvent apiAuthenticationFailureEvent:
                        return new ApiAuthenticationFailureEventAdapter(apiAuthenticationFailureEvent);
                    case ApiAuthenticationSuccessEvent apiAuthenticationSuccessEvent:
                        return new ApiAuthenticationSuccessEventAdapter(apiAuthenticationSuccessEvent);
                    case UnhandledExceptionEvent unhandledExceptionEvent:
                        return new UnhandledExceptionEventAdapter(unhandledExceptionEvent);
                }
            }

            return null;
        }
    }
}
