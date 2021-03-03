using System.Threading.Tasks;

namespace RSK.Audit
{
    /// <summary>
    /// Used to record actions to be audited
    /// </summary>
    public interface IRecordAuditableActions
    {
       /// <summary>
       /// Records a successful action
       /// </summary>
       /// <param name="auditEventArguments">Information about the audited action</param>
       /// <returns></returns>
        Task RecordSuccess(IAuditEventArguments auditEventArguments);

        /// <summary>
        /// Records a succesful action
        /// </summary>
        /// <param name="auditEventArguments">Information about the audited action</param>
        /// <returns></returns>
        Task RecordFailure(IAuditEventArguments auditEventArguments);
       
    }
}