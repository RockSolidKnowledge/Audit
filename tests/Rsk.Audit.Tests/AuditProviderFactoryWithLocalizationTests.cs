using System;
using Microsoft.Extensions.Localization;
using Moq;
using Xunit;

namespace RSK.Audit.Tests
{
    public class AuditProviderFactoryWithLocalizationTests
    {
        private Mock<IStringLocalizer> stringLocalizer;
        private Mock<AuditProviderFactory> auditProviderFactory;

        public AuditProviderFactoryWithLocalizationTests()
        {
                stringLocalizer = new Mock<IStringLocalizer>();
            auditProviderFactory = new Mock<AuditProviderFactory>();
        }

        [Fact]
        public void ctor_WhenCalledWithNullAuditProviderFactory_ShouldThrowArgumentNullException()
        {
            auditProviderFactory = null;

            Assert.Throws<ArgumentNullException>(() => CreateSut());
        }

        [Fact]
        public void ctor_WhenCalledWithNullStringLocalizer_ShouldThrowArgumentNullException()
        {
            stringLocalizer = null;

            Assert.Throws<ArgumentNullException>(() => CreateSut());
        }

        [Fact]
        public void CreateAuditQuery_WhenCalled_ShouldCallDecoratedFactory()
        {
            Mock<IQueryableAuditableActions> expectedQueryableAuditableActions = new Mock<IQueryableAuditableActions>();

            auditProviderFactory.Setup(apf => apf.CreateAuditQuery()).Returns(expectedQueryableAuditableActions.Object);

            var sut = CreateSut();
            
            var query = sut.CreateAuditQuery();

            Assert.Equal(expectedQueryableAuditableActions.Object,query);
        }

        [Fact]
        public void CreateAuditSource_WhenCalled_ShouldReturnLocalizedAuditSource()
        {
            string expectedSource = "IdentityServer";

            Mock<IRecordAuditableActions> expectedAuditableRecordedActions = new Mock<IRecordAuditableActions>();

            auditProviderFactory.Setup(apf => apf.CreateAuditSource(expectedSource)).Returns(expectedAuditableRecordedActions.Object);

            var sut = CreateSut();

            var recorder = sut.CreateAuditSource(expectedSource);

            Assert.Equal(typeof(RecordLocalizedAuditableActions),recorder.GetType());
            auditProviderFactory.Verify(apf => apf.CreateAuditSource(expectedSource),Times.Once);
        }
       
        private AuditProviderFactoryWithLocalization CreateSut()
        {
            return new AuditProviderFactoryWithLocalization(auditProviderFactory?.Object, stringLocalizer?.Object);
        }
    }
}
