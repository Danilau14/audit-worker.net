namespace AuditWorker.Workers;

public class Worker : BackgroundService
{
    private readonly IRabbitMQService _rabbitMQService;

    public Worker(IRabbitMQService rabbitMQService)
    {
        _rabbitMQService = rabbitMQService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _rabbitMQService.ConsumeMessagesAsync();
            await _rabbitMQService.ConsumeMessagesAsync("email");

            await Task.Delay(1000, stoppingToken);
        }
    }
}
