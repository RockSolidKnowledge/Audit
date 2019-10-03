using System;
using System.Threading.Tasks;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace RSK.IdentityServer4.AuditEventSink.Tests
{
    public class EventSinkAggregatorTests
    {
        [Fact]
        public async Task PersistAsync_WhenCalledWithMultiEventSinks_WillRaiseWithAll()
        {
            // Arrange
            var sink1 = new StubSink();
            var sink2 = new StubSink();

            var sut = new EventSinkAggregator(new Mock<ILogger>().Object);

            sut.EventSinks.Add(sink1);
            sut.EventSinks.Add(sink2);

            // Act
            await sut.PersistAsync(new StubEvent());

            // Assert
            Assert.Equal(1, sink1.WasCalled);
            Assert.Equal(1, sink2.WasCalled);
        }

        [Fact]
        public async Task PersistAsync_WhenCalledWithMultiEventSinksAndOneThrowsAnException_WillRaiseToAllEventSinks()
        {
            // Arrange
            var sink1 = new StubSink();
            var sink2 = new StubSink();
            var sink3 = new StubSinkThrowsException();

            var logger = new StubLogger();

            var sut = new EventSinkAggregator(logger);

            sut.EventSinks.Add(sink1);
            sut.EventSinks.Add(sink2);
            sut.EventSinks.Add(sink3);

            // Act
            await sut.PersistAsync(new StubEvent());

            // Assert
            Assert.Equal(1, sink1.WasCalled);
            Assert.Equal(1, sink2.WasCalled);
            Assert.Equal(1, sink3.WasCalled);
            Assert.Equal(1, logger.TimesErrored);
        }

        private class StubSink : IEventSink
        {
            public int WasCalled { get; private set; }

            public Task PersistAsync(Event evt)
            {
                WasCalled++;
                return Task.CompletedTask;
            }
        }
        
        private class StubSinkThrowsException : IEventSink
        {
            public int WasCalled { get; private set; }

            public Task PersistAsync(Event evt)
            {
                WasCalled++;
                throw new Exception("Blah");
            }
        }

        private class StubEvent : Event
        {
            public StubEvent() : base(string.Empty, string.Empty, EventTypes.Failure, 0)
            {
            }
        }

        private class StubLogger : ILogger
        {
            public int TimesErrored = 0;
            
            public IDisposable BeginScope<TState>(TState state)
            {
                throw new NotImplementedException();
            }

            public bool IsEnabled(LogLevel logLevel) => true;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                if (logLevel == LogLevel.Error) TimesErrored++;
            }
        }
    }
}
