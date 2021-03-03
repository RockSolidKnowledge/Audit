using System;
using Microsoft.Extensions.Localization;
using Moq;
using Rsk.Audit.Tests;
using Xunit;

namespace RSK.Audit.Tests
{
    public class RecordLocalizedAuditableActionsTests
    {
        private Mock<IRecordAuditableActions> recordAuditableActions;
        private Mock<IStringLocalizer> localizer;

        public RecordLocalizedAuditableActionsTests()
        {
                recordAuditableActions = new Mock<IRecordAuditableActions>();
                localizer = new Mock<IStringLocalizer>();
        }

        [Fact]
        public void ctor_WithNullRecordAutiableActions_ShouldThrowArgumentNullException()
        {
            recordAuditableActions = null;

            Assert.Throws<ArgumentNullException>(() => CreateSut());
        }

        [Fact]
        public void ctor_WithNullLocalizer_ShouldThrowArgumentNullException()
        {
            localizer = null;

            Assert.Throws<ArgumentNullException>(() => CreateSut());
        }

        [Fact]
        public void RecordSuccess_WhenCalled_ShouldLocalizeActionAndForward()
        {
            var expectedSubject = new UserResourceActor( "fred");
            var expectedResource = new AuditableResource("identityServer");

            string actionKey = "login";
            string expectedTranslationText = "la Login";
            LocalizedString expectedTranslation = new LocalizedString(actionKey, expectedTranslationText, false);

            localizer.Setup(sl => sl[actionKey]).Returns(expectedTranslation);

            var sut = CreateSut();

            IAuditEventArguments auditEventContext =
                AuditContextMockingHelper.CreateAuditEventContext(expectedSubject, actionKey, expectedResource, "");
            sut.RecordSuccess(auditEventContext).Wait();

            recordAuditableActions.Verify(
                raa => raa.RecordSuccess(It.Is<IAuditEventArguments>(aec => aec.Action == expectedTranslationText)),
                Times.Once);
        }

        [Fact]
        public void RecordFailure_WhenCalled_ShouldLocalizeActionAndForward()
        {
            var expectedSubject = new UserResourceActor("fred");
            var expectedResource = new AuditableResource("identityServer");

            string actionKey = "login";
            string expectedTranslationText = "la Login";
            LocalizedString expectedTranslation = new LocalizedString(actionKey, expectedTranslationText, false);

            localizer.Setup(sl => sl[actionKey]).Returns(expectedTranslation);

            var sut = CreateSut();

            IAuditEventArguments auditEventContext =
                AuditContextMockingHelper.CreateAuditEventContext(expectedSubject, actionKey, expectedResource, "");
            sut.RecordFailure(auditEventContext).Wait();

            recordAuditableActions.Verify(
                raa => raa.RecordFailure(It.Is<IAuditEventArguments>(aec => aec.Action == expectedTranslationText)),Times.Once);
        }


        [Fact]
        public void RecordFailure_WhenCalled_ShouldLocalizeActionContext()
        {
            var expectedSubject = new UserResourceActor("fred");
            var expectedResource = new AuditableResource("identityServer");

            string action = "login";
           
            localizer.Setup(sl => sl[action]).Returns(new LocalizedString("login","la login",true));

            var sut = CreateSut();

            IAuditEventArguments auditEventContext =
                AuditContextMockingHelper.CreateAuditEventContext(expectedSubject, action, expectedResource, "");
            sut.RecordSuccess(auditEventContext).Wait();

            recordAuditableActions.Verify(raa => raa.RecordSuccess(It.IsAny<LocalizedAuditEventArguments>()), Times.Once);
        }

        private RecordLocalizedAuditableActions CreateSut()
        {
            return new RecordLocalizedAuditableActions(recordAuditableActions?.Object,localizer?.Object);
        }
    }
}