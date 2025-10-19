namespace BlazorLens.Shared.Kernel;

/// <summary>
/// Base class for Value Objects in Domain-Driven Design.
/// Value Objects are immutable and compared by their values, not identity.
/// Compliance: OOD-001 (Single Responsibility), CON-001 (Change Localization)
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// Gets the atomic values that define equality for this Value Object.
    /// Must be implemented by derived classes.
    /// </summary>
    /// <returns>Enumerable of equality components</returns>
    protected abstract IEnumerable<object?> GetEqualityComponents();

    /// <summary>
    /// Determines whether two Value Objects are equal based on their values.
    /// Compliance: OOD-005 (Design by Contract)
    /// </summary>
    /// <param name="obj">Object to compare</param>
    /// <returns>True if objects are equal by value</returns>
    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ValueObject)obj;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    /// <summary>
    /// Gets hash code based on equality components.
    /// Ensures consistent hashing for collections.
    /// Compliance: CCP-005 (Defensive Programming)
    /// </summary>
    /// <returns>Hash code</returns>
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }

    /// <summary>
    /// Equality operator overload.
    /// </summary>
    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    /// <summary>
    /// Inequality operator overload.
    /// </summary>
    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Creates a shallow copy of the Value Object.
    /// Useful for creating modified versions in immutable patterns.
    /// </summary>
    /// <returns>Shallow copy</returns>
    protected ValueObject GetCopy()
    {
        return (ValueObject)MemberwiseClone();
    }
}