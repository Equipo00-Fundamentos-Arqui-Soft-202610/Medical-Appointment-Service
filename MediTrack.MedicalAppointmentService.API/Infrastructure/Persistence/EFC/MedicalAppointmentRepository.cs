using MediTrack.MedicalAppointmentService.API.Domain.Model;
using MediTrack.MedicalAppointmentService.API.Domain.Model.Aggregates;
using MediTrack.MedicalAppointmentService.API.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace MediTrack.MedicalAppointmentService.API.Infrastructure.Persistence.EFC;

public class MedicalAppointmentRepository : IMedicalAppointmentRepository
{
    private readonly MedicalAppointmentDbContext _context;

    public MedicalAppointmentRepository(MedicalAppointmentDbContext context)
    {
        _context = context;
    }

    public async Task<MedicalAppointment?> FindByIdAsync(int appointmentId)
    {
        return await _context.MedicalAppointments
            .Include(a => a.Requirements)
            .FirstOrDefaultAsync(a => a.Id == appointmentId);
    }

    public async Task<ICollection<MedicalAppointment>> FindByPatientIdAsync(int patientId)
    {
        return await _context.MedicalAppointments
            .Where(a => a.PatientId == patientId)
            .Include(a => a.Requirements)
            .OrderBy(a => a.ScheduledAt)
            .ToListAsync();
    }

    public async Task AddAsync(MedicalAppointment appointment)
    {
        await _context.MedicalAppointments.AddAsync(appointment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(MedicalAppointment appointment)
    {
        _context.MedicalAppointments.Update(appointment);
        await _context.SaveChangesAsync();
    }
}
