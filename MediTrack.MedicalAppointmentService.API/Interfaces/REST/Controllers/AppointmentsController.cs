using MediTrack.MedicalAppointmentService.API.Domain.Model;
using MediTrack.MedicalAppointmentService.API.Domain.Model.Commands;
using MediTrack.MedicalAppointmentService.API.Domain.Model.Queries;
using MediTrack.MedicalAppointmentService.API.Interfaces.REST.Resources;
using MediTrack.MedicalAppointmentService.API.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace MediTrack.MedicalAppointmentService.API.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/appointments")]
public class AppointmentsController : ControllerBase
{
    private readonly IMedicalAppointmentCommandService _appointmentCommandService;
    private readonly IMedicalAppointmentQueryService _appointmentQueryService;
    private readonly AppointmentCommandFromResourceAssembler _commandAssembler;
    private readonly MedicalAppointmentResourceFromEntityAssembler _resourceAssembler;

    public AppointmentsController(
        IMedicalAppointmentCommandService appointmentCommandService,
        IMedicalAppointmentQueryService appointmentQueryService,
        AppointmentCommandFromResourceAssembler commandAssembler,
        MedicalAppointmentResourceFromEntityAssembler resourceAssembler)
    {
        _appointmentCommandService = appointmentCommandService;
        _appointmentQueryService = appointmentQueryService;
        _commandAssembler = commandAssembler;
        _resourceAssembler = resourceAssembler;
    }

    [HttpPost]
    public async Task<ActionResult<MedicalAppointmentResource>> ScheduleAppointment(
        [FromBody] ScheduleAppointmentResource resource)
    {
        try
        {
            var command = _commandAssembler.ToCommand(resource);
            var appointment = await _appointmentCommandService.HandleAsync(command);
            var response = _resourceAssembler.ToResource(appointment);

            return CreatedAtAction(
                nameof(GetAppointmentById),
                new { id = appointment.Id },
                response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MedicalAppointmentResource>> GetAppointmentById(int id)
    {
        try
        {
            var appointment = await _appointmentQueryService.HandleAsync(new GetAppointmentByIdQuery(id));
            if (appointment == null)
                return NotFound(new { message = $"Appointment with id {id} not found" });

            return Ok(_resourceAssembler.ToResource(appointment));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<MedicalAppointmentResource>>> GetAppointmentsByPatientId(
        [FromQuery] int patientId)
    {
        try
        {
            var appointments = await _appointmentQueryService.HandleAsync(
                new GetAppointmentsByPatientIdQuery(patientId));

            if (!appointments.Any())
                return Ok(new List<MedicalAppointmentResource>());

            return Ok(_resourceAssembler.ToResources(appointments));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpGet("patient/{patientId:int}")]
    public async Task<ActionResult<ICollection<MedicalAppointmentResource>>> GetAppointmentsByPatientIdPath(
        int patientId)
    {
        try
        {
            var appointments = await _appointmentQueryService.HandleAsync(
                new GetAppointmentsByPatientIdQuery(patientId));

            return Ok(_resourceAssembler.ToResources(appointments));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    

    [HttpPut("{id}")]
    public async Task<ActionResult<MedicalAppointmentResource>> UpdateAppointment(
        int id,
        [FromBody] UpdateAppointmentResource resource)
    {
        try
        {
            var command = _commandAssembler.ToCommand(id, resource);
            var appointment = await _appointmentCommandService.HandleAsync(command);
            return Ok(_resourceAssembler.ToResource(appointment));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("{id}/cancel")]
    public async Task<ActionResult<MedicalAppointmentResource>> CancelAppointment(int id)
    {
        try
        {
            var appointment = await _appointmentCommandService.HandleAsync(new CancelAppointmentCommand(id));
            return Ok(_resourceAssembler.ToResource(appointment));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/attendance")]
    public async Task<ActionResult<MedicalAppointmentResource>> RegisterAttendance(
        int id,
        [FromBody] RegisterAppointmentAttendanceResource resource)
    {
        try
        {
            var command = _commandAssembler.ToCommand(id, resource);
            var appointment = await _appointmentCommandService.HandleAsync(command);
            return Ok(_resourceAssembler.ToResource(appointment));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    
    
    
}
