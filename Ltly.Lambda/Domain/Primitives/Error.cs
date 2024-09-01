namespace Ltly.Lambda.Domain.Primitives;

public record Error(string Message)
{
    public static Error None = new(string.Empty);
    public static Error NullValue = new("Null input");
    public override string ToString() => $"{Message}";
}