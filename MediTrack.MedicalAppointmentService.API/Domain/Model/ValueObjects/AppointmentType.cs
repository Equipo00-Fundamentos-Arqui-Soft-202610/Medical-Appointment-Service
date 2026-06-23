namespace MediTrack.MedicalAppointmentService.API.Domain.Model.ValueObjects;

public class AppointmentType
{
    public string Value { get; }

    private AppointmentType(string value)
    {
        Value = value;
    }

    public static AppointmentType From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("AppointmentType cannot be empty or null", nameof(value));

        var normalizedValue = value.Trim();

        if (normalizedValue.Length > 80)
            throw new ArgumentException("AppointmentType cannot exceed 80 characters", nameof(value));

        return new AppointmentType(normalizedValue);
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