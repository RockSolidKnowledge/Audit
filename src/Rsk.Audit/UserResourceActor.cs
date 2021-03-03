using System;

namespace RSK.Audit
{

    /// <summary>
    /// A user resource actor
    /// </summary>
    public class UserResourceActor : ResourceActor
    {
        public UserResourceActor(string userIdentifier) : this(userIdentifier,String.Empty)
        {
        }

        public UserResourceActor(string userIdentifier, string knownAs) : base(ResourceActor.UserSubjectType, userIdentifier, knownAs)
        {
        }
    }
}