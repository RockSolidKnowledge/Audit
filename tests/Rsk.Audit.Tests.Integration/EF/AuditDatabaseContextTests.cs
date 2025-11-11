using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using RSK.Audit.EF;
using Xunit;

namespace Rsk.Audit.Tests.Integration.EF;

[Collection("AuditDatabaseContextTests")]

public class AuditDatabaseContextTests
{
    [Fact]
    public void Ctor_WhenCalledWithSchema_ExpectThatSchemaSetInTheDatabase()
    {
        string schema = "guest";
        
        var dbContextOptions = new DbContextOptionsBuilder<AuditDatabaseContext>()
            .ReplaceService<IModelCacheKeyFactory, TestModelCacheKeyFactory>()
            .UseInMemoryDatabase(nameof(AuditQueryIntegrationTests) + "WithSchema")
            .Options;

        var databaseContext = new AuditDatabaseContext(dbContextOptions, schema);
        databaseContext.Database.EnsureDeleted();
        databaseContext.Database.EnsureCreated();
        
        Assert.Equal(schema, databaseContext.Schema);
        Assert.Equal(schema, databaseContext.Model["Relational:DefaultSchema"]!.ToString());
    }
    
    [Fact]
    public void Ctor_WhenCalledWithoutSchema_ExpectNoSchemaSetInTheDatabase()
    {
        var dbContextOptions = new DbContextOptionsBuilder<AuditDatabaseContext>()
            .ReplaceService<IModelCacheKeyFactory, TestModelCacheKeyFactory>()
            .UseInMemoryDatabase(nameof(AuditQueryIntegrationTests) + "WithoutSchema")
            .Options;

        var databaseContext = new AuditDatabaseContext(dbContextOptions);
        databaseContext.Database.EnsureDeleted();
        databaseContext.Database.EnsureCreated();
        
        Assert.Null(databaseContext.Schema);
        Assert.Null(databaseContext.Model["Relational:DefaultSchema"]?.ToString());
    }
}