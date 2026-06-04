using MediTrack.MedicalAppointmentService.API.Domain.Model.Aggregates;
using MediTrack.MedicalAppointmentService.API.Domain.Model.Commands;

namespace MediTrack.MedicalAppointmentService.API.Domain.Model;

public interface IMedicalAppointmentCommandService
{
    Task<MedicalAppointment> HandleAsync(ScheduleAppointmentCommand command);
    Task<MedicalAppointment> HandleAsync(UpdateAppointmentCommand command);
    Task<MedicalAppointment> HandleAsync(CancelAppointmentCommand command);
    Task<MedicalAppointment> HandleAsync(RegisterAppointmentAttendanceCommand command);
}
