using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using RSK.Audit.EF;
using Rsk.Audit.Tests;
using Xunit;

namespace RSK.Audit.Tests.EF
{
    public class AuditQueryTests
    {
        private IQueryable<AuditEntity> baseQuery;
        private Mock<ICriteriaBuilder<AuditEntity>> criteriaBuilder;

        public AuditQueryTests()
        {
            criteriaBuilder = new Mock<ICriteriaBuilder<AuditEntity>>();
            baseQuery = new List<AuditEntity>().AsQueryable();

            criteriaBuilder.Setup(cb => cb.Build()).Returns(_ => true);

            criteriaBuilder.Setup(cb => cb.AddStringTransform(It.IsAny<string>(),
                It.IsAny<Func<string, string>>())).Returns(criteriaBuilder.Object);
        }

        [Fact]
        public void ctor_WhenCalledWithNullBaseQuery_ShouldThrowArgumentNullException()
        {
            baseQuery = null;

            Assert.Throws<ArgumentNullException>(() => CreateSut());
        }

        [Fact]
        public void AndSubject_WhenCalled_ShouldAddToCriteria()
        {
           ResourceActor subject = new UserResourceActor("andy");

            var sut = CreateSut();

            sut.AndSubject(subject);

            criteriaBuilder.Verify(cb => cb.AndStringMatch(Matches.Exactly,  nameof(AuditEntity.SubjectIdentifier), subject.Identifier), Times.Once);
            criteriaBuilder.Verify(cb => cb.AndStringMatch(Matches.Exactly, nameof(AuditEntity.SubjectType), subject.Type), Times.Once);
        }

        [Fact]
        public void AndSubjectIdentifier_WhenCalled_ShouldAddToCriteria()
        {
            Matches expectedMatches = Matches.Exactly;
            string expectedValueToMatch = "andy";

            var sut = CreateSut();

            sut.AndSubjectIdentifier(expectedMatches, expectedValueToMatch);

            criteriaBuilder.Verify(cb => cb.AndStringMatch(expectedMatches,nameof(AuditEntity.SubjectIdentifier),expectedValueToMatch),Times.Once);
        }

        [Fact]
        public void AndSubjectType_WhenCalled_ShouldAddToCriteria()
        {
            Matches expectedMatches = Matches.Exactly;
            string expectedValueToMatch = "AdminUI";

            var sut = CreateSut();

            sut.AndSubjectType(expectedMatches, expectedValueToMatch);

            criteriaBuilder.Verify(cb => cb.AndStringMatch(expectedMatches, nameof(AuditEntity.SubjectType), expectedValueToMatch), Times.Once);
        }

        [Fact]
        public void AndSubjectKnownAs_WhenCalled_ShouldAddToCriteria()
        {
            Matches expectedMatches = Matches.Exactly;
            string expectedValueToMatch = "andy@rocksolidknowledge.com";

            var sut = CreateSut();

            sut.AndSubjectKnownAs(expectedMatches, expectedValueToMatch);

            criteriaBuilder.Verify(cb => cb.AndStringMatch(expectedMatches, nameof(AuditEntity.NormalisedSubject), expectedValueToMatch), Times.Once);
        }

        [Fact]
        public void AndSubjectWithString_WhenCalled_ShouldAddToCriteria()
        {
            Matches expectedMatches = Matches.Exactly;
            string expectedValueToMatch = "andy@rocksolidknowledge.com";

            var sut = CreateSut();

            sut.AndSubject(expectedMatches, expectedValueToMatch);

            criteriaBuilder.Verify(cb => cb.AndStringMatchAny(expectedMatches, new string[] { nameof(AuditEntity.NormalisedSubject) , nameof(AuditEntity.SubjectIdentifier) }, expectedValueToMatch), Times.Once);
        }

        [Fact]
        public void AndResourceWithString_WhenCalled_ShouldAddToCriteria()
        {
            Matches expectedMatches = Matches.Exactly;
            string expectedValueToMatch = "andy@rocksolidknowledge.com";

            var sut = CreateSut();

            sut.AndResource(expectedMatches, expectedValueToMatch);

            criteriaBuilder.Verify(cb => cb.AndStringMatchAny(expectedMatches, 
                new string[] { nameof(AuditEntity.NormalisedResource), nameof(AuditEntity.ResourceIdentifier) }, expectedValueToMatch), Times.Once);
        }

        [Fact]
        public void AndResource_WhenCalled_ShouldAddToCriteria()
        {
            var expectedMatch = new AuditableResource("Clients", "https://ids.acme.com");

            var sut = CreateSut();

            sut.AndResource(expectedMatch);

            criteriaBuilder.Verify(
                cb => cb.AndStringMatch(Matches.Exactly, nameof(AuditEntity.Resource), expectedMatch.Identifier),
                Times.Once);
            criteriaBuilder.Verify(
                cb => cb.AndStringMatch(Matches.Exactly, nameof(AuditEntity.ResourceType), expectedMatch.Type),
                Times.Once);
        }

        [Fact]
        public void AndResourceIdentifier_WhenCalled_ShouldAddToCriteria()
        {
            Matches expectedMatches = Matches.Exactly;
            string expectedValueToMatch = "https://ids.acme.com";

            var sut = CreateSut();

            sut.AndResourceIdentifier(expectedMatches, expectedValueToMatch);

            criteriaBuilder.Verify(cb => cb.AndStringMatch(expectedMatches, nameof(AuditEntity.Resource), expectedValueToMatch), Times.Once);
        }

        [Fact]
        public void AndResourceType_WhenCalled_ShouldAddToCriteria()
        {
            Matches expectedMatches = Matches.Exactly;
            string expectedValueToMatch = "Client";

            var sut = CreateSut();

            sut.AndResourceType(expectedMatches, expectedValueToMatch);

            criteriaBuilder.Verify(cb => cb.AndStringMatch(expectedMatches, nameof(AuditEntity.ResourceType), expectedValueToMatch), Times.Once);
        }

        [Fact]
        public void AndSource_WhenCalled_ShouldAddToCriteria()
        {
            Matches expectedMatches = Matches.Exactly;
            string expectedValueToMatch = "andy";

            var sut = CreateSut();

            sut.AndSource(expectedMatches, expectedValueToMatch);

            criteriaBuilder.Verify(cb => cb.AndStringMatch(expectedMatches, nameof(AuditEntity.NormalisedSource), expectedValueToMatch), Times.Once);
        }

        [Fact]
        public void AndAction_WhenCalled_ShouldAddToCriteria()
        {
            Matches expectedMatches = Matches.Exactly;
            string expectedValueToMatch = "andy";

            var sut = CreateSut();

            sut.AndAction(expectedMatches, expectedValueToMatch);

            criteriaBuilder.Verify(cb => cb.AndStringMatch(expectedMatches, nameof(AuditEntity.NormalisedAction), expectedValueToMatch), Times.Once);
        }

        [Fact]
        public void AndDescription_WhenCalled_ShouldAddToCriteria()
        {
            Matches expectedMatches = Matches.Exactly;
            string expectedValueToMatch = "andy";

            var sut = CreateSut();

            sut.AndDescription(expectedMatches, expectedValueToMatch);

            criteriaBuilder.Verify(cb => cb.AndStringMatch(expectedMatches, nameof(AuditEntity.Description), expectedValueToMatch), Times.Once);
        }

        [Fact]
        public void AndAnyText_WhenCalled_ShouldAddToCriteria()
        {
            Matches expectedMatches = Matches.Exactly;
            string expectedValueToMatch = "andy";

            var sut = CreateSut();

            sut.AndAnyText(expectedMatches, expectedValueToMatch);

            criteriaBuilder.Verify(cb => cb.AndStringMatch(expectedMatches, nameof(AuditEntity.Description), expectedValueToMatch), Times.Once);
            criteriaBuilder.Verify(cb => cb.OrStringMatch(expectedMatches, nameof(AuditEntity.Action), expectedValueToMatch), Times.Once);
            criteriaBuilder.Verify(cb => cb.OrStringMatch(expectedMatches, nameof(AuditEntity.Source), expectedValueToMatch), Times.Once);
            criteriaBuilder.Verify(cb => cb.OrStringMatch(expectedMatches, nameof(AuditEntity.SubjectIdentifier), expectedValueToMatch), Times.Once);
            criteriaBuilder.Verify(cb => cb.OrStringMatch(expectedMatches, nameof(AuditEntity.Resource), expectedValueToMatch), Times.Once);
        }

        [Fact]
        public void AndSucceeded_WhenCalled_ShouldAddToCriteria()
        {
          var expectedValueToMatch = true;

            var sut = CreateSut();

            sut.AndActionSucceeded();

            criteriaBuilder.Verify(cb => 
            cb.AndBoolean( nameof(AuditEntity.Succeeded), expectedValueToMatch), Times.Once);
        }

        [Fact]
        public void AndFailed_WhenCalled_ShouldAddToCriteria()
        {
            var expectedValueToMatch = false;

            var sut = CreateSut();

            sut.AndActionFailed();

            criteriaBuilder.Verify(cb =>
                cb.AndBoolean(nameof(AuditEntity.Succeeded), expectedValueToMatch), Times.Once);
        }


        [Fact]
        public void ExecuteAscending_WhenCalled_ShouldReturnAllSuccessfullRecordsAndCount()
        {
            var entities = SetupFindSuccessEntities();

            var result = CreateSut().ExecuteAscending().Result;

            var expectedResultSet = entities.Where(ae => ae.Succeeded).ToList();

            Assert.Equal(expectedResultSet.ToAuditEntries(), result.Rows);
            Assert.Equal(expectedResultSet.Count,result.TotalNumberOfRows);
        }


        [Fact]
        public void ExecuteDescending_WhenCalled_ShouldReturnAllSuccessfullRecordsAndCount()
        {
            var entities = SetupFindSuccessEntities();

            var result = CreateSut().ExecuteDescending().Result;

            var expectedResultSet = entities.Where(ae => ae.Succeeded).Reverse().ToList();

            Assert.Equal(expectedResultSet.ToAuditEntries(), result.Rows);
            Assert.Equal(expectedResultSet.Count, result.TotalNumberOfRows);
        }


        [Fact]
        public void ExecuteAscending_WhenCalled_ShouldReturnAllRecordsSortedBySource()
        {
            var entities = CreateAuditEntities();

            baseQuery = entities.AsQueryable();

            var result = CreateSut().ExecuteAscending(AuditQuerySort.Source).Result;

            var expectedResultSet = entities.OrderBy(ae => ae.Source).ToList();

            Assert.Equal(expectedResultSet.ToAuditEntries(), result.Rows);
            Assert.Equal(expectedResultSet.Count, result.TotalNumberOfRows);

        }

        [Theory()]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void ExecuteAscending_WhenPagingEnabled_ShouldReturnResultsPaged(int page)
        {
            const int rowsPerPage = 2;

            var entities = CreateAuditEntities();

            baseQuery = entities.AsQueryable();

            var sut = new AuditQueryWithPaging(baseQuery,page,rowsPerPage);

            var results = sut.ExecuteAscending().Result;

            var expectedQuery = entities.OrderBy(ae => ae.When);
            var expectedCount = expectedQuery.Count();

            var expectedNumberOfPages = 4;
            var expectedResultSet =expectedQuery
                .Skip(rowsPerPage * (page-1))
                .Take(rowsPerPage).ToList();
           
            Assert.Equal(expectedCount,results.TotalNumberOfRows);
            Assert.Equal(rowsPerPage,results.Rows.Count());

            Assert.Equal(expectedResultSet.ToAuditEntries(),results.Rows);
            Assert.Equal(expectedNumberOfPages,results.TotalNumberOfPages);

        } 
        private List<AuditEntity> SetupFindSuccessEntities()
        {
            var entities = CreateAuditEntities();

            baseQuery = entities.AsQueryable();

            criteriaBuilder
                .Setup(cb => cb.Build())
                .Returns(ae => ae.Succeeded);
            return entities;
        }

        private static List<AuditEntity> CreateAuditEntities()
        {
            List<AuditEntity> entities = new List<AuditEntity>()
            {
                new AuditEntity()
                {
                    Succeeded = true,
                    Action = "Login",
                    Description = "Stuff",
                    Source = "AdminUI",
                    SubjectIdentifier = "andy",
                    SubjectType = "User",
                    Resource = "https://www.IdentityServer.com",
                    ResourceType = "IdentityServer",
                    When = DateTime.Now
                },
                new AuditEntity()
                {
                    Succeeded = false,
                    Action = "Login",
                    Description = "Stuff",
                    Source = "IdentityServer",
                    SubjectIdentifier = "andy",
                    SubjectType = "User",
                    Resource = "https://www.IdentityServer.com",
                    ResourceType = "IdentityServer",
                    When = DateTime.Now
                },
                new AuditEntity()
                {
                    Succeeded = true,
                    Action = "Login",
                    Description = "Stuff",
                    Source = "BankingApp",
                    SubjectIdentifier = "andy",
                    SubjectType = "User",
                    Resource = "https://www.IdentityServer.com",
                    ResourceType = "IdentityServer",
                    When = DateTime.Now
                },
                new AuditEntity()
                {
                    Succeeded = false,
                    Action = "Login",
                    Description = "Stuff",
                    Source = "ClientApp",
                    SubjectIdentifier = "andy",
                    SubjectType = "User",
                    Resource = "https://www.IdentityServer.com",
                    ResourceType = "IdentityServer",
                    When = DateTime.Now
                },
                new AuditEntity()
                {
                    Succeeded = true,
                    Action = "Login",
                    Description = "Stuff",
                    Source = "BankingApp",
                    SubjectIdentifier = "andy",
                    SubjectType = "User",
                    Resource = "https://www.IdentityServer.com",
                    ResourceType = "IdentityServer",
                    When = DateTime.Now
                },
                new AuditEntity()
                {
                    Succeeded = false,
                    Action = "Login",
                    Description = "Stuff",
                    Source = "ClientApp",
                    SubjectIdentifier = "andy",
                    SubjectType = "User",
                    Resource = "https://www.IdentityServer.com",
                    ResourceType = "IdentityServer",
                    When = DateTime.Now
                },
                new AuditEntity()
                {
                    Succeeded = false,
                    Action = "Login",
                    Description = "Stuff and more",
                    Source = "ClientApp2",
                    SubjectIdentifier = "andy",
                    SubjectType = "User",
                    Resource = "https://www.IdentityServer.com",
                    ResourceType = "IdentityServer",
                    When = DateTime.Now
                },
            };
            return entities;
        }

        AuditQuery CreateSut()
        {
            return new AuditQuery(baseQuery,criteriaBuilder?.Object);
        }
    }
}