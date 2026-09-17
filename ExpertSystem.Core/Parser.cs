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
    /// <summary>
    /// Сохранение списка правил в файл rulesets.rs.
    /// </summary>
    /// <param name="ruleSets">Список правил.</param>
    public static void SaveRuleSets(List<RuleSet> ruleSets)
    {
        string serialized = JsonConvert.SerializeObject(ruleSets);
        WriteToFile(serialized);
    }

    /// <summary>
    /// Загрузить список правил из файла.
    /// </summary>
    /// <param name="path">Путь к файлу.</param>
    /// <returns>Десериализованный список правил.</returns>
    /// <exception cref="NullReferenceException"></exception>
    public static List<RuleSet> LoadRuleSets(string? path = null)
    {
        if (path is null)
            path = GetDefaultRuleSetsFilePath();

        EnsureRuleSetsFileExists(path);

        string serialized  = ReadFromFile(path);
        List<RuleSet>? ruleSets =
            JsonConvert.DeserializeObject<List<RuleSet>>(serialized);

        if (ruleSets is null)
            throw new NullReferenceException("Не удалось загрузить набор правил");

        return ruleSets;
    }
    #endregion

    #region Filesystem
    private static void WriteToFile(string content)
        => File.WriteAllText(GetDefaultRuleSetsFilePath(), content);

    private static string ReadFromFile(string path)
        => File.ReadAllText(path);

    private static string GetDefaultRuleSetsFilePath()
        => Path.Combine(Environment.CurrentDirectory, "rulesets.rs");

    private static void EnsureRuleSetsFileExists(string path)
    {
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "[]");
        }
    }
    #endregion
}
