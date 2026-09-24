namespace ExpertSystem.Core.Models;

/// <summary>
/// Один шаг вывода: сработавшее правило и выведенный им факт.
/// </summary>
public class InferenceStep
{
    /// <summary>
    /// Индекс сработавшего правила в своде.
    /// </summary>
    public required int RuleIndex { get; init; }

    /// <summary>
    /// Сработавшее правило.
    /// </summary>
    public required RuleSet RuleSet { get; init; }

    /// <summary>
    /// Новый факт, добавленный в рабочую базу данных.
    /// </summary>
    public required Match Derived { get; init; }
}

/// <summary>
/// Результат работы прямой цепочки рассуждений.
/// </summary>
public class InferenceResult
{
    /// <summary>
    /// Итоговое состояние рабочей базы данных:
    /// сначала стартовые факты, затем выведенные по порядку.
    /// </summary>
    public required List<Match> Facts { get; init; }

    /// <summary>
    /// Шаги вывода в порядке срабатывания правил.
    /// </summary>
    public required List<InferenceStep> Steps { get; init; }

    /// <summary>
    /// Вывод системы — последний выведенный факт;
    /// <c>null</c>, если ни одно правило не сработало.
    /// </summary>
    public Match? Conclusion => Steps.Count > 0 ? Steps[^1].Derived : null;
}
