namespace ExpertSystem.Core.Models;

/// <summary>
/// Объект соответствия.
/// <para>
/// Представляет собой одно <i>соответствие</i>.
/// </para>
/// </summary>
public class Match
{
    /// <summary>
    /// Объект.
    /// </summary>
    public required string Object { get; set; }

    /// <summary>
    /// Значение.
    /// </summary>
    public required string Value { get; set; }

    // Перегрузка Equals и определение оператора ==
    // для упрощенного проведения соответствия
    // Экспертной Системой.
    #region Overrides

    public static bool operator ==(Match a, Match b)
    {
        if (a.Object == b.Object &&
            a.Value == b.Value)
            return true;

        return false;
    }

    public static bool operator !=(Match a, Match b)
    {
        return !(a == b);
    }

    public override bool Equals(object? obj)
    {
        return obj is Match other && this == other;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Object, Value);
    }

    #endregion
}
