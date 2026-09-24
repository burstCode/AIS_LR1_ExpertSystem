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
        => Infer(ruleSets, startState).Conclusion;

    /// <summary>
    /// Прямая цепочка рассуждений с полным протоколом вывода.
    /// <para>
    /// Правила просматриваются повторно, пока хотя бы одно из них
    /// добавляет новый факт, поэтому порядок правил в своде не важен.
    /// Каждое правило срабатывает не более одного раза.
    /// </para>
    /// </summary>
    /// <param name="ruleSets">Набор правил.</param>
    /// <param name="startState">Стартовое состояние системы.</param>
    public static InferenceResult Infer(List<RuleSet> ruleSets, List<Match> startState)
    {
        List<Match> facts = [];
        foreach (Match fact in startState)
        {
            if (!facts.Contains(fact))
                facts.Add(fact);
        }

        List<InferenceStep> steps = [];
        bool[] fired = new bool[ruleSets.Count];

        bool progress = true;
        while (progress)
        {
            progress = false;

            for (int i = 0; i < ruleSets.Count; i++)
            {
                if (fired[i] || !IsStateMatchesRuleSet(facts, ruleSets[i]))
                    continue;

                fired[i] = true;

                if (facts.Contains(ruleSets[i].Consequence))
                    continue;

                facts.Add(ruleSets[i].Consequence);
                steps.Add(new InferenceStep
                {
                    RuleIndex = i,
                    RuleSet = ruleSets[i],
                    Derived = ruleSets[i].Consequence
                });
                progress = true;
            }
        }

        return new InferenceResult { Facts = facts, Steps = steps };
    }

    /// <summary>
    /// Определяет объекты, о которых стоит спросить пользователя,
    /// когда ни одно правило больше не срабатывает.
    /// <para>
    /// Это объекты из условий несработавших правил, значение которых
    /// ещё неизвестно. Объекты, которые сами выводятся какими-либо
    /// правилами, спрашиваются, только если других вопросов нет.
    /// </para>
    /// </summary>
    /// <param name="ruleSets">Набор правил.</param>
    /// <param name="facts">Текущее состояние рабочей базы данных.</param>
    /// <returns>Названия объектов без повторов.</returns>
    public static List<string> FindMissingObjects(List<RuleSet> ruleSets, List<Match> facts)
    {
        static string Key(string s) => s.Trim().ToLowerInvariant();

        HashSet<string> known = [.. facts.Select(f => Key(f.Object))];
        HashSet<string> derivable = [.. ruleSets.Select(r => Key(r.Consequence.Object))];

        Dictionary<string, string> missing = [];
        foreach (RuleSet rs in ruleSets)
        {
            if (IsStateMatchesRuleSet(facts, rs))
                continue;

            foreach (Match cond in rs.Conditions)
            {
                string key = Key(cond.Object);
                if (!known.Contains(key))
                    missing.TryAdd(key, cond.Object.Trim());
            }
        }

        List<string> primary = [.. missing.Where(m => !derivable.Contains(m.Key)).Select(m => m.Value)];
        return primary.Count > 0 ? primary : [.. missing.Values];
    }
}
