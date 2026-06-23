namespace MediTrack.MedicalAppointmentService.API.Domain.Model.Commands;

public record ScheduleAppointmentCommand(
    int PatientId,
    string Type,
    DateTime ScheduledAt,
    string? Location,
    string? Notes,
    ICollection<string>? Requirements);