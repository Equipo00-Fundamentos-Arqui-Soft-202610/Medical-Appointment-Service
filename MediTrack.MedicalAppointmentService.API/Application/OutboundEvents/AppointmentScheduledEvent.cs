namespace MediTrack.MedicalAppointmentService.API.Application.OutboundEvents;

public record AppointmentScheduledEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
    public int PatientId { get; init; }
    public int AppointmentId { get; init; }
    public string AppointmentType { get; init; } = string.Empty;
    public string? Location { get; init; }
    public DateTime AppointmentDateUtc { get; init; }
}
