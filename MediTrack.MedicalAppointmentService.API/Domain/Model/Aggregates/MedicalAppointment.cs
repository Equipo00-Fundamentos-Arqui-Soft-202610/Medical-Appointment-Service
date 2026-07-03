using MediTrack.MedicalAppointmentService.API.Domain.Model.ValueObjects;

namespace MediTrack.MedicalAppointmentService.API.Domain.Model.Aggregates;

public class MedicalAppointment
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public AppointmentType Type { get; set; } = null!;
    public DateTime ScheduledAt { get; set; }
    public string? Location { get; set; }
    public string? Notes { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<AppointmentRequirement> Requirements { get; set; } = new();

    public MedicalAppointment() { }

    public MedicalAppointment(int patientId, string type, DateTime scheduledAt, string? location = null, string? notes = null)
    {
        if (patientId <= 0)
            throw new ArgumentException("PatientId must be greater than 0", nameof(patientId));

        if (scheduledAt <= DateTime.UtcNow)
            throw new ArgumentException("La fecha de la cita debe ser posterior a ahora");

        PatientId = patientId;
        Type = AppointmentType.From(type);
        ScheduledAt = scheduledAt;
        Location = string.IsNullOrWhiteSpace(location) ? null : location.Trim();
        Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();
        Status = AppointmentStatus.Scheduled;
        CreatedAt = DateTime.UtcNow;
    }

    public bool IsPast => ScheduledAt <= DateTime.UtcNow;
    public bool CanBeModified => !IsPast && Status.IsScheduled;

    public void Reschedule(string type, DateTime scheduledAt, string? location,string? notes)
    {
        EnsureCanBeModified();
        if (scheduledAt <= DateTime.UtcNow)
            throw new ArgumentException("La fecha de la cita debe ser posterior a ahora");

        Type = AppointmentType.From(type);
        ScheduledAt = scheduledAt;
        Location = string.IsNullOrWhiteSpace(location) ? null : location.Trim();
        Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        EnsureCanBeModified();
        Status = AppointmentStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Complete()
    {
        if (Status.IsCancelled)
            throw new ArgumentException("Cannot complete a cancelled appointment");

        Status = AppointmentStatus.Completed;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ReplaceRequirements(IEnumerable<string>? requirements)
    {
        Requirements.Clear();
        if (requirements == null)
            return;

        Requirements = requirements
            .Where(requirement => !string.IsNullOrWhiteSpace(requirement))
            .Select(requirement => new AppointmentRequirement(Id, requirement))
            .ToList();
    }

    private void EnsureCanBeModified()
    {
        if (!CanBeModified)
            throw new ArgumentException("Cannot modify past or non-scheduled appointments");
    }

}
