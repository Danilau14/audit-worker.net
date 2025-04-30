
namespace AuditWorker.Repositories;

public class AuditRecordRepository : BaseRepository<AuditRecorder>, IAuditRecordRepository
{
    public AuditRecordRepository(ApplicationDbContext context) : base(context) { }

    public async Task<AuditRecorder> CreateAuditRecorder(AuditRecorder auditRecorder)
    {
        await _dbSet.AddAsync(auditRecorder);
        await _context.SaveChangesAsync();
        return auditRecorder;
    }
}
