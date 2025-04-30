namespace AuditWorker.Interfaces;

public interface IRabbitMQService
{
    public Task PublishMessage(MessageDto message, string? queueName = null);
    public Task ConsumeMessagesAsync(string? queueName = null);
}
