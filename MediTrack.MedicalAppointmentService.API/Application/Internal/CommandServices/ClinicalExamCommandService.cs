using MediTrack.MedicalAppointmentService.API.Domain.Model;
using MediTrack.MedicalAppointmentService.API.Domain.Model.Aggregates;
using MediTrack.MedicalAppointmentService.API.Domain.Model.Commands;

namespace MediTrack.MedicalAppointmentService.API.Application.Internal.CommandServices;

public class ClinicalExamCommandService : IClinicalExamCommandService
{
    private readonly IClinicalExamRepository _clinicalExamRepository;

    public ClinicalExamCommandService(IClinicalExamRepository clinicalExamRepository)
    {
        _clinicalExamRepository = clinicalExamRepository;
    }

    public async Task<ClinicalExam> HandleAsync(CreateClinicalExamCommand command)
    {
        var clinicalExam = new ClinicalExam(
            patientId: command.PatientId,
            examType: command.ExamType,
            pickupDate: command.PickupDate,
            laboratoryName: command.LaboratoryName);

        await _clinicalExamRepository.AddAsync(clinicalExam);
        return clinicalExam;
    }

    public async Task<ClinicalExam> HandleAsync(MarkClinicalExamPickedUpCommand command)
    {
        var clinicalExam = await _clinicalExamRepository.FindByIdAsync(command.ClinicalExamId);
        if (clinicalExam == null)
            throw new ArgumentException($"Clinical exam with id {command.ClinicalExamId} not found");

        clinicalExam.MarkAsPickedUp();

        await _clinicalExamRepository.UpdateAsync(clinicalExam);
        return clinicalExam;
    }
}
