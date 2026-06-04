namespace MediTrack.MedicalAppointmentService.API.Domain.Model.ValueObjects;

public class AppointmentStatus
{
    public static readonly AppointmentStatus Scheduled = new("scheduled");
    public static readonly AppointmentStatus Cancelled = new("cancelled");
    public static readonly AppointmentStatus Attended = new("attended");
    public static readonly AppointmentStatus Missed = new("missed");

    private static readonly Dictionary<string, AppointmentStatus> ValidStatuses = new()
    {
        { "scheduled", Scheduled },
        { "cancelled", Cancelled },
        { "attended", Attended },
        { "missed", Missed }
    };

    public string Value { get; }

    private AppointmentStatus(string value)
    {
        Value = value;
    }

    public static AppointmentStatus From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("AppointmentStatus cannot be empty or null", nameof(value));

        var normalizedValue = value.Trim().ToLowerInvariant();
        if (!ValidStatuses.TryGetValue(normalizedValue, out var status))
            throw new ArgumentException("AppointmentStatus must be scheduled, cancelled, attended, or missed", nameof(value));

        return status;
    }

    public bool IsScheduled => Value == Scheduled.Value;
    public bool IsCancelled => Value == Cancelled.Value;

    public override bool Equals(object? obj)
    {
        return obj is AppointmentStatus status && status.Value == Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value;
    }

    public static implicit operator string(AppointmentStatus status) => status.Value;
    public static implicit operator AppointmentStatus(string value) => From(value);
}
