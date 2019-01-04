using System.Threading.Tasks;
using IdentityServer4.Events;
using Moq;
using RSK.Audit;
using Xunit;

namespace RSK.IdentityServer4.AuditEventSink.Tests
{
    public class AuditSinkTests
    {
        [Fact]
        public async Task PersistAsync_WhenSuccessEvent_WillCallSuccessAuditRecord()
        {
            // Arrange
            var recorder = new Mock<IRecordAuditableActions>();
            var factory = new Mock<IAdapterFactory>();
            factory.Setup(x => x.Create(It.IsAny<Event>())).Returns(new Mock<IAuditEventArguments>().Object);

            var sut = new AuditSink(recorder.Object) {Factory = factory.Object};


            var successfulEvent = new StubEvent(string.Empty, string.Empty, EventTypes.Success, -1);

            // Act
            await sut.PersistAsync(successfulEvent);

            // Assert
            recorder.Verify(x => x.RecordSuccess(It.IsAny<IAuditEventArguments>()), Times.Once);
        }

        [Fact]
        public async Task PersistAsync_WhenInformationEvent_WillCallSuccessAuditRecord()
        {
            // Arrange
            var recorder = new Mock<IRecordAuditableActions>();
            var factory = new Mock<IAdapterFactory>();
            factory.Setup(x => x.Create(It.IsAny<Event>())).Returns(new Mock<IAuditEventArguments>().Object);

            var sut = new AuditSink(recorder.Object) { Factory = factory.Object };


            var successfulEvent = new StubEvent(string.Empty, string.Empty, EventTypes.Information, -1);

            // Act
            await sut.PersistAsync(successfulEvent);

            // Assert
            recorder.Verify(x => x.RecordSuccess(It.IsAny<IAuditEventArguments>()), Times.Once);
        }

        [Fact]
        public async Task PersistAsync_WhenErrorEvent_WillCallFailureAuditRecord()
        {
            // Arrange
            var recorder = new Mock<IRecordAuditableActions>();
            var factory = new Mock<IAdapterFactory>();
            factory.Setup(x => x.Create(It.IsAny<Event>())).Returns(new Mock<IAuditEventArguments>().Object);

            var sut = new AuditSink(recorder.Object) { Factory = factory.Object };


            var successfulEvent = new StubEvent(string.Empty, string.Empty, EventTypes.Error, -1);

            // Act
            await sut.PersistAsync(successfulEvent);

            // Assert
            recorder.Verify(x => x.RecordFailure(It.IsAny<IAuditEventArguments>()), Times.Once);
        }

        [Fact]
        public async Task PersistAsync_WhenFailureEvent_WillCallFailureAuditRecord()
        {
            // Arrange
            var recorder = new Mock<IRecordAuditableActions>();
            var factory = new Mock<IAdapterFactory>();
            factory.Setup(x => x.Create(It.IsAny<Event>())).Returns(new Mock<IAuditEventArguments>().Object);

            var sut = new AuditSink(recorder.Object) { Factory = factory.Object };


            var successfulEvent = new StubEvent(string.Empty, string.Empty, EventTypes.Failure, -1);

            // Act
            await sut.PersistAsync(successfulEvent);

            // Assert
            recorder.Verify(x => x.RecordFailure(It.IsAny<IAuditEventArguments>()), Times.Once);
        }

        private class StubEvent : Event
        {
            public StubEvent(string category, string name, EventTypes type, int id, string message = null) : base(category, name, type, id, message)
            {
            }
        }
    }
}
