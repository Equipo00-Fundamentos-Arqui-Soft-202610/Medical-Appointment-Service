using MediTrack.MedicalAppointmentService.API.Domain.Model.Aggregates;
using MediTrack.MedicalAppointmentService.API.Interfaces.REST.Resources;

namespace MediTrack.MedicalAppointmentService.API.Interfaces.REST.Transform;

public class ClinicalExamResourceFromEntityAssembler
{
    public ClinicalExamResource ToResource(ClinicalExam clinicalExam)
    {
        return new ClinicalExamResource(
            Id: clinicalExam.Id,
            PatientId: clinicalExam.PatientId,
            ExamType: clinicalExam.ExamType,
            PickupDate: clinicalExam.PickupDate,
            LaboratoryName: clinicalExam.LaboratoryName,
            Status: clinicalExam.Status.Value,
            CreatedAt: clinicalExam.CreatedAt,
            UpdatedAt: clinicalExam.UpdatedAt);
    }

    public ICollection<ClinicalExamResource> ToResources(ICollection<ClinicalExam> clinicalExams)
    {
        return clinicalExams.Select(ToResource).ToList();
    }
}
