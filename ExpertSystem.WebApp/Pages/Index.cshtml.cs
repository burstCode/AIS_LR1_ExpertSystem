using ExpertSystem.Core;
using ExpertSystem.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExpertSystem.WebApp.Pages;

public class IndexModel : PageModel
{
    public List<RuleSet> RuleSets { get; private set; } = [];

    /// <summary>
    /// Привязка данных к модальному окну добавления/редактирования.
    /// </summary>
    [BindProperty]
    public RuleSetInput Input { get; set; } = new();

    [TempData]
    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
        RuleSets = Parser.LoadRuleSets();
    }

    /// <summary>
    /// Добавляет правило (<c>Index == null</c>) или изменяет существующее.
    /// </summary>
    public IActionResult OnPostSave()
    {
        Input.Normalize();

        List<RuleSet> ruleSets = Parser.LoadRuleSets();

        if (Input.Index is int index && (index < 0 || index >= ruleSets.Count))
            return Fail("Правило не найдено.");

        if (!Input.IsValid())
            return Fail("Заполните все поля правила: нужно минимум одно условие и следствие.");

        RuleSet ruleSet = Input.ToRuleSet();

        if (Input.Index is int i)
            ruleSets[i] = ruleSet;
        else
            ruleSets.Add(ruleSet);

        Parser.SaveRuleSets(ruleSets);
        return RedirectToPage();
    }

    public IActionResult OnPostDelete(int index)
    {
        List<RuleSet> ruleSets = Parser.LoadRuleSets();

        if (index < 0 || index >= ruleSets.Count)
            return Fail("Правило не найдено.");

        ruleSets.RemoveAt(index);
        Parser.SaveRuleSets(ruleSets);
        return RedirectToPage();
    }

    private IActionResult Fail(string message)
    {
        ErrorMessage = message;
        return RedirectToPage();
    }
}

public class MatchInput
{
    public string Object { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public bool IsFilled()
        => !string.IsNullOrWhiteSpace(Object) &&
           !string.IsNullOrWhiteSpace(Value);
}

public class RuleSetInput
{
    /// <summary>
    /// Индекс редактируемого правила; <c>null</c> - добавление нового.
    /// </summary>
    public int? Index { get; set; }

    public List<MatchInput> Conditions { get; set; } = [];

    public MatchInput Consequence { get; set; } = new();

    /// <summary>
    /// Обрезает пробелы и убирает полностью пустые условия.
    /// </summary>
    public void Normalize()
    {
        Conditions = [.. Conditions
            .Where(c => !string.IsNullOrWhiteSpace(c.Object) || !string.IsNullOrWhiteSpace(c.Value))
            .Select(c => new MatchInput { Object = c.Object?.Trim()!, Value = c.Value?.Trim()! })];

        Consequence = new()
        {
            Object = Consequence?.Object?.Trim() ?? "",
            Value = Consequence?.Value?.Trim() ?? "",
        };
    }

    public bool IsValid()
        => Conditions.Count > 0 && Conditions.All(c => c.IsFilled()) && Consequence.IsFilled();

    public RuleSet ToRuleSet() => new()
    {
        Conditions = [.. Conditions.Select(c => new Match { Object = c.Object, Value = c.Value })],
        Consequence = new Match { Object = Consequence.Object, Value = Consequence.Value },
    };
}
