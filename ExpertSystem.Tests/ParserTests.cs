using ExpertSystem.Core;
using ExpertSystem.Core.Models;
using Newtonsoft.Json;

namespace ExpertSystem.Tests;

public class ParserTests
{
    #region Serialization
    [Fact]
    public void SaveRuleSetsTest()
    {
        // Arrange
        List<RuleSet> ruleSets = new()
        {
            new()
            {
                Conditions = new()
                {
                    new()
                    {
                        Object = "Тип",
                        Value = "Офисный"
                    },
                    new()
                    {
                        Object = "Игра",
                        Value = "osu!"
                    }
                },
                Consequence = new()
                {
                    Object = "Процессор",
                    Value = "Intel Core i3"
                }
            },
            new()
            {
                Conditions = new()
                {
                    new()
                    {
                        Object = "Тип",
                        Value = "Игровой"
                    },
                    new()
                    {
                        Object = "Игра",
                        Value = "F1"
                    }
                },
                Consequence = new()
                {
                    Object = "Процессор",
                    Value = "Intel Core i7"
                }
            }
        };

        // Act
        Parser.SaveRuleSets(ruleSets);

        // Assert
        Assert.True(RuleSetsFileExists());

        try
        {
            string serialized = File.ReadAllText(GetRuleSetsFilePath());
            List<RuleSet>? deserialized = JsonConvert.DeserializeObject<List<RuleSet>>(serialized);
            Assert.NotNull(deserialized);
            Assert.Equal(ruleSets.Count, deserialized.Count);
        }
        finally
        {
            File.Delete(GetRuleSetsFilePath());
        }
        Assert.False(RuleSetsFileExists());
    }
    #endregion

    #region Filesystem helpers

    private bool RuleSetsFileExists() =>
        File.Exists(GetRuleSetsFilePath());

    private string GetRuleSetsFilePath() =>
        Path.Combine(
            Environment.CurrentDirectory,
            "rulesets.rs");

    #endregion
}
