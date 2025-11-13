using Microsoft.EntityFrameworkCore;

namespace RSK.Audit.EF
{
    public class AuditProviderFactory : Audit.AuditProviderFactory
    {
        private readonly DbContextOptions<AuditDatabaseContext> options;
        private readonly string dbSchema;

        public AuditProviderFactory(DbContextOptions<AuditDatabaseContext> options, string dbSchema = null)
        {
            this.options = options;
            this.dbSchema = dbSchema;
        }

        public override IRecordAuditableActions CreateAuditSource(string source)
        {
            var uwf = !string.IsNullOrWhiteSpace(dbSchema) ? 
                new AuditDatabseUnitOfWorkFactory(options, dbSchema) : 
                new AuditDatabseUnitOfWorkFactory(options);

            return new AuditRecorder(source,uwf);
        }

        public override IQueryableAuditableActions CreateAuditQuery()
        {
            var uwf = !string.IsNullOrWhiteSpace(dbSchema) ? 
                new AuditDatabseUnitOfWorkFactory(options, dbSchema) : 
                new AuditDatabseUnitOfWorkFactory(options);

            return new AuditQueryFactory(uwf);
        }
    }
}