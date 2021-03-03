using RSK.Audit.EF;
using Xunit;

namespace RSK.Audit.Tests.EF
{
    public class AuditEntityTests
    {
        [Fact]
        public void NormalisedSubject_WhenCalledShouldReturnSubjectInUppercase()
        {
            string expectedSubject = "lower case";

            var sut = new AuditEntity()
            {
                Subject = expectedSubject
            };

            Assert.Equal(sut.NormalisedSubject,expectedSubject.ToUpperInvariant());
        }

        [Fact]
        public void NormalisedAction_WhenCalledShouldReturnActionInUppercase()
        {
            string expectedAction = "lower case";

            var sut = new AuditEntity()
            {
                Action = expectedAction
            };

            Assert.Equal(sut.NormalisedAction, expectedAction.ToUpperInvariant());
        }

        [Fact]
        public void NormalisedResource_WhenCalledShouldReturnResourceInUppercase()
        {
            string expectedResource = "lower case";

            var sut = new AuditEntity()
            {
                Resource = expectedResource
            };

            Assert.Equal(sut.NormalisedResource, expectedResource.ToUpperInvariant());
        }

        [Fact]
        public void NormalisedSource_WhenCalledShouldReturnSourceInUppercase()
        {
            string expectedSource = "lower case";

            var sut = new AuditEntity()
            {
                Source = expectedSource
            };

            Assert.Equal(sut.NormalisedSource, expectedSource.ToUpperInvariant());
        }
    }
}