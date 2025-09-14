namespace Ves.Domain.ValueObjects;

/// <summary>
/// Represents a quantity value object used to avoid primitive obsession.
/// </summary>
public record Quantity(int Value)
{
    public static Quantity operator +(Quantity a, Quantity b) => new(a.Value + b.Value);
    public static Quantity operator -(Quantity a, Quantity b) => new(a.Value - b.Value);
    public override string ToString() => Value.ToString();
}
