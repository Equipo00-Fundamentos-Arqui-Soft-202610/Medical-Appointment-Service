using MediTrack.MedicalAppointmentService.API.Domain.Model.Aggregates;

namespace MediTrack.MedicalAppointmentService.API.Domain.Model;

public interface IClinicalExamRepository
{
    Task<ClinicalExam?> FindByIdAsync(int clinicalExamId);
    Task<ICollection<ClinicalExam>> FindPendingByPatientIdAsync(int patientId);
    Task AddAsync(ClinicalExam clinicalExam);
    Task UpdateAsync(ClinicalExam clinicalExam);
}
