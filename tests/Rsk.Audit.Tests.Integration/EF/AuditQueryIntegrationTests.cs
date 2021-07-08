using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RSK.Audit;
using RSK.Audit.EF;
using Xunit;
using AuditProviderFactory = RSK.Audit.AuditProviderFactory;

namespace Rsk.Audit.Tests.Integration.EF
{
    [Collection("AuditQueryIntegrationTests")]
    public class AuditQueryIntegrationTests
    {
        private readonly AuditProviderFactory auditProviderFactory;
        private readonly DbContextOptions<AuditDatabaseContext> dbContextOptions;

        public AuditQueryIntegrationTests()
        {
            dbContextOptions = new DbContextOptionsBuilder<AuditDatabaseContext>()
                .UseInMemoryDatabase(nameof(AuditQueryIntegrationTests))
                .Options;

            var databaseContext = new AuditDatabaseContext(dbContextOptions);
            databaseContext.Database.EnsureDeleted();
            databaseContext.Database.EnsureCreated();

            auditProviderFactory = new RSK.Audit.EF.AuditProviderFactory(dbContextOptions);
        }

        [Fact]
        public async Task GivenAQuery_ShouldReturnRecordsBetweenDatesSpecifiedAndNoMore()
        {
            var expectedFirstDateTime = new DateTime(2018, 3, 2, 10, 20, 10);
            var auditFrequency = TimeSpan.FromSeconds(3);
            var expectedNumberOfRecords = 104;
            var expectedEndDateTime = expectedFirstDateTime.Add(TimeSpan.FromSeconds(auditFrequency.TotalSeconds * expectedNumberOfRecords));

            WriteAuditRecords("AdminUI", "Login", expectedFirstDateTime.Subtract(TimeSpan.FromSeconds(1)), 1, auditFrequency);

            var expectedRows = WriteAuditRecords("AdminUI", "Login", expectedFirstDateTime, expectedNumberOfRecords, auditFrequency);
            WriteAuditRecords("AdminUI", "Login", expectedEndDateTime.AddSeconds(1), 1, auditFrequency);

            var sut = CreateSut();

            var results = await sut.Between(expectedFirstDateTime, expectedEndDateTime)
                .ExecuteAscending();

            Assert.Equal(expectedNumberOfRecords, results.TotalNumberOfRows);
            Assert.Equal(expectedNumberOfRecords, results.Rows.Count());
            Assert.Equal(expectedRows.Select(r => new AuditEntityToEntryAdapter(r)), results.Rows);
        }


        [Fact]
        public async Task GivenAQueryFilteringByResourceIdentifierOrName_ShouldReturnRecordsThatMatchIdentifierInGivenTimeRange()
        {
            var resourceIdentifierToFind = Guid.NewGuid().ToString();
            var expectedFirstDateTime = new DateTime(2018, 3, 2, 10, 20, 10);
            var auditFrequency = TimeSpan.FromSeconds(3);
            var expectedNumberOfRecords = 104;
            var expectedEndDateTime = expectedFirstDateTime.Add(TimeSpan.FromSeconds(auditFrequency.TotalSeconds * expectedNumberOfRecords));

            WriteAuditRecords("AdminUI", "Login", expectedFirstDateTime.Subtract(TimeSpan.FromSeconds(1)),
                1, auditFrequency, resourceIdentifier: resourceIdentifierToFind);

            WriteAuditRecords("AdminUI", "Login", expectedFirstDateTime.Subtract(TimeSpan.FromSeconds(1)),
                1, auditFrequency, resourceIdentifier: "Not required");

            WriteAuditRecords("AdminUI", "Login", expectedFirstDateTime, expectedNumberOfRecords, auditFrequency);

            var expectedRows = WriteAuditRecords("AdminUI", "Login", expectedFirstDateTime, expectedNumberOfRecords, auditFrequency,
                resourceIdentifier: resourceIdentifierToFind);

            WriteAuditRecords("AdminUI", "Login", expectedEndDateTime.AddSeconds(1), 1, auditFrequency);

            var sut = CreateSut();

            var results = await sut.Between(expectedFirstDateTime, expectedEndDateTime)
                .AndResource(Matches.Exactly, resourceIdentifierToFind)
                .ExecuteAscending();

            Assert.Equal(expectedNumberOfRecords, results.TotalNumberOfRows);
            Assert.Equal(expectedNumberOfRecords, results.Rows.Count());
            Assert.Equal(expectedRows.Select(r => new AuditEntityToEntryAdapter(r)), results.Rows);
        }

        [Fact]
        public async Task GivenAQueryForPageTwo_ShouldReturnRecordsBetweenDatesSpecifiedForPageTwoAndNoMore()
        {
            const int page = 2;
            const int pageSize = 10;
            const int expectedNumberOfPages = 11;

            var expectedFirstDateTime = new DateTime(2018, 3, 2, 10, 20, 10);
            var auditFrequency = TimeSpan.FromSeconds(3);
            var expectedNumberOfRecords = 104;
            var expectedEndDateTime = expectedFirstDateTime.Add(TimeSpan.FromSeconds(auditFrequency.TotalSeconds * expectedNumberOfRecords));

            WriteAuditRecords("AdminUI", "Login", expectedFirstDateTime.Subtract(TimeSpan.FromSeconds(1)),
                1, auditFrequency);

            var expectedRows =
                WriteAuditRecords("AdminUI", "Login", expectedFirstDateTime, expectedNumberOfRecords, auditFrequency)
                    .OrderByDescending(ae => ae.When);
            var expectedPage = expectedRows.Skip((page - 1) * pageSize).Take(pageSize);

            WriteAuditRecords("AdminUI", "Login", expectedEndDateTime.AddSeconds(1), 1, auditFrequency);

            var sut = CreateSut();

            var results = await sut.Between(expectedFirstDateTime, expectedEndDateTime, page, pageSize)
                .ExecuteDescending();

            Assert.Equal(expectedNumberOfRecords, results.TotalNumberOfRows);
            Assert.Equal(page, results.Page);
            Assert.Equal(expectedNumberOfPages, results.TotalNumberOfPages);
            Assert.Equal(expectedPage.ToAuditEntries(), results.Rows);
        }

        [Fact]
        public async Task GivenAQuerySortedBySubject_ShouldReturnRecordsSortedBySubject()
        {
            var auditFrequency = TimeSpan.FromSeconds(3);
            var startTime = DateTime.Now;

            var fredRows = WriteAuditRecords("AdminUI", "Login",
                startTime,
                5, auditFrequency, subject: "fred");

            var andyRows = WriteAuditRecords("AdminUI", "Login",
                DateTime.Now,
                5, auditFrequency, subject: "andy");

            var expectedRows = fredRows.Concat(andyRows).OrderBy(ae => ae.SubjectIdentifier).ToList();

            var sut = CreateSut();

            var results = await sut.Between(startTime, expectedRows.Last().When.ToLocalTime().AddSeconds(3))
                .ExecuteAscending(AuditQuerySort.Subject);

            var expectedNumberOfRecords = expectedRows.Count();

            Assert.Equal(expectedNumberOfRecords, results.TotalNumberOfRows);
            Assert.Equal(expectedNumberOfRecords, results.Rows.Count());
            Assert.Equal(expectedRows.ToAuditEntries(), results.Rows);
        }

        [Fact]
        public async Task GivenAQueryFilteredByAction_ShouldReturnRecordsMatchingAction()
        {
            var auditFrequency = TimeSpan.FromSeconds(3);
            var startTime = DateTime.Now;

            var loginRows = WriteAuditRecords("AdminUI", "Login",
                startTime,
                5, auditFrequency, subject: "fred");

            var logoutRows = WriteAuditRecords("AdminUI", "Logout",
                DateTime.Now,
                5, auditFrequency, subject: "andy");

            var sut = CreateSut();

            var results = await sut.Between(startTime, loginRows.Last().When.ToLocalTime().AddSeconds(3))
                .AndAction(Matches.SomethingLike, "Logi")
                .ExecuteAscending(AuditQuerySort.Subject);

            var expectedNumberOfRecords = loginRows.Count();

            Assert.Equal(expectedNumberOfRecords, results.TotalNumberOfRows);
            Assert.Equal(expectedNumberOfRecords, results.Rows.Count());
            Assert.Equal(loginRows.ToAuditEntries(), results.Rows);
        }


        [Fact]
        public async Task GivenAQueryFilteredByAction_ShouldReturnRecordsMatchingActionsIgnoringCase()
        {
            var auditFrequency = TimeSpan.FromSeconds(3);
            var startTime = DateTime.Now;

            var loginRows = WriteAuditRecords("AdminUI", "Login",
                startTime,
                5, auditFrequency, subject: "fred");

            var logoutRows = WriteAuditRecords("AdminUI", "Logout",
                DateTime.Now,
                5, auditFrequency, subject: "andy");

            var sut = CreateSut();

            var results = await sut.Between(startTime, loginRows.Last().When.ToLocalTime().AddSeconds(3))
                .AndAction(Matches.StartsWith, "logi")
                .ExecuteAscending(AuditQuerySort.Subject);

            var expectedNumberOfRecords = loginRows.Count();

            Assert.Equal(expectedNumberOfRecords, results.TotalNumberOfRows);
            Assert.Equal(expectedNumberOfRecords, results.Rows.Count());
            Assert.Equal(loginRows.ToAuditEntries(), results.Rows);
        }

        [Fact]
        public async Task GivenAQueryFilteredBySubject_ShouldReturnRecordsMatchingSubjectIgnoringCase()
        {
            var auditFrequency = TimeSpan.FromSeconds(3);
            var startTime = DateTime.Now;

            var loginRows = WriteAuditRecords("AdminUI", "Login",
                startTime,
                5, auditFrequency, subject: "fred");

            var logoutRows = WriteAuditRecords("AdminUI", "Logout",
                DateTime.Now,
                5, auditFrequency, subject: "andy");

            var sut = CreateSut();

            var results = await sut.Between(startTime, loginRows.Last().When.ToLocalTime().AddSeconds(3))
                .AndSubject(Matches.StartsWith, "fred")
                .ExecuteAscending(AuditQuerySort.Subject);

            var expectedNumberOfRecords = loginRows.Count();

            Assert.Equal(expectedNumberOfRecords, results.TotalNumberOfRows);
            Assert.Equal(expectedNumberOfRecords, results.Rows.Count());
            Assert.Equal(loginRows.ToAuditEntries(), results.Rows);
        }

        [Fact]
        public async Task GivenAQueryFilteredBySource_ShouldReturnRecordsMatchingSourceIgnoringCase()
        {
            var auditFrequency = TimeSpan.FromSeconds(3);
            var startTime = DateTime.Now;

            var loginRows = WriteAuditRecords("AdminUI", "Login",
                startTime,
                5, auditFrequency, subject: "fred");

            var sut = CreateSut();

            var results = await sut.Between(startTime, loginRows.Last().When.ToLocalTime().AddSeconds(3))
                .AndSource(Matches.StartsWith, "adminui")
                .ExecuteAscending(AuditQuerySort.Subject);

            var expectedNumberOfRecords = loginRows.Count();

            Assert.Equal(expectedNumberOfRecords, results.TotalNumberOfRows);
            Assert.Equal(expectedNumberOfRecords, results.Rows.Count());
            Assert.Equal(loginRows.ToAuditEntries(), results.Rows);
        }

        [Fact]
        public async Task GivenAQueryFilteredByResource_ShouldReturnRecordsMatchingResourceIgnoringCase()
        {
            var auditFrequency = TimeSpan.FromSeconds(3);
            var startTime = DateTime.Now;

            var loginRows = WriteAuditRecords("AdminUI", "Login",
                startTime,
                5, auditFrequency, subject: "fred", resource: "admin_ui");

            var sut = CreateSut();

            var results = await sut.Between(startTime, loginRows.Last().When.ToLocalTime().AddSeconds(3))
                .AndResource(Matches.StartsWith, "Admin_UI")
                .ExecuteAscending(AuditQuerySort.Subject);

            var expectedNumberOfRecords = loginRows.Count();

            Assert.Equal(expectedNumberOfRecords, results.TotalNumberOfRows);
            Assert.Equal(expectedNumberOfRecords, results.Rows.Count());
            Assert.Equal(loginRows.ToAuditEntries(), results.Rows);
        }

        private IQueryableAuditableActions CreateSut()
        {
            return auditProviderFactory.CreateAuditQuery();
        }

        private List<AuditEntity> WriteAuditRecords(string source, string action, DateTime from, int number, TimeSpan frequency,
            bool success = true, string subject = "andy", string subjectIdentifier = "3232-232198", string description = "",
            string resource = "Some Resource", string resourceType = "client", string resourceIdentifier = "342432-3256-4624")
        {
            var auditEntries = new List<AuditEntity>();
            DateTime when = from;

            using (var context = new AuditDatabaseContext(dbContextOptions))
            {
                for (int nAuditRecordIndex = 0; nAuditRecordIndex < number; nAuditRecordIndex++)
                {
                    var nextAuditEntry = new AuditEntity()
                    {
                        When = when.ToUniversalTime(),
                        Succeeded = success,
                        Subject = subject,
                        SubjectIdentifier = subjectIdentifier,
                        SubjectType = "User",
                        Source = source,
                        Description = description,
                        Action = action,
                        Resource = resource,
                        ResourceType = resourceType,
                        ResourceIdentifier = resourceIdentifier
                    };

                    auditEntries.Add(nextAuditEntry);

                    when = when.Add(frequency);
                }

                context.AuditEntries.AddRange(auditEntries);

                context.SaveChanges();
            }

            return auditEntries;
        }
    }
}