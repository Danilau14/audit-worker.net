namespace AuditWorker.Interfaces;


public interface IAuditRecordRepository : IBaseRepository<AuditRecorder>
{
    public Task<AuditRecorder> CreateAuditRecorder(AuditRecorder auditRecorder);
}