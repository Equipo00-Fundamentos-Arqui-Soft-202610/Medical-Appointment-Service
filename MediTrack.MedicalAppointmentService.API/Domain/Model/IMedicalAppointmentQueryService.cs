using MediTrack.MedicalAppointmentService.API.Domain.Model.Aggregates;
using MediTrack.MedicalAppointmentService.API.Domain.Model.Queries;

namespace MediTrack.MedicalAppointmentService.API.Domain.Model;

public interface IMedicalAppointmentQueryService
{
    Task<MedicalAppointment?> HandleAsync(GetAppointmentByIdQuery query);
    Task<ICollection<MedicalAppointment>> HandleAsync(GetAppointmentsByPatientIdQuery query);
}
