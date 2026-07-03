namespace MediTrack.MedicalAppointmentService.API.Application.OutboundEvents;

public record ExamenCreadoEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
    public int PatientId { get; init; }
    public long ExamId { get; init; }
    public string ExamType { get; init; } = string.Empty;
    public DateTime? PickupDate { get; init; }
}
