namespace MediTrack.MedicalAppointmentService.API.Domain.Model.Commands;

public record RegisterAppointmentAttendanceCommand(int AppointmentId, string Status);
