using MediTrack.MedicalAppointmentService.API.Domain.Model.Commands;
using MediTrack.MedicalAppointmentService.API.Interfaces.REST.Resources;

namespace MediTrack.MedicalAppointmentService.API.Interfaces.REST.Transform;

public class ClinicalExamCommandFromResourceAssembler
{
    public CreateClinicalExamCommand ToCommand(CreateClinicalExamResource resource)
    {
        return new CreateClinicalExamCommand(
            PatientId: resource.PatientId,
            ExamType: resource.ExamType,
            PickupDate: resource.PickupDate,
            LaboratoryName: resource.LaboratoryName);
    }
}
