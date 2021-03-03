using System;
using Moq;
using RSK.Audit.EF;
using Xunit;

namespace RSK.Audit.Tests.EF
{
    public class AuditQueryFactoryTests
    {
        private Mock<IUnitOfOWorkFactory> uowf;

        public AuditQueryFactoryTests()
        {
                uowf = new Mock<IUnitOfOWorkFactory>();
        }

        [Fact]
        public void ctor_WhenCalled_WithNullUnitOfWorkFactory_ShouldThrowArgumentNullException()
        {
            uowf = null;

            Assert.Throws<ArgumentNullException>(() => CreateSut());
        }

        [Fact]
        public void Between_WhenCalled_WithFromLessThanToo_ShouldThrowArgumentOutOfRangeException()
        {
            var sut = CreateSut();

            DateTime from = new DateTime(2018,3,2,10,0,0);
            DateTime to = from.Subtract(TimeSpan.FromDays(1));

            Assert.Throws<ArgumentOutOfRangeException>(() => sut.Between(from, to));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Between_WhenCalled_WithInvalidPageNumber_ShouldThrowArgumentOutOfRangeException(int invalidPage)
        {
            var sut = CreateSut();

            DateTime from = new DateTime(2018, 3, 2, 10, 0, 0);
            DateTime to = from.Add(TimeSpan.FromDays(1));

            Assert.Throws<ArgumentOutOfRangeException>(() => sut.Between(from, to,invalidPage,2));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Between_WhenCalled_WithInvalidPageSize_ShouldThrowArgumentOutOfRangeException(int invalidPageSize)
        {
            var sut = CreateSut();

            DateTime from = new DateTime(2018, 3, 2, 10, 0, 0);
            DateTime to = from.Add(TimeSpan.FromDays(1));

            Assert.Throws<ArgumentOutOfRangeException>(() => sut.Between(from, to, 1, invalidPageSize));
        }

        private AuditQueryFactory CreateSut()
        {
            return new AuditQueryFactory(uowf?.Object);
        }
    }
}