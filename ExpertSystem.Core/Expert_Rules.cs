using ExpertSystem.Core.Models;

namespace ExpertSystem.Core;

/// <summary>
/// Класс определяет методы логической обработки правил,
/// реализующие прямую цепочку рассуждений.
/// </summary>
public static partial class Expert
{
    /// <summary>
    /// Проверяет, соответствует ли текущее состояние правилу.
    /// </summary>
    /// <param name="state">Текущее состояние.</param>
    /// <param name="rule">Правило.</param>
    /// <returns>Истина/Ложь в зависимости от соответствия.</returns>
    public static bool IsStateMathesRule(Match state, Rule rule)
        => state == rule.Condition;

    /// <summary>
    /// Проверяет, соответствует ли текущее состояние
    /// набору правил.
    /// </summary>
    /// <param name="state">Текущее состояние.</param>
    /// <param name="ruleSet">Набор правил.</param>
    /// <returns>Истина/Ложь в зависимости от соответствия.</returns>
    public static bool IsStateMathesRuleSet(Match state, RuleSet ruleSet)
        => ruleSet.Conditions.Contains(state);

    /// <summary>
    /// Проверяет, соответствует ли текущее состояние
    /// набору правил.
    /// </summary>
    /// <param name="states">Текущее состояние.</param>
    /// <param name="ruleSet">Набор правил.</param>
    /// <returns>Истина/Ложь в зависимости от соответствия.</returns>
    public static bool IsStateMathesRuleSet(List<Match> states, RuleSet ruleSet)
    {
        foreach (Match state in states)
        {
            if (!IsStateMathesRuleSet(state, ruleSet))
            {
                return false;
            }
        }

        return true;
    }
}
