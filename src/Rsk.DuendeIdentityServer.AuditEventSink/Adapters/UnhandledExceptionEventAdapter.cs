﻿using System;
using Duende.IdentityServer.Events;
using RSK.Audit;

namespace Rsk.DuendeIdentityServer.AuditEventSink.Adapters
{
    public class UnhandledExceptionEventAdapter : IAuditEventArguments
    {
        private readonly UnhandledExceptionEvent evt;

        public UnhandledExceptionEventAdapter(UnhandledExceptionEvent evt)
        {
            this.evt = evt ?? throw new ArgumentNullException(nameof(evt));
        }

        public ResourceActor Actor => new ResourceActor(ResourceActor.MachineSubjectType, null, null);
        public string Action => evt.Name;
        public AuditableResource Resource => new AuditableResource("IdentityServer");
        public FormattedString Description => evt.ToString().SafeForFormatted();
    }
}
