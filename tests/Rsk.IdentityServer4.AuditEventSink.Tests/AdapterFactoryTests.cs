using System;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.ResponseHandling;
using IdentityServer4.Validation;
using RSK.IdentityServer4.AuditEventSink.Adapters;
using Xunit;

namespace RSK.IdentityServer4.AuditEventSink.Tests
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
    }
}
