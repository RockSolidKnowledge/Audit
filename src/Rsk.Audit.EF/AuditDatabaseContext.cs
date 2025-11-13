using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RSK.Audit.EF
{

    internal class AuditDatabseUnitOfWorkFactory : IUnitOfOWorkFactory
    {
        private readonly DbContextOptions<AuditDatabaseContext> options;
        private readonly string dbSchema;

        public AuditDatabseUnitOfWorkFactory(DbContextOptions<AuditDatabaseContext> options, string dbSchema = null)
        {
            this.options = options;
            this.dbSchema = dbSchema;
        }

        public IUnitOfWork Create()
        {
            if (!string.IsNullOrEmpty(dbSchema))
            {
                return new AuditDatabaseContext(options, dbSchema);
            }
            return new AuditDatabaseContext(options);
        }
    }

    public class AuditDatabaseContext : DbContext,IUnitOfWork
    {
        public AuditDatabaseContext(DbContextOptions<AuditDatabaseContext> options) : base(options)
        {
               
        }
        
        public AuditDatabaseContext(DbContextOptions<AuditDatabaseContext> options, string schema) : base(options)
        {
            Schema = schema;
        }
        
        public string Schema { get; set; }
        
        public DbSet<AuditEntity> AuditEntries { get; set; }
        
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (!string.IsNullOrEmpty(Schema))
            {
                modelBuilder.Model.SetOrRemoveAnnotation("Relational:DefaultSchema", Schema);
            }
            
            modelBuilder.Entity<AuditEntity>()
                .HasKey(ae => ae.Id);

            modelBuilder.Entity<AuditEntity>()
                .HasIndex(ae => ae.When);

            modelBuilder.Entity<AuditEntity>()
                .Property(e => e.When);
            
          
            base.OnModelCreating(modelBuilder);
        }

        
        public Task Commit()
        {
            return SaveChangesAsync();
        }
    }
}