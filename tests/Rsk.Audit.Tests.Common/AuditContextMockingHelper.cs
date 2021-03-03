using System.Collections.Generic;
using System.Linq;
using Moq;
using RSK.Audit;
using RSK.Audit.EF;

namespace Rsk.Audit.Tests
{
    public static class AuditContextMockingHelper
    {
        public static IEnumerable<IAuditEntry> ToAuditEntries(this IEnumerable<AuditEntity> entities)
        {
            return entities.Select(e => new AuditEntityToEntryAdapter(e));
        }
        public static IAuditEventArguments CreateAuditEventContext(ResourceActor subject, string action, AuditableResource expectedResource,
            string expectedDescription)
        {
            Mock<IAuditEventArguments> context = new Mock<IAuditEventArguments>();

            context.Setup(c => c.Description).Returns(expectedDescription);
            context.Setup(c => c.Actor).Returns(subject);
            context.Setup(c => c.Resource).Returns(expectedResource);
            context.Setup(c => c.Action).Returns(action);

            return context.Object;
        }
    }
}