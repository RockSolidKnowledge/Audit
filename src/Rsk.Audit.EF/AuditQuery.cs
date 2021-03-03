using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RSK.Audit.EF
{
    internal class AuditQuery : IAuditQuery
    {
        private readonly ICriteriaBuilder<AuditEntity> criteriaBuilder;


        protected virtual IQueryable<AuditEntity> ApplyAnyPaging(IQueryable<AuditEntity> query)
        {
            return query;
        }

        protected virtual IAuditQueryResult CreateResults(IEnumerable<AuditEntity> rows, long totalCount)
        {
            return new AuditQueryResult(rows.Select(r => new AuditEntityToEntryAdapter(r)), totalCount);
        }

        public AuditQuery(IQueryable<AuditEntity> baseQuery) : this(baseQuery,new ExpressionCriteriaBuilder<AuditEntity>())
        {
        }

        public AuditQuery(IQueryable<AuditEntity> baseQuery , ICriteriaBuilder<AuditEntity> criteriaBuilder)
        {
            this.criteriaBuilder = criteriaBuilder ?? throw new ArgumentNullException(nameof(criteriaBuilder));

            // Transforms inputs into upper case to 
            criteriaBuilder
                .AddStringTransform(nameof(AuditEntity.NormalisedAction), i => i.ToUpperInvariant())
                .AddStringTransform(nameof(AuditEntity.NormalisedSubject), i => i.ToUpperInvariant())
                .AddStringTransform(nameof(AuditEntity.NormalisedResource), i => i.ToUpperInvariant())
                .AddStringTransform(nameof(AuditEntity.NormalisedSource), i => i.ToUpperInvariant());

            Query = baseQuery ?? throw new ArgumentNullException(nameof(baseQuery));
        }

        public IQueryable<AuditEntity> Query { get; private set; }

        public IAuditQuery AndSubject(ResourceActor resourceActor)
        {
            criteriaBuilder.AndStringMatch(Matches.Exactly, nameof(AuditEntity.SubjectIdentifier), resourceActor.Identifier);
            criteriaBuilder.AndStringMatch(Matches.Exactly, nameof(AuditEntity.SubjectType), resourceActor.Type);

            return this;
        }

        public IAuditQuery AndSubject(Matches matchType ,string nameOrIdentifier)
        {
            criteriaBuilder.AndStringMatchAny(matchType,
                new string[] {nameof(AuditEntity.NormalisedSubject), nameof(AuditEntity.SubjectIdentifier)},
                nameOrIdentifier);

            return this;
        }

        public IAuditQuery AndResource(Matches matchType, string nameOrIdentifier)
        {
            criteriaBuilder.AndStringMatchAny(matchType,
                new string[] { nameof(AuditEntity.NormalisedResource), nameof(AuditEntity.ResourceIdentifier) },
                nameOrIdentifier);

            return this;
        }

        public IAuditQuery AndResource(AuditableResource resource)
        {
            criteriaBuilder.AndStringMatch(Matches.Exactly, nameof(AuditEntity.Resource), resource.Identifier);
            criteriaBuilder.AndStringMatch(Matches.Exactly, nameof(AuditEntity.ResourceType), resource.Type);

            return this;
        }

        public IAuditQuery AndSubjectIdentifier(Matches matchType, string subject)
        {
            criteriaBuilder.AndStringMatch(matchType, nameof(AuditEntity.SubjectIdentifier), subject);

            return this;
        }

        public IAuditQuery AndSubjectType(Matches matchType, string subjectType)
        {
            criteriaBuilder.AndStringMatch(matchType, nameof(AuditEntity.SubjectType), subjectType);

            return this;
        }

        public IAuditQuery AndSubjectKnownAs(Matches matchType, string subject)
        {
            criteriaBuilder.AndStringMatch(matchType, nameof(AuditEntity.NormalisedSubject), subject);

            return this;
        }


        public IAuditQuery AndSource(Matches matchType, string source)
        {
            criteriaBuilder.AndStringMatch(matchType, nameof(AuditEntity.NormalisedSource), source);

            return this;
        }

        public IAuditQuery AndAction(Matches matchType, string action)
        {
            criteriaBuilder.AndStringMatch(matchType, nameof(AuditEntity.NormalisedAction), action);

            return this;
        }

        public IAuditQuery AndResourceIdentifier(Matches matchType, string resourceIdentifier)
        {
            criteriaBuilder.AndStringMatch(matchType, nameof(AuditEntity.Resource), resourceIdentifier);

            return this;
        }

        public IAuditQuery AndResourceType(Matches matchType, string resourceType)
        {
            criteriaBuilder.AndStringMatch(matchType, nameof(AuditEntity.ResourceType), resourceType);

            return this;
        }

        public IAuditQuery AndDescription(Matches matchType, string description)
        {
            criteriaBuilder.AndStringMatch(matchType, nameof(AuditEntity.Description), description);

            return this;
        }

        public IAuditQuery AndAnyText(Matches matchType, string text)
        {
            criteriaBuilder.AndStringMatch(matchType, nameof(AuditEntity.Description), text);
            criteriaBuilder.OrStringMatch(matchType, nameof(AuditEntity.Action), text);
            criteriaBuilder.OrStringMatch(matchType, nameof(AuditEntity.Source), text);
            criteriaBuilder.OrStringMatch(matchType, nameof(AuditEntity.SubjectIdentifier), text);
            criteriaBuilder.OrStringMatch(matchType, nameof(AuditEntity.Resource), text);

            return this;
        }

        public IAuditQuery AndActionSucceeded()
        {
            criteriaBuilder.AndBoolean(nameof(AuditEntity.Succeeded), true);
            return this;
        }

        public IAuditQuery AndActionFailed()
        {
            criteriaBuilder.AndBoolean(nameof(AuditEntity.Succeeded), false);
            return this;
        }

        public  Task<IAuditQueryResult> ExecuteAscending()
        {
            return ExecuteAscending(AuditQuerySort.When);
        }

        public  Task<IAuditQueryResult> ExecuteDescending()
        {
            return ExecuteDescending(AuditQuerySort.When);
        }

        public virtual  Task<IAuditQueryResult> ExecuteAscending(AuditQuerySort sortColumn)
        {
            return Execute(sortColumn,AuditQuerySortDirection.Ascending);
        }

        public virtual Task<IAuditQueryResult> ExecuteDescending(AuditQuerySort sortColumn)
        {
            return Execute(sortColumn, AuditQuerySortDirection.Descending);
        }

        protected virtual Task<IAuditQueryResult> Execute(AuditQuerySort sortColumn , 
            AuditQuerySortDirection direction)
        {
            return Execute(sortColumn, direction, _ => _);
        }

        protected  async Task<IAuditQueryResult> Execute(AuditQuerySort sortColumn , 
            AuditQuerySortDirection direction , Func<IQueryable<AuditEntity>,IQueryable<AuditEntity>> applyPaging)
        {
            var query = Query
                .Where(criteriaBuilder.Build());

            long numberOfRows = await Count<AuditEntity>(query);

            var results = ToOrderedList(query , sortColumn,direction);

            return CreateResults(results, numberOfRows);
        }

        private static readonly Dictionary<AuditQuerySort, Expression<Func<AuditEntity, object>>>
            SortColumnSelectors = new Dictionary<AuditQuerySort, Expression<Func<AuditEntity, object>>>()
            {
                [AuditQuerySort.Source] = ae => ae.Source,
                [AuditQuerySort.When] = ae => ae.When,
                [AuditQuerySort.Action] = ae => ae.Action,
                [AuditQuerySort.Subject] = ae => ae.SubjectIdentifier,
                [AuditQuerySort.Success] = ae => ae.Succeeded,
                [AuditQuerySort.Description] = ae => ae.Description,
            };

        private IEnumerable<AuditEntity> ToOrderedList(IQueryable<AuditEntity> query , 
            AuditQuerySort sortColumn , AuditQuerySortDirection direction)
        {
            Expression<Func<AuditEntity, object>> sortColumnSelector = ae => ae.When;

            sortColumnSelector = SortColumnSelectors[sortColumn];

            query = direction == AuditQuerySortDirection.Descending ? query.OrderByDescending(sortColumnSelector) : query.OrderBy(sortColumnSelector);


            query = ApplyAnyPaging(query);

            return query;
        }

        private Task<long> Count<T>(IQueryable<T> query)
        {
            if (query is IAsyncEnumerable<T>)
            {
                return query.LongCountAsync();
            }

            return Task.FromResult(query.LongCount());
        }
    }
}