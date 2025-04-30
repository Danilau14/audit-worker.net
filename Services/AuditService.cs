namespace AuditWorker.Services;

public class AuditService : IAuditService
{
    private readonly IAuditRecordRepository _auditRecordRepository;

    public AuditService(IAuditRecordRepository auditRecordRepository)
    {
        _auditRecordRepository = auditRecordRepository;
    }

    public async Task SaveAuditRecordAsync(MessageDto message)
    {
        var record = new AuditRecorder
        {
            Action = message.Action,
            State = message.State,
            CreatedAt = message.CreatedAt,
            Response = message.Response,
            Entity = message.Entity
        };

        await _auditRecordRepository.CreateAuditRecorder(record);
    }
}
