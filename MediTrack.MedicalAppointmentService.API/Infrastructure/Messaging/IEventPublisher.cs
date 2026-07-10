namespace MediTrack.MedicalAppointmentService.API.Infrastructure.Messaging;

public interface IEventPublisher
{
    Task PublishAsync(string routingKey, object payload);
}
