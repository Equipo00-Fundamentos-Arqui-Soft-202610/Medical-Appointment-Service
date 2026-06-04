using MediTrack.MedicalAppointmentService.API.Domain.Model.Aggregates;
using MediTrack.MedicalAppointmentService.API.Domain.Model.Commands;

namespace MediTrack.MedicalAppointmentService.API.Domain.Model;

public interface IClinicalExamCommandService
{
    Task<ClinicalExam> HandleAsync(CreateClinicalExamCommand command);
    Task<ClinicalExam> HandleAsync(MarkClinicalExamPickedUpCommand command);
}
