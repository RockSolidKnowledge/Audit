using Xunit;
using RSK.Audit.EF;

namespace RSK.Audit.Tests.EF
{
    public class AuditEntityToEntryAdapterTests
    {
        [Fact]
        public void Subject_WhenCalledWithSubjectTypeUser_ShouldReturnUserResourceActor()
        {
            AuditEntity entity = new AuditEntity {SubjectType = "User"};
            var sut = CreateSut(entity);

            Assert.Equal(typeof(UserResourceActor),sut.Subject.GetType());
        }

        [Fact]
        public void Subject_WhenCalledWithSubjectTypeMachine_ShouldReturnMachineResourceActor()
        {
            AuditEntity entity = new AuditEntity() {SubjectType = "Machine"};

            var sut = CreateSut(entity);

            Assert.Equal(typeof(MachineResourceActor), sut.Subject.GetType());
        }

        private AuditEntityToEntryAdapter CreateSut(AuditEntity entity)
        {
           return new AuditEntityToEntryAdapter(entity);
        }
    }
}