using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace RSK.Audit
{
    internal class RecordLocalizedAuditableActions : IRecordAuditableActions
    {
        private readonly IRecordAuditableActions toDecorate;
        private readonly IStringLocalizer localizer;

        public RecordLocalizedAuditableActions(IRecordAuditableActions toDecorate, IStringLocalizer localizer)
        {
            this.toDecorate = toDecorate ?? throw new ArgumentNullException(nameof(toDecorate));
            this.localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }
        public Task RecordSuccess(IAuditEventArguments actionContext)
        {
            return Record( toDecorate.RecordSuccess,actionContext);
        }

        public Task RecordFailure(IAuditEventArguments actionContext)
        {
            return Record(toDecorate.RecordFailure, actionContext);
        }

        private Task Record(Func<IAuditEventArguments, Task> recordMethod, IAuditEventArguments actionContext)
        {

            actionContext = new LocalizedAuditEventArguments(actionContext,localizer);

            return recordMethod(actionContext);
        }
    }
}