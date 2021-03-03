using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RSK.Audit.EF
{

    internal class AuditDatabseUnitOfWorkFactory : IUnitOfOWorkFactory
    {
        private readonly DbContextOptions<AuditDatabaseContext> options;

        public AuditDatabseUnitOfWorkFactory(DbContextOptions<AuditDatabaseContext> options)
        {
            this.options = options;

        }

        public IUnitOfWork Create()
        {
            return new AuditDatabaseContext(options);
        }
    }

    public class AuditDatabaseContext : DbContext,IUnitOfWork
    {
        public AuditDatabaseContext(DbContextOptions<AuditDatabaseContext> options) : base(options)
        {
               
        }
        
        public DbSet<AuditEntity> AuditEntries { get; set; }
        
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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