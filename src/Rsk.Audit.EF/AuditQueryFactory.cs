using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace RSK.Audit.EF
{
    internal class AuditQueryFactory : IQueryableAuditableActions
    {
        private readonly IUnitOfOWorkFactory uwf;

        public AuditQueryFactory(IUnitOfOWorkFactory uwf)
        {
            this.uwf = uwf ?? throw new ArgumentNullException(nameof(uwf));
        }

        public IAuditQuery Between(DateTime @from, DateTime to)
        {
            var baseQuery = CreateBaseQuery(@from, to);

            return new AuditQuery(baseQuery);
        }

       
        public IAuditQuery Between(DateTime @from, DateTime to, int page, int pageSize)
        {
            if ( page < 1) throw new ArgumentOutOfRangeException(nameof(page),"Page must be >= 1");
            if ( pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be >= 1");

            var baseQuery = CreateBaseQuery(@from, to);

            return new AuditQueryWithPaging(baseQuery, page, pageSize);
        }

        private IQueryable<AuditEntity> CreateBaseQuery(DateTime @from, DateTime to)
        {
            if (from > to)
            {
                throw new ArgumentOutOfRangeException(nameof(from));
            }
            IUnitOfWork uw = uwf.Create();

            var baseQuery = uw.AuditEntries.AsNoTracking()
                .Where(ae => ae.When >= @from.ToUniversalTime() &&
                             ae.When <= to.ToUniversalTime());
            return baseQuery;
        }

    }
}