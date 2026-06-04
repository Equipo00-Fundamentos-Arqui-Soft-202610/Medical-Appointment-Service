using MediTrack.MedicalAppointmentService.API.Domain.Model.ValueObjects;

namespace MediTrack.MedicalAppointmentService.API.Domain.Model.Aggregates;

public class AppointmentRequirement
{
    public int Id { get; set; }
    public int MedicalAppointmentId { get; set; }
    public RequirementText Description { get; set; } = null!;
    public MedicalAppointment MedicalAppointment { get; set; } = null!;

    public AppointmentRequirement() { }

    public AppointmentRequirement(int medicalAppointmentId, string description)
    {
        MedicalAppointmentId = medicalAppointmentId;
        Description = new RequirementText(description);
    }
}
