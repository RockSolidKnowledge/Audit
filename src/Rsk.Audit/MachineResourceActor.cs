using System;

namespace RSK.Audit
{
    public class MachineResourceActor : ResourceActor
    {
        public MachineResourceActor(string clientId) : this(clientId, String.Empty)
        {
        }

        public MachineResourceActor(string clientId, string knownAs) : base(ResourceActor.MachineSubjectType, clientId,knownAs)
        {
        }
    }
}