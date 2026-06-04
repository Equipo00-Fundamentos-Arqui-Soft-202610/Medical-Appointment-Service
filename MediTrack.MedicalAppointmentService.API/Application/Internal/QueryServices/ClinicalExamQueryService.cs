using MediTrack.MedicalAppointmentService.API.Domain.Model;
using MediTrack.MedicalAppointmentService.API.Domain.Model.Aggregates;
using MediTrack.MedicalAppointmentService.API.Domain.Model.Queries;

namespace MediTrack.MedicalAppointmentService.API.Application.Internal.QueryServices;

public class ClinicalExamQueryService : IClinicalExamQueryService
{
    private readonly IClinicalExamRepository _clinicalExamRepository;

    public ClinicalExamQueryService(IClinicalExamRepository clinicalExamRepository)
    {
        _clinicalExamRepository = clinicalExamRepository;
    }

    public async Task<ICollection<ClinicalExam>> HandleAsync(GetPendingClinicalExamsByPatientIdQuery query)
    {
        if (query.PatientId <= 0)
            throw new ArgumentException("PatientId must be greater than 0");

        return await _clinicalExamRepository.FindPendingByPatientIdAsync(query.PatientId);
    }
}
