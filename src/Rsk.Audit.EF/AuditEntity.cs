using System;

namespace RSK.Audit.EF
{
   
    public class AuditEntity 
    {

        public long Id { get; set; }
        public DateTime When { get; set; }
        public string Source { get; set; }
        public string SubjectType { get; set; }
        public string SubjectIdentifier { get; set; }
        public string Subject { get; set; }

        public string Action { get; set; }
        public string ResourceType { get; set; }
        public string Resource { get; set; }
        public string ResourceIdentifier { get; set; }

        public bool Succeeded { get; set; }
        public string Description { get; set; }


        public string NormalisedSubject
        {
            get => Subject?.ToUpperInvariant();
            set => Ignore(value); 
        }

        public string NormalisedAction
        {
            get => Action?.ToUpperInvariant();
            set => Ignore(value);
        }

        public string NormalisedResource
        {
            get => Resource?.ToUpperInvariant();
            set => Ignore(value);
        }

        public string NormalisedSource
        {
            get => Source?.ToUpperInvariant();
            set => Ignore(value);
        }

        private static void Ignore(string value)
        {
            return;
        }

        protected bool Equals(AuditEntity other)
        {
            return Id == other.Id && When.Equals(other.When) && string.Equals(Source, other.Source) && string.Equals(SubjectType, other.SubjectType) && string.Equals(SubjectIdentifier, other.SubjectIdentifier) && string.Equals(Subject, other.Subject) && string.Equals(Action, other.Action) && string.Equals(ResourceType, other.ResourceType) && string.Equals(Resource, other.Resource) && string.Equals(ResourceIdentifier, other.ResourceIdentifier) && Succeeded == other.Succeeded && string.Equals(Description, other.Description);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AuditEntity) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ When.GetHashCode();
                hashCode = (hashCode * 397) ^ (Source != null ? Source.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (SubjectType != null ? SubjectType.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (SubjectIdentifier != null ? SubjectIdentifier.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Subject != null ? Subject.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Action != null ? Action.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ResourceType != null ? ResourceType.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Resource != null ? Resource.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ResourceIdentifier != null ? ResourceIdentifier.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Succeeded.GetHashCode();
                hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                return hashCode;
            }
        }

     
    }
}