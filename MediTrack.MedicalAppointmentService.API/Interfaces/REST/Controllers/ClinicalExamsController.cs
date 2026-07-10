using MediTrack.MedicalAppointmentService.API.Domain.Model;
using MediTrack.MedicalAppointmentService.API.Domain.Model.Commands;
using MediTrack.MedicalAppointmentService.API.Domain.Model.Queries;
using MediTrack.MedicalAppointmentService.API.Interfaces.REST.Resources;
using MediTrack.MedicalAppointmentService.API.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MediTrack.MedicalAppointmentService.API.Interfaces.REST.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/clinical-exams")]
public class ClinicalExamsController : ControllerBase
{
    private readonly IClinicalExamCommandService _clinicalExamCommandService;
    private readonly IClinicalExamQueryService _clinicalExamQueryService;
    private readonly ClinicalExamCommandFromResourceAssembler _commandAssembler;
    private readonly ClinicalExamResourceFromEntityAssembler _resourceAssembler;

    public ClinicalExamsController(
        IClinicalExamCommandService clinicalExamCommandService,
        IClinicalExamQueryService clinicalExamQueryService,
        ClinicalExamCommandFromResourceAssembler commandAssembler,
        ClinicalExamResourceFromEntityAssembler resourceAssembler)
    {
        _clinicalExamCommandService = clinicalExamCommandService;
        _clinicalExamQueryService = clinicalExamQueryService;
        _commandAssembler = commandAssembler;
        _resourceAssembler = resourceAssembler;
    }

    [HttpPost]
    public async Task<ActionResult<ClinicalExamResource>> CreateClinicalExam(
        [FromBody] CreateClinicalExamResource resource)
    {
        try
        {
            var command = _commandAssembler.ToCommand(resource);
            var clinicalExam = await _clinicalExamCommandService.HandleAsync(command);
            return Created(string.Empty, _resourceAssembler.ToResource(clinicalExam));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("pending")]
    public async Task<ActionResult<ICollection<ClinicalExamResource>>> GetPendingClinicalExams(
        [FromQuery] int patientId)
    {
        try
        {
            var exams = await _clinicalExamQueryService.HandleAsync(
                new GetPendingClinicalExamsByPatientIdQuery(patientId));

            if (!exams.Any())
                return NotFound(new { message = $"No pending clinical exams found for patient {patientId}" });

            return Ok(_resourceAssembler.ToResources(exams));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("{id}/picked-up")]
    public async Task<ActionResult<ClinicalExamResource>> MarkAsPickedUp(int id)
    {
        try
        {
            var clinicalExam = await _clinicalExamCommandService.HandleAsync(
                new MarkClinicalExamPickedUpCommand(id));

            return Ok(_resourceAssembler.ToResource(clinicalExam));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
