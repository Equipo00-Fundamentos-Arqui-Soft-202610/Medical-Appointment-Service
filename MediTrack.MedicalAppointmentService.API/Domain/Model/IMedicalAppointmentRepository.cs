using MediTrack.MedicalAppointmentService.API.Domain.Model.Aggregates;

namespace MediTrack.MedicalAppointmentService.API.Domain.Model;

public interface IMedicalAppointmentRepository
{
    Task<MedicalAppointment?> FindByIdAsync(int appointmentId);
    Task<ICollection<MedicalAppointment>> FindByPatientIdAsync(int patientId);
    Task AddAsync(MedicalAppointment appointment);
    Task UpdateAsync(MedicalAppointment appointment);
    Task<ICollection<MedicalAppointment>> FindAllAsync();
}
