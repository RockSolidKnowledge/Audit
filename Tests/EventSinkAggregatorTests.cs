using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
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

            var logger = new Mock<ILogger>();

            var sut = new EventSinkAggregator(logger.Object);

            sut.EventSinks.Add(sink1);
            sut.EventSinks.Add(sink2);
            sut.EventSinks.Add(sink3);

            // Act
            await sut.PersistAsync(new StubEvent());

            // Assert
            Assert.Equal(1, sink1.WasCalled);
            Assert.Equal(1, sink2.WasCalled);
            Assert.Equal(1, sink3.WasCalled);
            logger.Verify(x => x.Log(LogLevel.Error, 0, It.IsAny<object>(), null, It.IsAny<Func<object, Exception, string>>()), Times.Once);
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
    }
}
