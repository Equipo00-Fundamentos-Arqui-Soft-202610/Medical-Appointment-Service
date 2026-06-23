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
            AppointmentId: clinicalExam.AppointmentId,
            ExamType: clinicalExam.ExamType,
            ScheduledDate: clinicalExam.ScheduledDate,
            PickupDate: clinicalExam.PickupDate,
            Status: clinicalExam.Status.Value);
    }

    public ICollection<ClinicalExamResource> ToResources(ICollection<ClinicalExam> clinicalExams)
    {
        return clinicalExams.Select(ToResource).ToList();
    }
}