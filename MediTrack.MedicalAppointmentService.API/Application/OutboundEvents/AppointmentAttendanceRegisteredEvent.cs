namespace MediTrack.MedicalAppointmentService.API.Application.OutboundEvents;

public record AppointmentAttendanceRegisteredEvent
{
    public int PatientId { get; init; }
    public int AppointmentId { get; init; }
    public string AttendanceStatus { get; init; } = string.Empty;
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
}
