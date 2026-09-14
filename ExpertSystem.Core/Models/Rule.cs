namespace ExpertSystem.Core.Models;

/// <summary>
/// Объект правила.
/// <para>
/// Представляет собой одно условие формата
/// ЕСЛИ Объект1=Значение1 ТО Объект2=Значение2.
/// </para>
/// </summary>
public class Rule
{
    /// <summary>
    /// Условие.
    /// </summary>
    public required Match Condition { get; set; }

    /// <summary>
    /// Следствие.
    /// </summary>
    public required Match Consequence { get; set; }
}
