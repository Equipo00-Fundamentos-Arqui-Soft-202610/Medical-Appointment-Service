namespace MediTrack.MedicalAppointmentService.API.Domain.Model.Commands;

public record UpdateAppointmentCommand(
    int AppointmentId,
    string Type,
    DateTime ScheduledAt,
    string? Location,
    ICollection<string>? Requirements);
