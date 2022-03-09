using System.Collections.Generic;
using System.Security.Claims;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.ResponseHandling;
using Duende.IdentityServer.Validation;
using IdentityModel;
using Rsk.DuendeIdentityServer.AuditEventSink.Adapters;
using Xunit;

namespace Rsk.DuendeIdentityServer.AuditEventSink.Tests
{
    public class AdapterFactoryTests
    {
        [Fact]
        public void Create_WhenTokenIssuedSuccessEvent_WillReturnTokenIssuedSuccessEventAdapter()
        {
            // Arrange
            var authResponse = new AuthorizeResponse()
            {
                Request = new ValidatedAuthorizeRequest()
                {
                    Client = new Client(),
                    Subject = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(JwtClaimTypes.Subject, string.Empty)
                    }))
                }
            };

            var evt = new TokenIssuedSuccessEvent(authResponse);

            var sut = new AdapterFactory();

            // Act
            var adapter = sut.Create(evt);

            // Assert
            Assert.IsType<TokenIssuedSuccessEventAdapter>(adapter);
        }

        [Fact]
        public void Create_WhenConsentGrantedEvent_WillReturnConsentGrantedEventAdapter()
        {
            // Arrange
            var evt = new ConsentGrantedEvent(string.Empty, string.Empty, new List<string>(), new List<string>(), false);

            var sut = new AdapterFactory();

            // Act
            var adapter = sut.Create(evt);

            // Assert
            Assert.IsType<ConsentGrantedEventAdapter>(adapter);
        }

        [Fact]
        public void Create_WhenUserLoginFailureEvent_WillReturnUserLoginFailureAdapter()
        {
            // Arrange
            var evt = new UserLoginFailureEvent(string.Empty, string.Empty);

            var sut = new AdapterFactory();

            // Act
            var adapter = sut.Create(evt);

            // Assert
            Assert.IsType<UserLoginFailureEventAdapter>(adapter);
        }

        [Fact]
        public void Create_WhenUserLoginSuccessEvent_WillReturnUserLoginSuccessAdapter()
        {
            // Arrange
            var evt = new UserLoginSuccessEvent(string.Empty, string.Empty, string.Empty, string.Empty);

            var sut = new AdapterFactory();

            // Act
            var adapter = sut.Create(evt);

            // Assert
            Assert.IsType<UserLoginSuccessEventAdapter>(adapter);
        }

        [Fact]
        public void Create_WhenUserLogoutSuccessEvent_WillReturnUserLogoutSuccessAdapter()
        {
            // Arrange
            var evt = new UserLogoutSuccessEvent(string.Empty, string.Empty);

            var sut = new AdapterFactory();

            // Act
            var adapter = sut.Create(evt);

            // Assert
            Assert.IsType<UserLogoutSuccessEventAdapter>(adapter);
        }

        [Fact]
        public void Create_WhenConsentDeniedEvent_WillReturnConsentDeniedAdapter()
        {
            // Arrange
            var evt = new ConsentDeniedEvent(string.Empty, string.Empty, new string[] { });

            var sut = new AdapterFactory();

            // Act
            var adapter = sut.Create(evt);

            // Assert
            Assert.IsType<ConsentDeniedEventAdapter>(adapter);
        }

        [Fact]
        public void Create_WhenTokenIssuedFailureEvent_WillReturnTokenIssuedFailureAdapter()
        {
            // Arrange
            var request = new ValidatedAuthorizeRequest()
            {
                Client = new Client(),
                Subject = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(JwtClaimTypes.Subject, string.Empty)
                }))
            };

            var evt = new TokenIssuedFailureEvent(request, string.Empty, string.Empty);

            var sut = new AdapterFactory();

            // Act
            var adapter = sut.Create(evt);

            // Assert
            Assert.IsType<TokenIssuedFailureEventAdapter>(adapter);
        }

        [Fact]
        public void Create_WhenGrantsRevokedEvent_WillReturnGrantsRevokedAdapter()
        {
            // Arrange
            var evt = new GrantsRevokedEvent(string.Empty, string.Empty);

            var sut = new AdapterFactory();

            // Act
            var adapter = sut.Create(evt);

            // Assert
            Assert.IsType<GrantsRevokedEventAdapter>(adapter);
        }

        [Fact]
        public void Create_WhenApiAuthenticationFailureEvent_WillReturnApiAuthenticationFailureEventAdapter()
        {
            // Arrange
            var evt = new ApiAuthenticationFailureEvent(string.Empty, string.Empty);

            var sut = new AdapterFactory();

            // Act
            var adapter = sut.Create(evt);

            // Assert
            Assert.IsType<ApiAuthenticationFailureEventAdapter>(adapter);
        }

        [Fact]
        public void Create_WhenApiAuthenticationSuccessEvent_WillReturnApiAuthenticationSuccessEventAdapter()
        {
            // Arrange
            var evt = new ApiAuthenticationSuccessEvent(string.Empty, string.Empty);

            var sut = new AdapterFactory();

            // Act
            var adapter = sut.Create(evt);

            // Assert
            Assert.IsType<ApiAuthenticationSuccessEventAdapter>(adapter);
        }

        [Fact]
        public void Create_WhenClientAuthenticationFailureEvent_WillReturnClientAuthenticationFailureEventAdapter()
        {
            // Arrange
            var evt = new ClientAuthenticationFailureEvent(string.Empty, string.Empty);

            var sut = new AdapterFactory();

            // Act
            var adapter = sut.Create(evt);

            // Assert
            Assert.IsType<ClientAuthenticationFailureEventAdapter>(adapter);
        }

        [Fact]
        public void Create_WhenClientAuthenticationSuccessEvent_WillReturnClientAuthenticationSuccessEventAdapter()
        {
            // Arrange
            var evt = new ClientAuthenticationSuccessEvent(string.Empty, string.Empty);

            var sut = new AdapterFactory();

            // Act
            var adapter = sut.Create(evt);

            // Assert
            Assert.IsType<ClientAuthenticationSuccessEventAdapter>(adapter);
        }

        [Fact]
        public void Create_WhenDeviceAuthorizationFailureEvent_WillReturnDeviceAuthorizationFailureEventAdapter()
        {
            // Arrange
            var evt = new DeviceAuthorizationFailureEvent(new DeviceAuthorizationRequestValidationResult(null));

            var sut = new AdapterFactory();

            // Act
            var adapter = sut.Create(evt);

            // Assert
            Assert.IsType<DeviceAuthorizationFailureEventAdapter>(adapter);
        }

        [Fact]
        public void Create_WhenDeviceAuthorizationSuccessEvent_WillReturnDeviceAuthorizationSuccessEventAdapter()
        {
            // Arrange
            var evt = new DeviceAuthorizationSuccessEvent(new DeviceAuthorizationResponse(), new DeviceAuthorizationRequestValidationResult(new ValidatedDeviceAuthorizationRequest()));

            var sut = new AdapterFactory();

            // Act
            var adapter = sut.Create(evt);

            // Assert
            Assert.IsType<DeviceAuthorizationSuccessEventAdapter>(adapter);
        }

        [Fact]
        public void Create_WhenInvalidClientConfigurationEvent_WillReturnInvalidClientConfigurationEventAdapter()
        {
            // Arrange
            var evt = new InvalidClientConfigurationEvent(new Client(), string.Empty);

            var sut = new AdapterFactory();

            // Act
            var adapter = sut.Create(evt);

            // Assert
            Assert.IsType<InvalidClientConfigurationEventAdapter>(adapter);
        }

        [Fact]
        public void Create_WhenTokenIntrospectionFailureEvent_WillReturnTokenIntrospectionFailureEventAdapter()
        {
            // Arrange
            var evt = new TokenIntrospectionFailureEvent(string.Empty, string.Empty);

            var sut = new AdapterFactory();

            // Act
            var adapter = sut.Create(evt);

            // Assert
            Assert.IsType<TokenIntrospectionFailureEventAdapter>(adapter);
        }

        [Fact]
        public void Create_WhenTokenIntrospectionSuccessEvent_WillReturnTokenIntrospectionSuccessEventAdapter()
        {
            // Arrange
            var evt = new TokenIntrospectionSuccessEvent(new IntrospectionRequestValidationResult { Api = new ApiResource() });

            var sut = new AdapterFactory();

            // Act
            var adapter = sut.Create(evt);

            // Assert
            Assert.IsType<TokenIntrospectionSuccessEventAdapter>(adapter);
        }

        [Fact]
        public void Create_WhenTokenRevokedSuccessEvent_WillReturnTokenRevokedSuccessEventAdapter()
        {
            // Arrange
            var evt = new TokenRevokedSuccessEvent(new TokenRevocationRequestValidationResult(), new Client());

            var sut = new AdapterFactory();

            // Act
            var adapter = sut.Create(evt);

            // Assert
            Assert.IsType<TokenRevokedSuccessEventAdapter>(adapter);
        }

        [Fact]
        public void Create_WhenUnhandledExceptionEvent_WillReturnUnhandledExceptionEventAdapter()
        {
            // Arrange
            var evt = new UnhandledExceptionEvent(new System.Exception());

            var sut = new AdapterFactory();

            // Act
            var adapter = sut.Create(evt);

            // Assert
            Assert.IsType<UnhandledExceptionEventAdapter>(adapter);
        }

        [Fact]
        public void Create_WhenBackchannelAuthenticationSuccessEvent_WillReturnBackchannelAuthenticationSuccessEventAdapter()
        {
            // Arrange
            var evt = new BackchannelAuthenticationSuccessEvent(new BackchannelAuthenticationRequestValidationResult(new ValidatedBackchannelAuthenticationRequest{Client = new Client()}));

            var sut = new AdapterFactory();

            // Act
            var adapter = sut.Create(evt);

            // Assert
            Assert.IsType<BackchannelAuthenticationSuccessEventAdapter>(adapter);
        }

        [Fact]
        public void Create_WhenBackchannelAuthenticationFailureEvent_WillReturnBackchannelAuthenticationFailureEventAdapter()
        {
            // Arrange
            var evt = new BackchannelAuthenticationFailureEvent(new BackchannelAuthenticationRequestValidationResult(new ValidatedBackchannelAuthenticationRequest { Client = new Client() }));

            var sut = new AdapterFactory();

            // Act
            var adapter = sut.Create(evt);

            // Assert
            Assert.IsType<BackchannelAuthenticationFailureEventAdapter>(adapter);
        }

        [Fact]
        public void Create_WhenInvalidIdentityProviderConfigurationEvent_WillReturnInvalidIdentityProviderConfigurationAdapter()
        {
            // Arrange
            var evt = new InvalidIdentityProviderConfiguration(new IdentityProvider("a"), "err");

            var sut = new AdapterFactory();

            // Act
            var adapter = sut.Create(evt);

            // Assert
            Assert.IsType<InvalidIdentityProviderConfigurationAdapter>(adapter);
        }
    }
}
