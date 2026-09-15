using Newtonsoft.Json;
using ExpertSystem.Core.Models;

namespace ExpertSystem.Core;

/// <summary>
/// Класс определяет методы для обработки:
/// <list type="number">
///     <item>
///         <description>Пользовательского ввода.</description>
///     </item>
///     <item>
///         <description>Файла с базой правил.</description>
///     </item>
/// </list>
/// </summary>
public static class Parser
{
    #region Serialization
    public static void SaveRuleSets(List<RuleSet> ruleSets)
    {
        string serialized = JsonConvert.SerializeObject(ruleSets);
        WriteToFile(serialized);
    }
    #endregion

    #region Filesystem
    private static void WriteToFile(string content)
        => File.WriteAllText(
            Path.Combine(
                Environment.CurrentDirectory,
                 "rulesets.rs"),
            content);
    #endregion
}
