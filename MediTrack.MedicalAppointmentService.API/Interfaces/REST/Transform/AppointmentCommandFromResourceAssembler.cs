using MediTrack.MedicalAppointmentService.API.Domain.Model.Commands;
using MediTrack.MedicalAppointmentService.API.Interfaces.REST.Resources;

namespace MediTrack.MedicalAppointmentService.API.Interfaces.REST.Transform;

public class AppointmentCommandFromResourceAssembler
{
    public ScheduleAppointmentCommand ToCommand(ScheduleAppointmentResource resource)
    {
        return new ScheduleAppointmentCommand(
            PatientId: resource.PatientId,
            Type: resource.Type,
            ScheduledAt: resource.ScheduledAt,
            Location: resource.Location,
            Notes: resource.Notes,
            Requirements: resource.Requirements);
    }

    public UpdateAppointmentCommand ToCommand(int appointmentId, UpdateAppointmentResource resource)
    {
        return new UpdateAppointmentCommand(
            AppointmentId: appointmentId,
            Type: resource.Type,
            ScheduledAt: resource.ScheduledAt,
            Location: resource.Location,
            Notes: resource.Notes,
            Requirements: resource.Requirements);
    }

    public RegisterAppointmentAttendanceCommand ToCommand(
        int appointmentId,
        RegisterAppointmentAttendanceResource resource)
    {
        return new RegisterAppointmentAttendanceCommand(appointmentId, resource.Status);
    }
}
