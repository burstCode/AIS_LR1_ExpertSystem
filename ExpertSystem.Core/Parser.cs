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

    public static List<RuleSet> LoadRuleSets(string path)
    {
        string serialized = ReadFromFile(path);
        List<RuleSet>? ruleSets =
            JsonConvert.DeserializeObject<List<RuleSet>>(serialized);

        if (ruleSets is null)
            throw new NullReferenceException("Не удалось загрузить набор правил");

        return ruleSets;
    }
    #endregion

    #region Filesystem
    private static void WriteToFile(string content)
        => File.WriteAllText(
            Path.Combine(
                Environment.CurrentDirectory,
                 "rulesets.rs"),
            content);

    private static string ReadFromFile(string path)
        => File.ReadAllText(path);
    #endregion
}
