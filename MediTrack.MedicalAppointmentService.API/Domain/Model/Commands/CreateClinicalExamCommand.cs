namespace MediTrack.MedicalAppointmentService.API.Domain.Model.Commands;

public record CreateClinicalExamCommand(
    int PatientId,
    int? AppointmentId,
    string ExamType,
    DateTime? ScheduledDate,
    DateTime? PickupDate);