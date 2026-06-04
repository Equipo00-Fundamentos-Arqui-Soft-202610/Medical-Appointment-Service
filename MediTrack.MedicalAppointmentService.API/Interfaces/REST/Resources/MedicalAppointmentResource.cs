namespace MediTrack.MedicalAppointmentService.API.Interfaces.REST.Resources;

public record ScheduleAppointmentResource(
    int PatientId,
    string Type,
    DateTime ScheduledAt,
    string? Location,
    ICollection<string>? Requirements);

public record UpdateAppointmentResource(
    string Type,
    DateTime ScheduledAt,
    string? Location,
    ICollection<string>? Requirements);

public record RegisterAppointmentAttendanceResource(string Status);

public record AppointmentRequirementResource(
    int Id,
    string Description);

public record MedicalAppointmentResource(
    int Id,
    int PatientId,
    string Type,
    DateTime ScheduledAt,
    string? Location,
    string Status,
    bool CanBeModified,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    ICollection<AppointmentRequirementResource> Requirements);
