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
        string serialized = JsonConvert.SerializeObject(ruleSets, Formatting.Indented);
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

        string serialized = ReadFromFile(path);
        List<RuleSet>? ruleSets =
            JsonConvert.DeserializeObject<List<RuleSet>>(serialized);

        if (ruleSets is null)
            throw new NullReferenceException("Не удалось загрузить набор правил");

        return ruleSets;
    }

    /// <summary>
    /// Разобрать и проверить свод правил из JSON-текста.
    /// </summary>
    /// <param name="json">Содержимое файла.</param>
    /// <exception cref="FormatException">Формат не соответствует своду правил.</exception>
    public static List<RuleSet> ParseRuleSets(string json)
    {
        List<RuleSet>? ruleSets;
        try
        {
            ruleSets = JsonConvert.DeserializeObject<List<RuleSet>>(json);
        }
        catch (JsonException e)
        {
            throw new FormatException("Файл не является корректным сводом правил.", e);
        }

        if (ruleSets is null)
            throw new FormatException("Файл не содержит свода правил.");

        for (int i = 0; i < ruleSets.Count; i++)
        {
            RuleSet? rs = ruleSets[i];

            bool valid = rs?.Conditions is { Count: > 0 } conditions
                && conditions.All(IsFilled)
                && IsFilled(rs.Consequence);

            if (!valid)
                throw new FormatException(
                    $"Правило #{i + 1} некорректно: нужны хотя бы одно условие и следствие с непустыми объектом и значением.");
        }

        return ruleSets;
    }

    private static bool IsFilled(Match? m)
        => m is not null
            && !string.IsNullOrWhiteSpace(m.Object)
            && !string.IsNullOrWhiteSpace(m.Value);
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
