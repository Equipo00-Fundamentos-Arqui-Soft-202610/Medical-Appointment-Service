using MediTrack.MedicalAppointmentService.API.Domain.Model.ValueObjects;

namespace MediTrack.MedicalAppointmentService.API.Domain.Model.Aggregates;

public class ClinicalExam
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int? AppointmentId { get; set; }
    public string ExamType { get; set; } = null!;
    public DateTime? ScheduledDate { get; set; }
    public DateTime? PickupDate { get; set; }
    public ClinicalExamStatus Status { get; set; } = ClinicalExamStatus.Pending;

    public MedicalAppointment? Appointment { get; set; }

    public ClinicalExam() { }

    public ClinicalExam(
        int patientId,
        string examType,
        DateTime? scheduledDate = null,
        DateTime? pickupDate = null,
        int? appointmentId = null)
    {
        if (patientId <= 0)
            throw new ArgumentException("PatientId must be greater than 0", nameof(patientId));

        if (string.IsNullOrWhiteSpace(examType))
            throw new ArgumentException("ExamType cannot be empty or null", nameof(examType));

        PatientId = patientId;
        ExamType = examType.Trim();
        ScheduledDate = scheduledDate;
        PickupDate = pickupDate;
        AppointmentId = appointmentId;
        Status = ClinicalExamStatus.Pending;
    }

    public void MarkAsReady()
    {
        if (!Status.IsPending)
            throw new ArgumentException("Only pending exams can be marked as ready");

        Status = ClinicalExamStatus.Ready;
    }

    public void MarkAsCollected()
    {
        if (!Status.IsReady)
            throw new ArgumentException("Only ready exams can be marked as collected");

        Status = ClinicalExamStatus.Collected;
    }
}