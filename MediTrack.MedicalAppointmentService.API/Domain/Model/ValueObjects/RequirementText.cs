namespace MediTrack.MedicalAppointmentService.API.Domain.Model.ValueObjects;

public class RequirementText
{
    public string Value { get; }

    public RequirementText(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("RequirementText cannot be empty or null", nameof(value));

        if (value.Length > 500)
            throw new ArgumentException("RequirementText cannot exceed 500 characters", nameof(value));

        Value = value.Trim();
    }

    public override bool Equals(object? obj)
    {
        return obj is RequirementText text && text.Value == Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value;
    }

    public static implicit operator string(RequirementText text) => text.Value;
    public static implicit operator RequirementText(string value) => new(value);
}
