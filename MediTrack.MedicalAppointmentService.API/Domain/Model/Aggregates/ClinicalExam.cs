using MediTrack.MedicalAppointmentService.API.Domain.Model.ValueObjects;

namespace MediTrack.MedicalAppointmentService.API.Domain.Model.Aggregates;

public class ClinicalExam
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string ExamType { get; set; } = null!;
    public DateTime PickupDate { get; set; }
    public string? LaboratoryName { get; set; }
    public ClinicalExamStatus Status { get; set; } = ClinicalExamStatus.PendingPickup;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ClinicalExam() { }

    public ClinicalExam(int patientId, string examType, DateTime pickupDate, string? laboratoryName = null)
    {
        if (patientId <= 0)
            throw new ArgumentException("PatientId must be greater than 0", nameof(patientId));

        if (string.IsNullOrWhiteSpace(examType))
            throw new ArgumentException("ExamType cannot be empty or null", nameof(examType));

        PatientId = patientId;
        ExamType = examType.Trim();
        PickupDate = pickupDate;
        LaboratoryName = string.IsNullOrWhiteSpace(laboratoryName) ? null : laboratoryName.Trim();
        Status = ClinicalExamStatus.PendingPickup;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsPickedUp()
    {
        if (!Status.IsPendingPickup)
            throw new ArgumentException("Only pending exams can be marked as picked up");

        Status = ClinicalExamStatus.PickedUp;
        UpdatedAt = DateTime.UtcNow;
    }
}
