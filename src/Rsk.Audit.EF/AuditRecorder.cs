using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("RSK.Audit.Test")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace RSK.Audit.EF
{
    internal class AuditRecorder : IRecordAuditableActions
    {
        private readonly string source;
        private readonly IUnitOfOWorkFactory uowFactory;

        private readonly Func<DateTime> now;

        public AuditRecorder(string source , IUnitOfOWorkFactory uowFactory) : this(source, uowFactory, () => DateTime.Now.ToUniversalTime())
        {

        }

        public AuditRecorder(string source , IUnitOfOWorkFactory uowFactory, Func<DateTime> now)
        {
            if ( source == null) throw new ArgumentNullException(nameof(source));

            if (String.IsNullOrWhiteSpace(source)) throw new ArgumentException("Can not be empty",nameof(source));

            this.source = source;
            this.uowFactory = uowFactory;

            this.now = now;
        }
        public  Task RecordSuccess(IAuditEventArguments context)
        {
            return WriteAuditRecord(context , true);
        }

        public Task RecordFailure(IAuditEventArguments context)
        {
            return WriteAuditRecord(context, false);
        }

        private async Task WriteAuditRecord(IAuditEventArguments context, bool succeeded)
        {
            try
            {
                using (IUnitOfWork uow = uowFactory.Create())
                {
                    uow.AuditEntries.Add(new AuditEntity()
                    {
                        Source = source,
                        SubjectIdentifier = context.Actor.Identifier,
                        Subject =  context.Actor.DisplayName,
                        SubjectType = context.Actor.Type,
                        Succeeded = succeeded,
                        When = now(),
                        Action = context.Action,
                        Resource = context.Resource.Name,
                        ResourceType = context.Resource.Type,
                        ResourceIdentifier = context.Resource.Identifier,
                        Description = context.Description.ToString()
                    });

                    await uow.Commit();
                }
            }
            catch (Exception error)
            {
                throw new AuditWriteException($"Failed to write audit entry {context.Actor}:{context.Action}:{context.Description}:{succeeded}",error);
            }
        }

        
    }
}