using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace MediTrack.MedicalAppointmentService.API.Infrastructure.Messaging;

public interface IEventPublisher
{
    Task PublishAsync(string routingKey, object payload);
}

public class RabbitMqPublisher : IEventPublisher
{
    private const string DefaultExchange = "meditrack.events";
    private readonly IConfiguration _configuration;

    public RabbitMqPublisher(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task PublishAsync(string routingKey, object payload)
    {
        var rabbitMqSection = _configuration.GetSection("RabbitMq");
        var exchangeName = rabbitMqSection["ExchangeName"] ?? DefaultExchange;

        var factory = new ConnectionFactory
        {
            HostName = rabbitMqSection["Host"] ?? "localhost",
            Port = rabbitMqSection.GetValue<int?>("Port") ?? AmqpTcpEndpoint.UseDefaultPort,
            UserName = rabbitMqSection["UserName"] ?? "guest",
            Password = rabbitMqSection["Password"] ?? "guest",
            VirtualHost = rabbitMqSection["VirtualHost"] ?? "/"
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(
            exchange: exchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false);

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;
        properties.ContentType = "application/json";

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(payload));

        channel.BasicPublish(
            exchange: exchangeName,
            routingKey: routingKey,
            basicProperties: properties,
            body: body);

        return Task.CompletedTask;
    }
}
