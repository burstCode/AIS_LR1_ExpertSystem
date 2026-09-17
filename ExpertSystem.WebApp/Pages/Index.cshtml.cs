using ExpertSystem.Core;
using ExpertSystem.Core.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExpertSystem.WebApp.Pages;

public class IndexModel : PageModel
{
    public List<RuleSet> RuleSets = Parser.LoadRuleSets();

    public void OnGet()
    {
    }
}
