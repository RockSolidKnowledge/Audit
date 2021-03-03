using System.Collections.Generic;
using System.Linq;

namespace RSK.Audit.EF
{
    internal class AuditQueryWithPaging : AuditQuery
    {
        private readonly int page;
        private readonly int pageSize;

        public AuditQueryWithPaging(IQueryable<AuditEntity> baseQuery , int page , int pageSize) : base(baseQuery)
        {
            this.page = page;
            this.pageSize = pageSize;
        }

        protected override IQueryable<AuditEntity> ApplyAnyPaging(IQueryable<AuditEntity> query)
        {
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        protected override IAuditQueryResult CreateResults(IEnumerable<AuditEntity> rows, long totalCount)
        {
            int totalNumberOfPages = (int) (totalCount / pageSize) +
                                     (totalCount % pageSize > 0? 1: 0);

            return new AuditQueryResult(Enumerable.Select<AuditEntity, AuditEntityToEntryAdapter>(rows, r => new AuditEntityToEntryAdapter(r)), totalCount)
            {
                Page = page,
                TotalNumberOfPages = totalNumberOfPages
            };
        }
    }
}