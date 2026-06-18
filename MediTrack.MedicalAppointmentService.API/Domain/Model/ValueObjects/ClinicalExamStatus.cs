namespace MediTrack.MedicalAppointmentService.API.Domain.Model.ValueObjects;

public class ClinicalExamStatus
{
    public static readonly ClinicalExamStatus Pending = new("pending");
    public static readonly ClinicalExamStatus Ready = new("ready");
    public static readonly ClinicalExamStatus Collected = new("collected");

    private static readonly Dictionary<string, ClinicalExamStatus> ValidStatuses = new()
    {
        { "pending", Pending },
        { "ready", Ready },
        { "collected", Collected }
    };

    public string Value { get; }

    private ClinicalExamStatus(string value)
    {
        Value = value;
    }

    public static ClinicalExamStatus From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("ClinicalExamStatus cannot be empty or null", nameof(value));

        var normalizedValue = value.Trim().ToLowerInvariant();

        if (!ValidStatuses.TryGetValue(normalizedValue, out var status))
            throw new ArgumentException(
                "ClinicalExamStatus must be pending, ready, or collected",
                nameof(value));

        return status;
    }

    public bool IsPending => Value == Pending.Value;
    public bool IsReady => Value == Ready.Value;
    public bool IsCollected => Value == Collected.Value;

    public override bool Equals(object? obj)
    {
        return obj is ClinicalExamStatus status && status.Value == Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value;
    }

    public static implicit operator string(ClinicalExamStatus status) => status.Value;
    public static implicit operator ClinicalExamStatus(string value) => From(value);
}