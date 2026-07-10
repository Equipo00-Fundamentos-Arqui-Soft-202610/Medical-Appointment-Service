using System.Text.Json;
using MediTrack.MedicalAppointmentService.API.Infrastructure.Persistence.EFC;
using MediTrack.MedicalAppointmentService.API.Infrastructure.Persistence.EFC.Configuration;

namespace MediTrack.MedicalAppointmentService.API.Infrastructure.Messaging;

/// <summary>
/// Publica vía el patrón Outbox: en vez de mandar a RabbitMQ de inmediato (y
/// perder el evento en silencio si el broker está caído justo en ese momento),
/// persiste el mensaje y deja que <see cref="OutboxDispatcherHostedService"/>
/// lo entregue en background con reintentos.
/// </summary>
public sealed class OutboxEventPublisher : IEventPublisher
{
    private readonly MedicalAppointmentDbContext _context;

    public OutboxEventPublisher(MedicalAppointmentDbContext context)
    {
        _context = context;
    }

    public async Task PublishAsync(string routingKey, object payload)
    {
        var message = new OutboxMessage
        {
            EventType = routingKey,
            Payload = JsonSerializer.Serialize(payload),
            OccurredAtUtc = DateTime.UtcNow
        };

        _context.OutboxMessages.Add(message);
        await _context.SaveChangesAsync();
    }
}
