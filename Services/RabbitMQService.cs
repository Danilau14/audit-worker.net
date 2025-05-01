using AuditWorker.Interfaces;

namespace AuditWorker.Services;

public class RabbitMQService : IRabbitMQService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly RabbitMQSettings _rabbitMQSettings;
    private IConnection _connection;
    private IChannel _channel;

    public RabbitMQService(
        IOptions<RabbitMQSettings> rabbitMQSettings,
         IServiceScopeFactory scopeFactory
    )
    {
        _rabbitMQSettings = rabbitMQSettings.Value;
        _scopeFactory = scopeFactory;
    }

    private async Task EnsureConnectionAndChannelAsync()
    {
        if (_connection == null || !_connection.IsOpen)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = _rabbitMQSettings.HostName,
                UserName = _rabbitMQSettings.UserName,
                Password = _rabbitMQSettings.Password,
                Port = _rabbitMQSettings.Port
            };

            _connection = await connectionFactory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
        }
    }

    public async Task PublishMessage(MessageDto message, string? queueName = null)
    {
        await EnsureConnectionAndChannelAsync();

        await _channel.QueueDeclareAsync(
            queueName ?? _rabbitMQSettings.QueueName, 
            durable: true, 
            exclusive: false, 
            autoDelete: false, 
            arguments: null
        );

        var messageSerilized = JsonSerializer.Serialize(message);

        var body = Encoding.UTF8.GetBytes(messageSerilized);

        await _channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: queueName ?? _rabbitMQSettings.QueueName,
            body: body
           );

        Console.WriteLine($" [x] Sent {message}");
    }

    public async Task ConsumeMessagesAsync(string? queueName = null)
    {
        await EnsureConnectionAndChannelAsync();

        await _channel.QueueDeclareAsync(
            queueName ?? _rabbitMQSettings.QueueName, 
            durable: true, 
            exclusive: false, 
            autoDelete: false, 
            arguments: null
        );

        var consumer = new AsyncEventingBasicConsumer(_channel);
        var message = string.Empty;

        consumer.ReceivedAsync += async (model, eventArgs) =>
        {
            try
            {
                message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                if (queueName == null)
                {
                    var messageDto = JsonSerializer.Deserialize<MessageDto>(message);
                    if (messageDto != null)
                    {
                       await ProcessMessageAsync(messageDto);

                    }
                }
                else
                {
                    var email = JsonSerializer.Deserialize<EmailForUserDto>(message);
                    if(email != null)
                    {
                        await ProcessSendEmailAsync(email);
                    }
                }

                await _channel.BasicAckAsync(eventArgs.DeliveryTag, multiple:false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
                await _channel.BasicNackAsync(eventArgs.DeliveryTag, multiple: false, requeue: true);
            }
        };

        await _channel.BasicConsumeAsync(
            queueName ?? _rabbitMQSettings.QueueName, 
            autoAck: false, 
            consumer: consumer
        );
    }

    private async Task ProcessMessageAsync(MessageDto message)
    {
        using var scope = _scopeFactory.CreateScope();

        var auditService = scope.ServiceProvider.GetRequiredService<IAuditService>();

        await auditService.SaveAuditRecordAsync(message);
    }

    private async Task ProcessSendEmailAsync(EmailForUserDto email)
    {
        using var scope = _scopeFactory.CreateScope();

        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

        await emailService.SendEmailAsync(
                email.Email,
                email.Subject,
                email.Message
            );
    }
}
