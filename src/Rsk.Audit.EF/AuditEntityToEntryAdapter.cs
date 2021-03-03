using System;
using System.Collections.Generic;

namespace RSK.Audit.EF
{
    internal class AuditEntityToEntryAdapter : IAuditEntry
    {
        private readonly AuditEntity entity;

        public AuditEntityToEntryAdapter(AuditEntity entity)
        {
            this.entity = entity;
        }

        public DateTime When => entity.When;
        public string Source => entity.Source;
        public ResourceActor Subject => CreateActorResource();

        private ResourceActor CreateActorResource()
        {
            switch (entity.SubjectType)
            {
                case ResourceActor.MachineSubjectType:
                    return new MachineResourceActor(entity.SubjectIdentifier,entity.Subject);

                case ResourceActor.UserSubjectType:
                    return new UserResourceActor(entity.SubjectIdentifier,entity.Subject);
            }

            return new ResourceActor(entity.SubjectType, entity.SubjectIdentifier,entity.Subject);
        }

        public string Action => entity.Action;
        public AuditableResource Resource => new AuditableResource(entity.ResourceType,entity.ResourceIdentifier,entity.Resource);
        public bool Succeeded => entity.Succeeded;
        public string Description => entity.Description;

        public override bool Equals(object obj)
        {
            var other = obj as IAuditEntry;

            var result =
                other != null &&
                other.When == When &&
                other.Source == Source &&
                other.Subject.Equals(Subject) &&
                other.Action == Action &&
                other.Resource.Equals(Resource) &&
                other.Succeeded == Succeeded &&
                other.Description == Description;

            return result;
        }

        public override int GetHashCode()
        {
            var hashCode = -1538883855;
            hashCode = hashCode * -1521134295 + EqualityComparer<AuditEntity>.Default.GetHashCode(entity);
            hashCode = hashCode * -1521134295 + When.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Source);
            hashCode = hashCode * -1521134295 + EqualityComparer<ResourceActor>.Default.GetHashCode(Subject);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Action);
            hashCode = hashCode * -1521134295 + EqualityComparer<AuditableResource>.Default.GetHashCode(Resource);
            hashCode = hashCode * -1521134295 + Succeeded.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            return hashCode;
        }

        public override string ToString()
        {
            return $"{nameof(When)}: {When}, {nameof(Source)}: {Source}, {nameof(Subject)}: {Subject}, {nameof(Action)}: {Action}, {nameof(Resource)}: {Resource}, {nameof(Succeeded)}: {Succeeded}, {nameof(Description)}: {Description}";
        }
    }
}