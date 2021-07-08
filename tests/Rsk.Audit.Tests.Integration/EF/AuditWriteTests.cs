using System.Linq;
using Microsoft.EntityFrameworkCore;
using RSK.Audit;
using RSK.Audit.EF;
using Xunit;
using AuditProviderFactory = RSK.Audit.AuditProviderFactory;

namespace Rsk.Audit.Tests.Integration.EF
{
    [Collection("AuditWriteTests")]
    public class AuditWriteTests
    {
        private const string AuditSource = "Test";
        private readonly AuditDatabaseContext databaseContext;
        private readonly AuditProviderFactory auditProviderFactory;

        public AuditWriteTests()
        {
            var options = new DbContextOptionsBuilder<AuditDatabaseContext>()
                .UseInMemoryDatabase(nameof(AuditWriteTests))
                .Options;

            databaseContext = new AuditDatabaseContext(options);

            databaseContext.Database.EnsureDeleted();
            databaseContext.Database.EnsureCreated();

            auditProviderFactory = new RSK.Audit.EF.AuditProviderFactory(options);
        }
      
        [Fact]
        public void GivenIHaveAnAuditSource_WhenAnAttemptIsMadeToRecordSuccess_ThenShouldWriteNewAuditEntryInDatabase()
        {
            var expectedSubject = new UserResourceActor("andy");
            const string expectedtedAction = "Login";
            var expectedResource = new AuditableResource("Client", "3232-4343-342-34123", "AdminUI");
            const string expectedDescription = "Logging in";

            var sut = CreateSut();

            sut.RecordSuccess(AuditContextMockingHelper.CreateAuditEventContext( expectedSubject,expectedtedAction,expectedResource,expectedDescription)).Wait();

            var auditEntries = databaseContext.AuditEntries.Where(ae => ae.Succeeded &&
                                                                      ae.SubjectIdentifier == expectedSubject.Identifier &&
                                                                      ae.SubjectType == expectedSubject.Type &&
                                                                      ae.Action == expectedtedAction &&
                                                                      ae.Resource == expectedResource.Name &&
                                                                      ae.ResourceType == expectedResource.Type &&
                                                                      ae.ResourceIdentifier == expectedResource.Identifier &&
                                                                      ae.Description == expectedDescription).ToList();

            Assert.Single(auditEntries);
        }

        [Fact]
        public void GivenIHaveAnAuditSource_WhenAnAttemptIsMadeToRecordFailure_ThenShouldWriteNewAuditEntryInDatabase()
        {
            var expectedSubject = new UserResourceActor("andy");
            const string expectedtedAction = "Login";
            var expectedResource = new AuditableResource("Client","3232-4343-342-34123","AdminUI");
            const string expectedDescription = "Logging in";

            var sut = CreateSut();

            sut.RecordFailure(AuditContextMockingHelper.CreateAuditEventContext(expectedSubject, expectedtedAction, expectedResource,expectedDescription)).Wait();

            var auditEntries = databaseContext.AuditEntries.Where(ae => !ae.Succeeded &&
                                                                        ae.SubjectIdentifier == expectedSubject.Identifier &&
                                                                        ae.SubjectType == expectedSubject.Type &&
                                                                        ae.Action == expectedtedAction &&
                                                                        ae.Resource == expectedResource.Name &&
                                                                        ae.ResourceType == expectedResource.Type &&
                                                                        ae.ResourceIdentifier == expectedResource.Identifier &&
                                                                        ae.Description == expectedDescription).ToList();

            Assert.Single(auditEntries);
        }

        private IRecordAuditableActions CreateSut()
        {
            return auditProviderFactory.CreateAuditSource(AuditSource);
        }
    }
}
