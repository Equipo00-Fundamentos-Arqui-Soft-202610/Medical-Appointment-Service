using MediTrack.MedicalAppointmentService.API.Domain.Model;
using MediTrack.MedicalAppointmentService.API.Domain.Model.Aggregates;
using MediTrack.MedicalAppointmentService.API.Domain.Model.ValueObjects;
using MediTrack.MedicalAppointmentService.API.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace MediTrack.MedicalAppointmentService.API.Infrastructure.Persistence.EFC;

public class ClinicalExamRepository : IClinicalExamRepository
{
    private readonly MedicalAppointmentDbContext _context;

    public ClinicalExamRepository(MedicalAppointmentDbContext context)
    {
        _context = context;
    }

    public async Task<ClinicalExam?> FindByIdAsync(int clinicalExamId)
    {
        return await _context.ClinicalExams
            .FirstOrDefaultAsync(e => e.Id == clinicalExamId);
    }

    public async Task<ICollection<ClinicalExam>> FindPendingByPatientIdAsync(int patientId)
    {
        var pendingStatus = ClinicalExamStatus.PendingPickup.Value;
        return await _context.ClinicalExams
            .Where(e => e.PatientId == patientId && e.Status.Value == pendingStatus)
            .OrderBy(e => e.PickupDate)
            .ToListAsync();
    }

    public async Task AddAsync(ClinicalExam clinicalExam)
    {
        await _context.ClinicalExams.AddAsync(clinicalExam);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ClinicalExam clinicalExam)
    {
        _context.ClinicalExams.Update(clinicalExam);
        await _context.SaveChangesAsync();
    }
}
