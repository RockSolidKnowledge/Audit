using System;
using Microsoft.Extensions.Localization;
using Moq;
using Xunit;

namespace RSK.Audit.Tests
{
    public class LocalizedAuditActionContextTests
    {
        private Mock<IAuditEventArguments> context;
        private Mock<IStringLocalizer> localizer;

        public LocalizedAuditActionContextTests()
        {
            context = new Mock<IAuditEventArguments>();
            localizer = new Mock<IStringLocalizer>();
        }

        [Fact]
        public void ctor_WhenContextIsNull_ShouldThrowArgumentNulLException()
        {
            context = null;
            Assert.Throws<ArgumentNullException>(() => CreateSut());
        }


        [Fact]
        public void ctor_WhenLocalizerIsNull_ShouldThrowArgumentNulLException()
        {
            localizer = null;
            Assert.Throws<ArgumentNullException>(() => CreateSut());
        }

        [Fact]
        public void Description_ShouldReturnWithFormatStringLocalized()
        {
            string expectedFormat = "Format";
            string expectedLocalizedFormat = "Localized Format";
            object[] args = new object[] { "arg1",2};

            FormattedString formattedString = new FormattedString(expectedFormat,args);
            context.Setup(c => c.Description).Returns(formattedString);

            localizer.Setup(l => l[expectedFormat])
                .Returns(new LocalizedString(expectedFormat, expectedLocalizedFormat, false));

            var sut = CreateSut();

            Assert.Equal(expectedLocalizedFormat,sut.Description.Format);
        }

        private LocalizedAuditEventArguments CreateSut()
        {
            return new LocalizedAuditEventArguments(context?.Object, localizer?.Object);
        }
    }
}