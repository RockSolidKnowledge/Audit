using System;
using Microsoft.Extensions.Localization;

namespace RSK.Audit
{
    /// <summary>
    /// Decorates an AuditProviderFactory to provide language translations for IAuditEventArguments ( Actions, Descriptions )
    /// </summary>
    public class AuditProviderFactoryWithLocalization : AuditProviderFactory
    {
        private readonly AuditProviderFactory auditProviderFactory;
        private readonly IStringLocalizer localizer;

        public AuditProviderFactoryWithLocalization(AuditProviderFactory auditProviderFactory, IStringLocalizer localizer)
        {
            this.auditProviderFactory = auditProviderFactory ?? throw new ArgumentNullException(nameof(auditProviderFactory));
            this.localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        public override IRecordAuditableActions CreateAuditSource(string source)
        {
            IRecordAuditableActions recordAuditableActions = auditProviderFactory.CreateAuditSource(source);
            
           return new RecordLocalizedAuditableActions(recordAuditableActions,localizer);
        }

        public override IQueryableAuditableActions CreateAuditQuery()
        {
            return auditProviderFactory.CreateAuditQuery();
        }
    }
}