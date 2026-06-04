using MediTrack.MedicalAppointmentService.API.Domain.Model.ValueObjects;

namespace MediTrack.MedicalAppointmentService.API.Domain.Model.Aggregates;

public class MedicalAppointment
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public AppointmentType Type { get; set; } = null!;
    public DateTime ScheduledAt { get; set; }
    public string? Location { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<AppointmentRequirement> Requirements { get; set; } = new();

    public MedicalAppointment() { }

    public MedicalAppointment(int patientId, string type, DateTime scheduledAt, string? location = null)
    {
        if (patientId <= 0)
            throw new ArgumentException("PatientId must be greater than 0", nameof(patientId));

        ValidateFutureDate(scheduledAt);

        PatientId = patientId;
        Type = AppointmentType.From(type);
        ScheduledAt = scheduledAt;
        Location = string.IsNullOrWhiteSpace(location) ? null : location.Trim();
        Status = AppointmentStatus.Scheduled;
        CreatedAt = DateTime.UtcNow;
    }

    public bool IsPast => ScheduledAt <= DateTime.UtcNow;
    public bool CanBeModified => !IsPast && Status.IsScheduled;

    public void Reschedule(string type, DateTime scheduledAt, string? location)
    {
        EnsureCanBeModified();
        ValidateFutureDate(scheduledAt);

        Type = AppointmentType.From(type);
        ScheduledAt = scheduledAt;
        Location = string.IsNullOrWhiteSpace(location) ? null : location.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        EnsureCanBeModified();
        Status = AppointmentStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RegisterAttendance(string attendanceStatus)
    {
        if (ScheduledAt.Date > DateTime.UtcNow.Date)
            throw new ArgumentException("Cannot register attendance for a future appointment");

        var status = AppointmentStatus.From(attendanceStatus);
        if (status.Value != AppointmentStatus.Attended.Value && status.Value != AppointmentStatus.Missed.Value)
            throw new ArgumentException("Attendance status must be attended or missed", nameof(attendanceStatus));

        Status = status;
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

    private static void ValidateFutureDate(DateTime scheduledAt)
    {
        if (scheduledAt <= DateTime.UtcNow)
            throw new ArgumentException("Appointment date must be later than the current date", nameof(scheduledAt));
    }
}
