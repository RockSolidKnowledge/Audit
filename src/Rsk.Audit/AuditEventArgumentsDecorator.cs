namespace RSK.Audit
{
    internal class AuditEventArgumentsDecorator : IAuditEventArguments
    {
        private readonly IAuditEventArguments toDecorate;

        public AuditEventArgumentsDecorator(IAuditEventArguments toDecorate)
        {
            this.toDecorate = toDecorate;
        }


        public ResourceActor Actor => toDecorate.Actor;

        public virtual string Action => toDecorate.Action;

        public virtual AuditableResource Resource => toDecorate.Resource;

        public virtual FormattedString Description => toDecorate.Description;
    }
}