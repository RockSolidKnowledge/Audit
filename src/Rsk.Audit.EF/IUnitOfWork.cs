using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RSK.Audit.EF
{
    internal interface IUnitOfWork : IDisposable
    {
        DbSet<AuditEntity> AuditEntries { get; }

        Task Commit();
    }
}