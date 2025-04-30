namespace AuditWorker.Interfaces;

public interface IAuditService
{
    Task SaveAuditRecordAsync(MessageDto message);
}
