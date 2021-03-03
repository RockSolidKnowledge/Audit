#if NETCOREAPP2_1
using System.Data.SqlClient;
#else
using Microsoft.Data.SqlClient;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
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
        private AuditDatabaseContext databaseContext;

        private AuditProviderFactory auditProviderFactory;
        private DbContextOptions<AuditDatabaseContext> dbContextOptions;

        public AuditQueryIntegrationTests()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AuditDatabaseContext>();

            string connectionString = @"server=.\SQLEXPRESS;database=AuditQueryIntegrationTests;integrated security=true";
            string masterConnectionString = @"server=.\SQLEXPRESS;database=master;integrated security=true";

            dbContextOptions = optionsBuilder
                .UseSqlServer(connectionString).Options;

            databaseContext = new AuditDatabaseContext(dbContextOptions);
            databaseContext.Database.EnsureDeleted();

            ExecuteCommand("CREATE DATABASE AuditQueryIntegrationTests", masterConnectionString);
            ExecuteCommand("ALTER DATABASE audit SET SINGLE_USER WITH ROLLBACK IMMEDIATE", masterConnectionString);
            ExecuteCommand("ALTER DATABASE audit COLLATE Latin1_General_CS_AS", masterConnectionString);
            ExecuteCommand("ALTER DATABASE audit SET MULTI_USER", masterConnectionString);

            SqlConnection.ClearAllPools();
            databaseContext.Database.EnsureCreated();

            auditProviderFactory = new RSK.Audit.EF.AuditProviderFactory(dbContextOptions);
        }

        [Fact]
        public void GivenAQuery_ShouldReturnRecordsBetweenDatesSpecifiedAndNoMore()
        {
            DateTime expectedFirstDateTime = new DateTime(2018, 3, 2, 10, 20, 10);
            TimeSpan auditFrequency = TimeSpan.FromSeconds(3);
            int expectedNumberOfRecords = 104;
            DateTime expectedEndDateTime = expectedFirstDateTime.Add(
                TimeSpan.FromSeconds(auditFrequency.TotalSeconds * expectedNumberOfRecords));

            WriteAuditRecords("AdminUI", "Login", expectedFirstDateTime.Subtract(TimeSpan.FromSeconds(1)),
                1, auditFrequency);

            var expectedRows = WriteAuditRecords("AdminUI", "Login", expectedFirstDateTime, expectedNumberOfRecords, auditFrequency);
            WriteAuditRecords("AdminUI", "Login", expectedEndDateTime.AddSeconds(1), 1, auditFrequency);

            var sut = CreateSut();

            var results = sut.Between(expectedFirstDateTime, expectedEndDateTime)
                .ExecuteAscending()
                .Result;

            Assert.Equal(expectedNumberOfRecords, results.TotalNumberOfRows);
            Assert.Equal(expectedNumberOfRecords, results.Rows.Count());
            Assert.Equal(expectedRows.Select(r => new AuditEntityToEntryAdapter(r)), results.Rows);
        }


        [Fact]
        public void GivenAQueryFilteringByResourceIdentifierOrName_ShouldReturnRecordsThatMatchIdentifierInGivenTimeRange()
        {
            string resourceIdentifierToFind = Guid.NewGuid().ToString();
            DateTime expectedFirstDateTime = new DateTime(2018, 3, 2, 10, 20, 10);
            TimeSpan auditFrequency = TimeSpan.FromSeconds(3);
            int expectedNumberOfRecords = 104;
            DateTime expectedEndDateTime = expectedFirstDateTime.Add(
                TimeSpan.FromSeconds(auditFrequency.TotalSeconds * expectedNumberOfRecords));

            WriteAuditRecords("AdminUI", "Login", expectedFirstDateTime.Subtract(TimeSpan.FromSeconds(1)),
                1, auditFrequency, resourceIdentifier: resourceIdentifierToFind);

            WriteAuditRecords("AdminUI", "Login", expectedFirstDateTime.Subtract(TimeSpan.FromSeconds(1)),
                1, auditFrequency, resourceIdentifier: "Not required");

            WriteAuditRecords("AdminUI", "Login", expectedFirstDateTime, expectedNumberOfRecords, auditFrequency);

            var expectedRows = WriteAuditRecords("AdminUI", "Login", expectedFirstDateTime, expectedNumberOfRecords, auditFrequency,
                resourceIdentifier: resourceIdentifierToFind);

            WriteAuditRecords("AdminUI", "Login", expectedEndDateTime.AddSeconds(1), 1, auditFrequency);

            var sut = CreateSut();

            var results = sut.Between(expectedFirstDateTime, expectedEndDateTime)
                .AndResource(Matches.Exactly, resourceIdentifierToFind)
                .ExecuteAscending()
                .Result;

            Assert.Equal(expectedNumberOfRecords, results.TotalNumberOfRows);
            Assert.Equal(expectedNumberOfRecords, results.Rows.Count());
            Assert.Equal(expectedRows.Select(r => new AuditEntityToEntryAdapter(r)), results.Rows);
        }

        [Fact]
        public void GivenAQueryForPageTwo_ShouldReturnRecordsBetweenDatesSpecifiedForPageTwoAndNoMore()
        {
            const int page = 2;
            const int pageSize = 10;
            const int expectedNumberOfPages = 11;

            DateTime expectedFirstDateTime = new DateTime(2018, 3, 2, 10, 20, 10);
            TimeSpan auditFrequency = TimeSpan.FromSeconds(3);
            int expectedNumberOfRecords = 104;
            DateTime expectedEndDateTime = expectedFirstDateTime.Add(
                TimeSpan.FromSeconds(auditFrequency.TotalSeconds * expectedNumberOfRecords));

            WriteAuditRecords("AdminUI", "Login", expectedFirstDateTime.Subtract(TimeSpan.FromSeconds(1)),
                1, auditFrequency);

            var expectedRows =
                WriteAuditRecords("AdminUI", "Login", expectedFirstDateTime, expectedNumberOfRecords, auditFrequency)
                    .OrderByDescending(ae => ae.When);
            var expectedPage = expectedRows.Skip((page - 1) * pageSize).Take(pageSize);

            WriteAuditRecords("AdminUI", "Login", expectedEndDateTime.AddSeconds(1), 1, auditFrequency);

            var sut = CreateSut();

            var results = sut.Between(expectedFirstDateTime, expectedEndDateTime, page, pageSize)
                .ExecuteDescending()
                .Result;

            Assert.Equal(expectedNumberOfRecords, results.TotalNumberOfRows);
            Assert.Equal(page, results.Page);
            Assert.Equal(expectedNumberOfPages, results.TotalNumberOfPages);
            Assert.Equal(expectedPage.ToAuditEntries(), results.Rows);
        }

        [Fact]
        public void GivenAQuerySortedBySubject_ShouldReturnRecordsSortedBySubject()
        {
            TimeSpan auditFrequency = TimeSpan.FromSeconds(3);

            var startTime = DateTime.Now;

            var fredRows = WriteAuditRecords("AdminUI", "Login",
                startTime,
                5, auditFrequency, subject: "fred");

            var andyRows = WriteAuditRecords("AdminUI", "Login",
                DateTime.Now,
                5, auditFrequency, subject: "andy");

            var expectedRows = fredRows.Concat(andyRows).OrderBy(ae => ae.SubjectIdentifier);

            var sut = CreateSut();

            var results = sut.Between(startTime,
                    expectedRows.Last().When.ToLocalTime().AddSeconds(3))
                .ExecuteAscending(AuditQuerySort.Subject)
                .Result;

            var expectedNumberOfRecords = expectedRows.Count();

            Assert.Equal(expectedNumberOfRecords, results.TotalNumberOfRows);
            Assert.Equal(expectedNumberOfRecords, results.Rows.Count());
            Assert.Equal(expectedRows.ToAuditEntries(), results.Rows);
        }

        [Fact]
        public void GivenAQueryFilteredByAction_ShouldReturnRecordsMatchingAction()
        {
            TimeSpan auditFrequency = TimeSpan.FromSeconds(3);

            var startTime = DateTime.Now;

            var loginRows = WriteAuditRecords("AdminUI", "Login",
                startTime,
                5, auditFrequency, subject: "fred");

            var logoutRows = WriteAuditRecords("AdminUI", "Logout",
                DateTime.Now,
                5, auditFrequency, subject: "andy");

            var expectedRows = loginRows;

            var sut = CreateSut();

            var results = sut.Between(startTime, expectedRows.Last().When.ToLocalTime().AddSeconds(3))
                .AndAction(Matches.SomethingLike, "Logi")
                .ExecuteAscending(AuditQuerySort.Subject)
                .Result;

            var expectedNumberOfRecords = expectedRows.Count();

            Assert.Equal(expectedNumberOfRecords, results.TotalNumberOfRows);
            Assert.Equal(expectedNumberOfRecords, results.Rows.Count());
            Assert.Equal(expectedRows.ToAuditEntries(), results.Rows);
        }


        [Fact]
        public void GivenAQueryFilteredByAction_ShouldReturnRecordsMatchingActionsIgnoringCase()
        {
            TimeSpan auditFrequency = TimeSpan.FromSeconds(3);

            var startTime = DateTime.Now;

            var loginRows = WriteAuditRecords("AdminUI", "Login",
                startTime,
                5, auditFrequency, subject: "fred");

            var logoutRows = WriteAuditRecords("AdminUI", "Logout",
                DateTime.Now,
                5, auditFrequency, subject: "andy");

            var expectedRows = loginRows;

            var sut = CreateSut();

            var results = sut.Between(startTime, expectedRows.Last().When.ToLocalTime().AddSeconds(3))
                .AndAction(Matches.StartsWith, "logi")
                .ExecuteAscending(AuditQuerySort.Subject)
                .Result;

            var expectedNumberOfRecords = expectedRows.Count();

            Assert.Equal(expectedNumberOfRecords, results.TotalNumberOfRows);
            Assert.Equal(expectedNumberOfRecords, results.Rows.Count());
            Assert.Equal(expectedRows.ToAuditEntries(), results.Rows);
        }

        [Fact]
        public void GivenAQueryFilteredBySubject_ShouldReturnRecordsMatchingSubjectIgnoringCase()
        {
            TimeSpan auditFrequency = TimeSpan.FromSeconds(3);

            var startTime = DateTime.Now;

            var loginRows = WriteAuditRecords("AdminUI", "Login",
                startTime,
                5, auditFrequency, subject: "fred");

            var logoutRows = WriteAuditRecords("AdminUI", "Logout",
                DateTime.Now,
                5, auditFrequency, subject: "andy");

            var expectedRows = loginRows;

            var sut = CreateSut();

            var results = sut.Between(startTime, expectedRows.Last().When.ToLocalTime().AddSeconds(3))
                .AndSubject(Matches.StartsWith, "fred")
                .ExecuteAscending(AuditQuerySort.Subject)
                .Result;

            var expectedNumberOfRecords = expectedRows.Count();

            Assert.Equal(expectedNumberOfRecords, results.TotalNumberOfRows);
            Assert.Equal(expectedNumberOfRecords, results.Rows.Count());
            Assert.Equal(expectedRows.ToAuditEntries(), results.Rows);
        }

        [Fact]
        public void GivenAQueryFilteredBySource_ShouldReturnRecordsMatchingSourceIgnoringCase()
        {
            TimeSpan auditFrequency = TimeSpan.FromSeconds(3);

            var startTime = DateTime.Now;

            var loginRows = WriteAuditRecords("AdminUI", "Login",
                startTime,
                5, auditFrequency, subject: "fred");

            var expectedRows = loginRows;

            var sut = CreateSut();

            var results = sut.Between(startTime, expectedRows.Last().When.ToLocalTime().AddSeconds(3))
                .AndSource(Matches.StartsWith, "adminui")
                .ExecuteAscending(AuditQuerySort.Subject)
                .Result;

            var expectedNumberOfRecords = expectedRows.Count();

            Assert.Equal(expectedNumberOfRecords, results.TotalNumberOfRows);
            Assert.Equal(expectedNumberOfRecords, results.Rows.Count());
            Assert.Equal(expectedRows.ToAuditEntries(), results.Rows);
        }

        [Fact]
        public void GivenAQueryFilteredByResource_ShouldReturnRecordsMatchingResourceIgnoringCase()
        {
            TimeSpan auditFrequency = TimeSpan.FromSeconds(3);

            var startTime = DateTime.Now;

            var loginRows = WriteAuditRecords("AdminUI", "Login",
                startTime,
                5, auditFrequency, subject: "fred", resource: "admin_ui");


            var expectedRows = loginRows;

            var sut = CreateSut();

            var results = sut.Between(startTime, expectedRows.Last().When.ToLocalTime().AddSeconds(3))
                .AndResource(Matches.StartsWith, "Admin_UI")
                .ExecuteAscending(AuditQuerySort.Subject)
                .Result;

            var expectedNumberOfRecords = expectedRows.Count();

            Assert.Equal(expectedNumberOfRecords, results.TotalNumberOfRows);
            Assert.Equal(expectedNumberOfRecords, results.Rows.Count());
            Assert.Equal(expectedRows.ToAuditEntries(), results.Rows);
        }

        IQueryableAuditableActions CreateSut()
        {
            return auditProviderFactory.CreateAuditQuery();
        }

        private List<AuditEntity> WriteAuditRecords(string source, string action, DateTime from, int number, TimeSpan frequency,
            bool success = true, string subject = "andy", string subjectIdentifier = "3232-232198", string description = "",
            string resource = "Some Resource", string resourceType = "client", string resourceIdentifier = "342432-3256-4624")
        {
            List<AuditEntity> auditEntries = new List<AuditEntity>();
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

        private void ExecuteCommand(string commandString, string connectionString)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = commandString;
                    conn.Open();
                    int result = cmd.ExecuteNonQuery();

                    return;
                }
            }
        }
    }
}