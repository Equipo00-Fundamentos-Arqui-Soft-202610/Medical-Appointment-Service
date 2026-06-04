using MediTrack.MedicalAppointmentService.API.Domain.Model;
using MediTrack.MedicalAppointmentService.API.Domain.Model.Aggregates;
using MediTrack.MedicalAppointmentService.API.Domain.Model.Queries;

namespace MediTrack.MedicalAppointmentService.API.Application.Internal.QueryServices;

public class MedicalAppointmentQueryService : IMedicalAppointmentQueryService
{
    private readonly IMedicalAppointmentRepository _appointmentRepository;

    public MedicalAppointmentQueryService(IMedicalAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<MedicalAppointment?> HandleAsync(GetAppointmentByIdQuery query)
    {
        if (query.AppointmentId <= 0)
            throw new ArgumentException("AppointmentId must be greater than 0");

        return await _appointmentRepository.FindByIdAsync(query.AppointmentId);
    }

    public async Task<ICollection<MedicalAppointment>> HandleAsync(GetAppointmentsByPatientIdQuery query)
    {
        if (query.PatientId <= 0)
            throw new ArgumentException("PatientId must be greater than 0");

        return await _appointmentRepository.FindByPatientIdAsync(query.PatientId);
    }
}
