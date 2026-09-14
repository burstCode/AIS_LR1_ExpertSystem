namespace ExpertSystem.Core.Models;

/// <summary>
/// Объект набора правил, связанных
/// логическим оператором "И".
/// </summary>
public class RuleSet
{
    /// <summary>
    /// Список условий.
    /// </summary>
    public required List<Match> Conditions { get; set; }

    /// <summary>
    /// Следствие.
    /// </summary>
    public required Match Consequence { get; set; }
}
