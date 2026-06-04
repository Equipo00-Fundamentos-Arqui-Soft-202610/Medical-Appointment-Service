using MediTrack.MedicalAppointmentService.API.Domain.Model;
using MediTrack.MedicalAppointmentService.API.Domain.Model.Aggregates;
using MediTrack.MedicalAppointmentService.API.Domain.Model.Commands;

namespace MediTrack.MedicalAppointmentService.API.Application.Internal.CommandServices;

public class MedicalAppointmentCommandService : IMedicalAppointmentCommandService
{
    private readonly IMedicalAppointmentRepository _appointmentRepository;

    public MedicalAppointmentCommandService(IMedicalAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<MedicalAppointment> HandleAsync(ScheduleAppointmentCommand command)
    {
        var appointment = new MedicalAppointment(
            patientId: command.PatientId,
            type: command.Type,
            scheduledAt: command.ScheduledAt,
            location: command.Location);

        appointment.ReplaceRequirements(command.Requirements);

        await _appointmentRepository.AddAsync(appointment);
        return appointment;
    }

    public async Task<MedicalAppointment> HandleAsync(UpdateAppointmentCommand command)
    {
        var appointment = await _appointmentRepository.FindByIdAsync(command.AppointmentId);
        if (appointment == null)
            throw new ArgumentException($"Appointment with id {command.AppointmentId} not found");

        appointment.Reschedule(command.Type, command.ScheduledAt, command.Location);
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

    public async Task<MedicalAppointment> HandleAsync(RegisterAppointmentAttendanceCommand command)
    {
        var appointment = await _appointmentRepository.FindByIdAsync(command.AppointmentId);
        if (appointment == null)
            throw new ArgumentException($"Appointment with id {command.AppointmentId} not found");

        appointment.RegisterAttendance(command.Status);

        await _appointmentRepository.UpdateAsync(appointment);
        return appointment;
    }
}
