namespace MediTrack.MedicalAppointmentService.API.Domain.Model.ValueObjects;

public class AppointmentType
{
    public static readonly AppointmentType General = new("general");
    public static readonly AppointmentType Control = new("control");
    public static readonly AppointmentType Exam = new("exam");
    public static readonly AppointmentType Specialist = new("specialist");

    private static readonly Dictionary<string, AppointmentType> ValidTypes = new()
    {
        { "general", General },
        { "control", Control },
        { "exam", Exam },
        { "specialist", Specialist }
    };

    public string Value { get; }

    private AppointmentType(string value)
    {
        Value = value;
    }

    public static AppointmentType From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("AppointmentType cannot be empty or null", nameof(value));

        var normalizedValue = value.Trim().ToLowerInvariant();
        if (!ValidTypes.TryGetValue(normalizedValue, out var appointmentType))
            throw new ArgumentException("AppointmentType must be general, control, exam, or specialist", nameof(value));

        return appointmentType;
    }

    public override bool Equals(object? obj)
    {
        return obj is AppointmentType type && type.Value == Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value;
    }

    public static implicit operator string(AppointmentType type) => type.Value;
    public static implicit operator AppointmentType(string value) => From(value);
}
