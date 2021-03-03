using Microsoft.EntityFrameworkCore;

namespace RSK.Audit.EF
{
    public class AuditProviderFactory : Audit.AuditProviderFactory
    {
        private readonly DbContextOptions<AuditDatabaseContext> options;

        public AuditProviderFactory(DbContextOptions<AuditDatabaseContext> options)
        {
            this.options = options;
        }

        public override IRecordAuditableActions CreateAuditSource(string source)
        {
            var uwf = new AuditDatabseUnitOfWorkFactory(options);

            return new AuditRecorder(source,uwf);
        }

        public override IQueryableAuditableActions CreateAuditQuery()
        {
            var uwf = new AuditDatabseUnitOfWorkFactory(options);

            return new AuditQueryFactory(uwf);
        }
    }
}