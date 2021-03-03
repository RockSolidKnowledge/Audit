using RSK.Audit.EF;
using Xunit;

namespace RSK.Audit.Tests.EF
{
    public class ExpressionCriteriaFactoryTests
    {
        [Fact]
        public void CreateStringMatchExpression_WhenCalledWithEquals_ShouldCreateCriteria()
        {
            var sut = CreateSut();

            var criteria = sut.CreateStringMatchExpression<AuditEntity>(Matches.Exactly, "Action", "create").Compile();

            var matchResult = criteria(new AuditEntity() {Action = "create"});
            var missedResult = criteria(new AuditEntity() {Action = "update"});

            Assert.True(matchResult);
            Assert.False(missedResult);
        }

        [Fact]
        public void CreateStringMatchExpression_WhenCalledWithStartsWith_ShouldCreateCriteria()
        {
            var sut = CreateSut();

            var criteria = sut.CreateStringMatchExpression<AuditEntity>(Matches.StartsWith, "Action", "cr").Compile();

            var matchResult = criteria(new AuditEntity() { Action = "create" });
            var missedResult = criteria(new AuditEntity() { Action = "delete" });

            Assert.True(matchResult);
            Assert.False(missedResult);
        }

        ExpressionCriteriaFactory CreateSut()
        {
            return new ExpressionCriteriaFactory();
        }
    }
}