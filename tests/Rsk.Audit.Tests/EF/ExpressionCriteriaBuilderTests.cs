using RSK.Audit.EF;
using Xunit;

namespace RSK.Audit.Tests.EF
{
    public class ExpressionCriteriaBuilderTests
    {
        [Fact]
        public void Build_WhenCalledWithNoCriteria_ShouldReturnAlwaysTrueCriteria()
        {
            var sut = CreateSut();

            var criteria = sut.Build().Compile();

            Assert.True(criteria(new AuditEntity()));
        }

        [Fact]
        public void AndStringMatch_WhenCalledWithEqualsAndTransformation_ShouldCreateCriteria()
        {
            var sut = CreateSut();

            sut.AddStringTransform(nameof(AuditEntity.NormalisedAction), i =>i.ToUpperInvariant());

            var criteria = sut.AndStringMatch(Matches.Exactly,nameof(AuditEntity.NormalisedAction), "create")
                .Build()
                .Compile();

            var matchResult = criteria(new AuditEntity() { Action = "create" });
            var missedResult = criteria(new AuditEntity() { Action = "delete" });

            Assert.True(matchResult);
            Assert.False(missedResult);
        }


        [Fact]
        public void AndStringMatch_WhenCalledWithEquals_ShouldCreateCriteria()
        {
            var sut = CreateSut();

            var criteria = sut.AndStringMatch(Matches.Exactly, "Action", "create")
                .Build()
                .Compile();

            var matchResult = criteria(new AuditEntity() { Action = "create" });
            var missedResult = criteria(new AuditEntity() { Action = "delete" });

            Assert.True(matchResult);
            Assert.False(missedResult);
        }

        [Fact]
        public void AndStringMatch_WhenCalledWithTwoEquals_ShouldCreateCriteriaThatMatchesOnBoth()
        {
            var sut = CreateSut();

            var criteria = sut
                .AndStringMatch(Matches.Exactly, "Action", "create")
                .AndStringMatch(Matches.Exactly,"Source","IdentityServer")
                .Build()
                .Compile();

            var matchResult = criteria(new AuditEntity() { Action = "create"  , Source = "IdentityServer"});
            var missedResult = criteria(new AuditEntity() { Action = "create" , Source = "blah" });

            Assert.True(matchResult);
            Assert.False(missedResult);
        }

        [Fact]
        public void AndStringMatch_WhenCalledWithStartsWith_ShouldCreateCriteria()
        {
            var sut = CreateSut();

            var criteria = sut.AndStringMatch(Matches.StartsWith, "Action", "cr")
                .Build()
                .Compile();

            var matchResult = criteria(new AuditEntity() { Action = "create" });
            var missedResult = criteria(new AuditEntity() { Action = "delete" });

            Assert.True(matchResult);
            Assert.False(missedResult);
        }

        [Fact]
        public void AndStringMatch_WhenCalledWithEndsWith_ShouldCreateCriteria()
        {
            var sut = CreateSut();

            var criteria = sut.AndStringMatch(Matches.EndsWith, "Action", "eate")
                .Build()
                .Compile();

            var matchResult = criteria(new AuditEntity() { Action = "create" });
            var missedResult = criteria(new AuditEntity() { Action = "delete" });

            Assert.True(matchResult);
            Assert.False(missedResult);
        }

        [Fact]
        public void AndStringMatch_WhenCalledWithSomethingLike_ShouldCreateCriteria()
        {
            var sut = CreateSut();

            var criteria = sut.AndStringMatch(Matches.SomethingLike, "Action", "re")
                .Build()
                .Compile();

            var matchResult = criteria(new AuditEntity() { Action = "create" });
            var missedResult = criteria(new AuditEntity() { Action = "update" });

            Assert.True(matchResult);
            Assert.False(missedResult);
        }


        [Fact]
        public void AndBooleanMatch_WhenCalled_ShouldCreateCriteria()
        {
            var sut = CreateSut();

            var criteria = sut.AndBoolean(nameof(AuditEntity.Succeeded),true)
                .Build()
                .Compile();

            var matchResult = criteria(new AuditEntity() { Action = "create" ,Succeeded = true});
            var missedResult = criteria(new AuditEntity() { Action = "create" ,Succeeded = false});

            Assert.True(matchResult);
            Assert.False(missedResult);
        }

        [Fact]
        public void OrStringMatch_WhenCalledWithTwoEquals_ShouldCreateCriteriaThatMatchesEither()
        {
            var sut = CreateSut();

            var criteria = sut
                .AndStringMatch(Matches.Exactly, "Action", "create")
                .OrStringMatch(Matches.Exactly, "Source", "IdentityServer")
                .Build()
                .Compile();

            var matchResult = criteria(new AuditEntity() { Action = "create", Source = "IdentityServer" });
            var secondMatchResult = criteria(new AuditEntity() { Action = "create", Source = "blah" });
            var missedMatchResult = criteria(new AuditEntity() {Action = "delete", Source = "Stuff"});

            Assert.True(matchResult);
            Assert.True(secondMatchResult);
            Assert.False(missedMatchResult);
        }

        [Fact]
        public void AndStringMatchAny_WhenCalledShouldBuildExpressionThatMatchesEitherStrings()
        {
            var sut = CreateSut();

            var criteria = sut
                .AndStringMatchAny(Matches.Exactly, new string[]{ nameof(AuditEntity.Subject) ,nameof(AuditEntity.SubjectIdentifier)}, "andy")
                .Build()
                .Compile();

            var matchResult = criteria(new AuditEntity() { SubjectIdentifier = "1231-324-43243",  Subject = "andy" });
            Assert.True(matchResult);

             matchResult = criteria(new AuditEntity() { SubjectIdentifier = "andy", Subject = "fred" });
            Assert.True(matchResult);


            matchResult = criteria(new AuditEntity() { SubjectIdentifier = "rubbish", Subject = "fred" });
            Assert.False(matchResult);
        }

        ExpressionCriteriaBuilder<AuditEntity> CreateSut()
        {
            return new ExpressionCriteriaBuilder<AuditEntity>();
        }
    }
}