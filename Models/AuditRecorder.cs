namespace AuditWorker.Models;

public class AuditRecorder
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public required string Entity { get; set; }
    public required Actions Action { get; set; }
    public required bool State { get; set; }
    public required string Response { get; set; }
}
