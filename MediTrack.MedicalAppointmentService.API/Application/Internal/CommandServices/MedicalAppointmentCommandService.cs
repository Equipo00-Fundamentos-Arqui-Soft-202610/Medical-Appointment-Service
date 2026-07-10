using MediTrack.MedicalAppointmentService.API.Application.OutboundEvents;
using MediTrack.MedicalAppointmentService.API.Domain.Model;
using MediTrack.MedicalAppointmentService.API.Domain.Model.Aggregates;
using MediTrack.MedicalAppointmentService.API.Domain.Model.Commands;
using MediTrack.MedicalAppointmentService.API.Infrastructure.Messaging;

namespace MediTrack.MedicalAppointmentService.API.Application.Internal.CommandServices;

public class MedicalAppointmentCommandService : IMedicalAppointmentCommandService
{
    private readonly IMedicalAppointmentRepository _appointmentRepository;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<MedicalAppointmentCommandService> _logger;

    public MedicalAppointmentCommandService(
        IMedicalAppointmentRepository appointmentRepository,
        IEventPublisher eventPublisher,
        ILogger<MedicalAppointmentCommandService> logger)
    {
        _appointmentRepository = appointmentRepository;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

    public async Task<MedicalAppointment> HandleAsync(ScheduleAppointmentCommand command)
    {
        var appointment = new MedicalAppointment(
            patientId: command.PatientId,
            type: command.Type,
            scheduledAt: command.ScheduledAt,
            location: command.Location, notes: command.Notes);

        appointment.ReplaceRequirements(command.Requirements);

        await _appointmentRepository.AddAsync(appointment);

        try
        {
            await _eventPublisher.PublishAsync("CitaAgendada",
                new AppointmentScheduledEvent
                {
                    PatientId = appointment.PatientId,
                    AppointmentId = appointment.Id,
                    AppointmentType = appointment.Type.Value,
                    Location = appointment.Location,
                    AppointmentDateUtc = appointment.ScheduledAt
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to publish CitaAgendada event for appointment {AppointmentId}",
                appointment.Id);
        }

        return appointment;
    }

    public async Task<MedicalAppointment> HandleAsync(UpdateAppointmentCommand command)
    {
        var appointment = await _appointmentRepository.FindByIdAsync(command.AppointmentId);
        if (appointment == null)
            throw new ArgumentException($"Appointment with id {command.AppointmentId} not found");

        appointment.Reschedule(command.Type, command.ScheduledAt, command.Location, command.Notes);
        appointment.ReplaceRequirements(command.Requirements);

        await _appointmentRepository.UpdateAsync(appointment);
        return appointment;
    }

    public async Task<MedicalAppointment> HandleAsync(CancelAppointmentCommand command)
    {
        var appointment = await _appointmentRepository.FindByIdAsync(command.AppointmentId);
        if (appointment == null)
            throw new ArgumentException($"Appointment with id {command.AppointmentId} not found");

        appointment.Cancel();

        await _appointmentRepository.UpdateAsync(appointment);
        return appointment;
    }

    private static readonly HashSet<string> ValidAttendanceStatuses =
        new(StringComparer.OrdinalIgnoreCase) { "Attended", "NoShow" };

    public async Task<MedicalAppointment> HandleAsync(RegisterAppointmentAttendanceCommand command)
    {
        // Sin esto, cualquier string (incluido vacío) se aceptaba y se difundía
        // tal cual en el evento -- Medical-Analysis-Service compara este valor
        // contra "attended" para calcular adherencia, así que un typo o un vacío
        // rompe silenciosamente esa métrica en otro servicio.
        if (string.IsNullOrWhiteSpace(command.Status) || !ValidAttendanceStatuses.Contains(command.Status))
            throw new ArgumentException("Status must be 'Attended' or 'NoShow'", nameof(command.Status));

        var appointment = await _appointmentRepository.FindByIdAsync(command.AppointmentId);
        if (appointment == null)
            throw new ArgumentException($"Appointment with id {command.AppointmentId} not found");

        appointment.Complete();

        await _appointmentRepository.UpdateAsync(appointment);

        try
        {
            await _eventPublisher.PublishAsync("AppointmentAttendanceRegistered",
                new AppointmentAttendanceRegisteredEvent
                {
                    PatientId = appointment.PatientId,
                    AppointmentId = appointment.Id,
                    AttendanceStatus = command.Status,
                    OccurredAt = DateTime.UtcNow
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to publish appointment attendance registered event for appointment {AppointmentId}",
                appointment.Id);
        }

        return appointment;
    }
}
