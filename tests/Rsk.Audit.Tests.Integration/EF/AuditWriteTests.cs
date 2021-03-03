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
        private string auditSource = "Test";
        private AuditDatabaseContext databaseContext;

        private AuditProviderFactory auditProviderFactory;

        public AuditWriteTests()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AuditDatabaseContext>();

            var options = optionsBuilder
                .UseSqlServer(@"server=.\SQLEXPRESS;database=AuditWriteTests;integrated security=true").Options;
              
           databaseContext = new AuditDatabaseContext(options);

            databaseContext.Database.EnsureDeleted();
            databaseContext.Database.EnsureCreated();

            auditProviderFactory = new RSK.Audit.EF.AuditProviderFactory(options);
        }
      
        [Fact]
        public void GivenIHaveAnAuditSource_WhenAnAttemptIsMadeToRecordSuccess_ThenShouldWriteNewAuditEntryInDatabase()
        {
            var expectedSubject = new UserResourceActor("andy");
            string expectedtedAction = "Login";
            string expectedDescription = "Logging in";
            var expectedResource = new AuditableResource("Client", "3232-4343-342-34123", "AdminUI");

            var sut = CreateSut();

            sut.RecordSuccess(AuditContextMockingHelper.CreateAuditEventContext( expectedSubject,expectedtedAction,expectedResource,expectedDescription)).Wait();

            var auditEntry = databaseContext.AuditEntries.Single(ae => ae.Succeeded && 
                                                                 ae.SubjectIdentifier == expectedSubject.Identifier &&
                                                                 ae.SubjectType == expectedSubject.Type &&
                                                                 ae.Action == expectedtedAction &&
                                                                 ae.Resource == expectedResource.Name &&
                                                                 ae.ResourceType == expectedResource.Type &&
                                                                 ae.ResourceIdentifier == expectedResource.Identifier &&
                                                                 ae.Description == expectedDescription);

            Assert.True(true); // If there is not a single entry that passes then previous line will throw 
        }

        [Fact]
        public void GivenIHaveAnAuditSource_WhenAnAttemptIsMadeToRecordFailure_ThenShouldWriteNewAuditEntryInDatabase()
        {
            var expectedSubject = new UserResourceActor("andy");
            string expectedtedAction = "Login";
            string expectedDescription = "Logging in";
            var expectedResource = new AuditableResource("Client","3232-4343-342-34123","AdminUI");

            var sut = CreateSut();

            sut.RecordFailure(AuditContextMockingHelper.CreateAuditEventContext(expectedSubject, expectedtedAction, expectedResource,expectedDescription)).Wait();

            var auditEntry = databaseContext.AuditEntries.Single(ae => !ae.Succeeded &&
                                                                       ae.SubjectIdentifier == expectedSubject.Identifier &&
                                                                       ae.SubjectType == expectedSubject.Type &&
                                                                       ae.Action == expectedtedAction &&
                                                                       ae.Resource == expectedResource.Name &&
                                                                       ae.ResourceType == expectedResource.Type &&
                                                                       ae.ResourceIdentifier == expectedResource.Identifier &&
                                                                       ae.Description == expectedDescription);

            Assert.True(true); // If there is not a single entry that passes then previous line will throw 
        }

        IRecordAuditableActions CreateSut()
        {
            return auditProviderFactory.CreateAuditSource(auditSource);
        }
    }
}
