using ExpertSystem.Core.Models;

namespace ExpertSystem.Core;

/// <summary>
/// Класс определяет методы логической обработки правил,
/// реализующие прямую цепочку рассуждений.
/// </summary>
public static partial class Expert
{
    /// <summary>
    /// Основной метод, который реализует прямую цепочку рассуждений.
    /// </summary>
    /// <param name="ruleSets">Набор правил.</param>
    /// <param name="startState">Стартовое состояние системы.</param>
    /// <returns>Итоговое состояние системы; <c>null</c> если данных недостаточно.</returns>
    public static Match? Think(List<RuleSet> ruleSets, List<Match> startState)
    {
        List<Match> states = [.. startState];

        foreach (RuleSet ruleSet in ruleSets)
        {
            bool isMathing = IsStateMatchesRuleSet(states, ruleSet);

            if (isMathing)
                states.Add(ruleSet.Consequence);
        }

        if (startState.Count == states.Count)
            return null;

        return states.Last();
    }
}
