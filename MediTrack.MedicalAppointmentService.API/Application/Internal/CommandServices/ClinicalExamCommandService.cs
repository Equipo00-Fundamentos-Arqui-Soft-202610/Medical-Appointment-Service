using MediTrack.MedicalAppointmentService.API.Application.OutboundEvents;
using MediTrack.MedicalAppointmentService.API.Domain.Model;
using MediTrack.MedicalAppointmentService.API.Domain.Model.Aggregates;
using MediTrack.MedicalAppointmentService.API.Domain.Model.Commands;
using MediTrack.MedicalAppointmentService.API.Infrastructure.Messaging;

namespace MediTrack.MedicalAppointmentService.API.Application.Internal.CommandServices;

public class ClinicalExamCommandService : IClinicalExamCommandService
{
    private readonly IClinicalExamRepository _clinicalExamRepository;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<ClinicalExamCommandService> _logger;

    public ClinicalExamCommandService(
        IClinicalExamRepository clinicalExamRepository,
        IEventPublisher eventPublisher,
        ILogger<ClinicalExamCommandService> logger)
    {
        _clinicalExamRepository = clinicalExamRepository;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

    public async Task<ClinicalExam> HandleAsync(CreateClinicalExamCommand command)
    {
        var clinicalExam = new ClinicalExam(
            patientId: command.PatientId,
            examType: command.ExamType,
            scheduledDate: command.ScheduledDate,
            pickupDate: command.PickupDate,
            appointmentId: command.AppointmentId);

        await _clinicalExamRepository.AddAsync(clinicalExam);

        try
        {
            await _eventPublisher.PublishAsync("ExamenCreado",
                new ExamenCreadoEvent
                {
                    PatientId = clinicalExam.PatientId,
                    ExamId = clinicalExam.Id,
                    ExamType = clinicalExam.ExamType,
                    PickupDate = clinicalExam.PickupDate
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to publish ExamenCreado event for exam {ExamId}",
                clinicalExam.Id);
        }

        return clinicalExam;
    }

    public async Task<ClinicalExam> HandleAsync(MarkClinicalExamPickedUpCommand command)
    {
        var clinicalExam = await _clinicalExamRepository.FindByIdAsync(command.ClinicalExamId);
        if (clinicalExam == null)
            throw new ArgumentException($"Clinical exam with id {command.ClinicalExamId} not found");

        clinicalExam.MarkAsCollected();

        await _clinicalExamRepository.UpdateAsync(clinicalExam);
        return clinicalExam;
    }
}