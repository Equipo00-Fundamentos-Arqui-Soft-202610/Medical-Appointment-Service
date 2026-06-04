using MediTrack.MedicalAppointmentService.API.Domain.Model.Aggregates;
using MediTrack.MedicalAppointmentService.API.Domain.Model.Queries;

namespace MediTrack.MedicalAppointmentService.API.Domain.Model;

public interface IClinicalExamQueryService
{
    Task<ICollection<ClinicalExam>> HandleAsync(GetPendingClinicalExamsByPatientIdQuery query);
}
