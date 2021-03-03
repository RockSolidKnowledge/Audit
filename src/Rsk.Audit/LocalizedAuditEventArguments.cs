using System;
using Microsoft.Extensions.Localization;

namespace RSK.Audit
{
    internal class LocalizedAuditEventArguments : AuditEventArgumentsDecorator
    {
        private readonly IStringLocalizer localizer;

        public LocalizedAuditEventArguments(IAuditEventArguments toLocalize,IStringLocalizer localizer):base(toLocalize)
        {
            if (toLocalize == null) throw new ArgumentNullException(nameof(toLocalize));
            this.localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));

            Description = new FormattedString(localizer[toLocalize.Description.Format]?.Value ?? "",toLocalize.Description.Arguments);
        }

        public override FormattedString Description { get; }

        public override string Action => localizer[base.Action].Value;
    }
}