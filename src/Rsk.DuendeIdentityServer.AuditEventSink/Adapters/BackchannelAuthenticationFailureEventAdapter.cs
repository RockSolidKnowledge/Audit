﻿using System;
using Duende.IdentityServer.Events;
using RSK.Audit;

namespace Rsk.DuendeIdentityServer.AuditEventSink.Adapters
{
    public class BackchannelAuthenticationFailureEventAdapter : IAuditEventArguments
    {
        private readonly BackchannelAuthenticationFailureEvent evt;

        public BackchannelAuthenticationFailureEventAdapter(BackchannelAuthenticationFailureEvent evt)
        {
            this.evt = evt ?? throw new ArgumentNullException(nameof(evt));
        }

        public ResourceActor Actor => new ResourceActor(ResourceActor.UserSubjectType, evt.ClientId, evt.ClientName);
        public string Action => evt.Name;
        public AuditableResource Resource => new AuditableResource("IdentityServer", evt.ClientId);
        public FormattedString Description => evt.ToString().SafeForFormatted();
    }
}
