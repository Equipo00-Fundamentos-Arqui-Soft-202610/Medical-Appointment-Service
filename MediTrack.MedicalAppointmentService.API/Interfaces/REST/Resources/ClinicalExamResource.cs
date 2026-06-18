namespace MediTrack.MedicalAppointmentService.API.Interfaces.REST.Resources;

public record CreateClinicalExamResource(
    int PatientId,
    int? AppointmentId,
    string ExamType,
    DateTime? ScheduledDate,
    DateTime? PickupDate);

public record ClinicalExamResource(
    int Id,
    int PatientId,
    int? AppointmentId,
    string ExamType,
    DateTime? ScheduledDate,
    DateTime? PickupDate,
    string Status);