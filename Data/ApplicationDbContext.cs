namespace AuditWorker.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<AuditRecorder> AuditRecorder { get; set; }
}
