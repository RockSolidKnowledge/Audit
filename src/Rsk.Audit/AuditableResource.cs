namespace RSK.Audit
{

    /// <summary>
    /// Represents a target for an action
    /// </summary>
    public class AuditableResource
    {
        /// <summary>
        /// Type of resource
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Display name given to the resource
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Unique identifier for the resource
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// Creates a resource for an audit record
        /// </summary>
        /// <param name="identifier">Unique identifier for the resource</param>
        public AuditableResource(string identifier) : this(null, identifier, null)
        {
          
        }

        /// <summary>
        /// Creates a resource for an audit record
        /// </summary>
        /// <param name="type">Type of resource</param>
        /// <param name="identifier">Unique identifier for the resource</param>
        public AuditableResource(string type, string identifier) : this(type, identifier, null)
        {
        }

        /// <summary>
        /// Creates a resource for an audit record
        /// </summary>
        /// <param name="type">Type of resource</param>
        /// <param name="identifier">Unique identifier for the resource</param>
        /// <param name="name">Display name of the resource</param>
        public AuditableResource(string type, string identifier , string name)
        {
            Type = type;
            Identifier = identifier;
            Name = name;
        }

       
        public override string ToString()
        {
            return $"Type: {Type}, Identifier:{Identifier} Name: {Name}";
        }

        protected bool Equals(AuditableResource other)
        {
            return string.Equals(Type, other.Type) && string.Equals(Name, other.Name) && string.Equals(Identifier, other.Identifier);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AuditableResource) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Type != null ? Type.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Identifier != null ? Identifier.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}