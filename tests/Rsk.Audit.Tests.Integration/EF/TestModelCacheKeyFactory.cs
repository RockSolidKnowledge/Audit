using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.InMemory.Infrastructure.Internal;

namespace Rsk.Audit.Tests.Integration.EF;

public class TestModelCacheKeyFactory : IModelCacheKeyFactory
{
    public object Create(DbContext context, bool designTime)
    {
        // Extract connection string from options without accessing the database
        var connectionString = string.Empty;
        var extension = context.GetService<IDbContextOptions>()
            ?.FindExtension<InMemoryOptionsExtension>();
        if (extension != null)
        {
            connectionString = extension.StoreName ?? string.Empty;
        }
        
        return (context.GetType(), connectionString, designTime);
    }
}