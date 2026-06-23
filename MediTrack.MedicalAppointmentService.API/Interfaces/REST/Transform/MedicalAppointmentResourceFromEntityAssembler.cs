using MediTrack.MedicalAppointmentService.API.Domain.Model.Aggregates;
using MediTrack.MedicalAppointmentService.API.Interfaces.REST.Resources;

namespace MediTrack.MedicalAppointmentService.API.Interfaces.REST.Transform;

public class MedicalAppointmentResourceFromEntityAssembler
{
    public MedicalAppointmentResource ToResource(MedicalAppointment appointment)
    {
        return new MedicalAppointmentResource(
            Id: appointment.Id,
            PatientId: appointment.PatientId,
            Type: appointment.Type.Value,
            ScheduledAt: appointment.ScheduledAt,
            Location: appointment.Location,
            Notes: appointment.Notes,
            Status: appointment.Status.Value,
            CanBeModified: appointment.CanBeModified,
            CreatedAt: appointment.CreatedAt,
            UpdatedAt: appointment.UpdatedAt,
            Requirements: appointment.Requirements
                .Select(requirement => new AppointmentRequirementResource(
                    Id: requirement.Id,
                    Description: requirement.Description.Value))
                .ToList());
    }

    public ICollection<MedicalAppointmentResource> ToResources(ICollection<MedicalAppointment> appointments)
    {
        return appointments.Select(ToResource).ToList();
    }
}
