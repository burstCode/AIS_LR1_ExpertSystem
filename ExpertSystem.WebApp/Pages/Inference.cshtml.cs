using ExpertSystem.Core;
using ExpertSystem.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExpertSystem.WebApp.Pages;

public class InferenceModel : PageModel
{
    public List<RuleSet> RuleSets { get; private set; } = [];

    /// <summary>
    /// Стартовая ситуация: начальное состояние рабочей базы данных.
    /// </summary>
    [BindProperty]
    public List<MatchInput> Facts { get; set; } = [];

    public InferenceResult? Result { get; private set; }

    /// <summary>
    /// Объекты, значения которых не хватает для дальнейшего вывода.
    /// </summary>
    public List<string> MissingObjects { get; private set; } = [];

    public string? ErrorMessage { get; private set; }

    public void OnGet()
    {
        RuleSets = Parser.LoadRuleSets();
    }

    /// <summary>
    /// Запуск вывода по стартовой ситуации.
    /// </summary>
    public void OnPostRun()
    {
        RuleSets = Parser.LoadRuleSets();

        List<MatchInput> facts = Clean(Facts);
        Facts = facts.Count > 0 ? facts : [new()];

        if (facts.Any(f => !f.IsFilled()))
        {
            ErrorMessage = "У каждого факта должны быть заполнены и объект, и значение.";
            return;
        }

        Result = Expert.Infer(RuleSets, [.. facts.Select(f => new Match { Object = f.Object, Value = f.Value })]);
        MissingObjects = Expert.FindMissingObjects(RuleSets, Result.Facts);
    }

    /// <summary>
    /// Обрезает пробелы и отбрасывает полностью пустые строки.
    /// </summary>
    private static List<MatchInput> Clean(List<MatchInput>? inputs)
        => [.. (inputs ?? [])
            .Where(i => !string.IsNullOrWhiteSpace(i.Object) || !string.IsNullOrWhiteSpace(i.Value))
            .Select(i => new MatchInput { Object = i.Object?.Trim() ?? "", Value = i.Value?.Trim() ?? "" })];

    /// <summary>
    /// Подпись источника факта в рабочей базе данных.
    /// </summary>
    public string SourceOf(Match fact)
    {
        InferenceStep? step = Result?.Steps.FirstOrDefault(s => ReferenceEquals(s.Derived, fact));
        return step is null ? "стартовые данные" : $"правило #{step.RuleIndex + 1}";
    }
}
