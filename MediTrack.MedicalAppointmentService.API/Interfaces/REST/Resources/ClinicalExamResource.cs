namespace MediTrack.MedicalAppointmentService.API.Interfaces.REST.Resources;

public record CreateClinicalExamResource(
    int PatientId,
    string ExamType,
    DateTime PickupDate,
    string? LaboratoryName);

public record ClinicalExamResource(
    int Id,
    int PatientId,
    string ExamType,
    DateTime PickupDate,
    string? LaboratoryName,
    string Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
