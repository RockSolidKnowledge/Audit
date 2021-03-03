namespace RSK.Audit
{
    /// <summary>
    /// Describes the subject performing the action to be audited
    /// </summary>
    public  class ResourceActor
    {
        public const string UserSubjectType = "User";
        public const string MachineSubjectType = "Machine";
        /// <summary>
        /// Creates a ResourceActor that is used to describe the subject of an audited action
        /// </summary>
        /// <param name="type">type of actor, user,machine etc</param>
        /// <param name="identifier">Unique identifier of the actor</param>
        /// <param name="displayName">The display name of the actor</param>
        public ResourceActor(string type , string identifier , string displayName)
        {
            Type = type;
            Identifier = identifier;
            DisplayName = displayName;
        }
        
        /// <summary>
        /// The type of the actor ( machine, user etc )
        /// </summary>
        public string Type { get; }
        /// <summary>
        /// Unique identifier of the actor
        /// </summary>
        public string Identifier { get; }
        /// <summary>
        /// The display name of the actor
        /// </summary>
        public string DisplayName { get; }


        protected bool Equals(ResourceActor other)
        {
            return string.Equals(Type, other.Type) && string.Equals(Identifier, other.Identifier);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ResourceActor) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Type != null ? Type.GetHashCode() : 0) * 397) ^ (Identifier != null ? Identifier.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return $"{Type}:{Identifier}({DisplayName})";
        }
    }
}