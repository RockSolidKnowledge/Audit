using System;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Moq;
using RSK.Audit.EF;
using Rsk.Audit.Tests;
using Xunit;

namespace RSK.Audit.Tests.EF
{
    public class AuditRecorderTests
    {
        private Fixture fixture = new Fixture();
        private Func<DateTime> now = () => DateTime.Now;
        private Mock<DbSet<AuditEntity>> auditEntities =  new Mock<DbSet<AuditEntity>>(); 

        private Mock<IUnitOfWork> unitOfWork = new Mock<IUnitOfWork>();
        private Mock<IUnitOfOWorkFactory> uowFactory = new Mock<IUnitOfOWorkFactory>();
        private string expectedSource = "TestSource";

        public AuditRecorderTests()
        {
            uowFactory.Setup(uowf => uowf.Create()).Returns(unitOfWork.Object);

            unitOfWork.Setup(uow => uow.AuditEntries).Returns(auditEntities.Object);
        }

        [Fact]
        public void ctor_WhenCalledWithNullSource_ShouldThrowArgumentNullException()
        {
            expectedSource = null;

            Assert.Throws<ArgumentNullException>(() => CreateSut());
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void ctor_WhenCalledWithEmptySource_ShouldThrowArgumentException(string invalidSource)
        {
            expectedSource = invalidSource;

            Assert.Throws<ArgumentException>(() => CreateSut());
        }

        [Fact]
        public void RecordSuccess_WhenCalled_ShouldAddSuccessRecordForSource()
        {
            RecordAction(sut => sut.RecordSuccess,true);
        }

        [Fact]
        public void RecordFailure_WhenCalled_ShouldAddFailureRecordForSource()
        {
            RecordAction(sut => sut.RecordFailure, false);
        }

        [Fact]
        public void RecordSuccess_WhenCalledAndFails_ShouldThrowAuditWriteException()
        {
            auditEntities.Setup(ae => ae.Add(It.IsAny<AuditEntity>()))
                .Throws(new Exception("Blah"));

            var sut = CreateSut();

            Assert.ThrowsAsync<AuditWriteException>(() =>
                sut.RecordSuccess(AuditContextMockingHelper.CreateAuditEventContext( 
                    
                new UserResourceActor(fixture.Create<string>()), 
                fixture.Create<string>(), 
                new AuditableResource(fixture.Create<string>()),
                fixture.Create<string>()))).Wait();
        }

        private void RecordAction( Func<AuditRecorder,Func<IAuditEventArguments,Task>> recordMethodFactory , bool isSuccess)
        {
            DateTime expectedWhen = new DateTime(2018, 3, 5, 10, 23, 11, DateTimeKind.Utc);
            var expectedSubject = new UserResourceActor(fixture.Create<string>());
            string expectedAction = fixture.Create<string>();
            string expectedDescription = fixture.Create<string>();
            var expectedResource = new AuditableResource(fixture.Create<string>());


            IAuditEventArguments auditEventContext = AuditContextMockingHelper.CreateAuditEventContext(expectedSubject,
                expectedAction, expectedResource, expectedDescription);
            now = () => expectedWhen;

            var sut = CreateSut();

            recordMethodFactory(sut)(auditEventContext).Wait();

            auditEntities.Verify(ae => ae.Add(new AuditEntity()
            {
                Source = expectedSource,
                SubjectIdentifier = expectedSubject.Identifier,
                SubjectType = expectedSubject.Type,
                Subject = "",
                Description = expectedDescription,
                Resource = expectedResource.Name,
                ResourceType = expectedResource.Type,
                ResourceIdentifier = expectedResource.Identifier,
                Succeeded = isSuccess,
                Action = expectedAction,
                When = expectedWhen
            }), Times.Once);

            unitOfWork.Verify(uow => uow.Commit(), Times.Once);
        }

        AuditRecorder CreateSut()
        {
            return new AuditRecorder(expectedSource,uowFactory?.Object,now);
        }
    }
}