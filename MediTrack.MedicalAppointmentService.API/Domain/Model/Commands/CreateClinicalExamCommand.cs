namespace MediTrack.MedicalAppointmentService.API.Domain.Model.Commands;

public record CreateClinicalExamCommand(
    int PatientId,
    string ExamType,
    DateTime PickupDate,
    string? LaboratoryName);
