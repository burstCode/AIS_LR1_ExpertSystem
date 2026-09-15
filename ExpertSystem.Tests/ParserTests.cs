using ExpertSystem.Core;
using ExpertSystem.Core.Models;
using ExpertSystem.Tests.Helpers;
using Newtonsoft.Json;

namespace ExpertSystem.Tests;

public class ParserTests
{
    #region Serialization
    [Fact]
    public void SaveRuleSetsTest()
    {
        // Arrange
        List<RuleSet> ruleSets = TestContentHelper.BuildTestRuleSets();

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

    [Fact]
    public void LoadRuleSetsTest()
    {
        // Arrange
        List<RuleSet> ruleSets = TestContentHelper.BuildTestRuleSets();

        try
        {
            // Act
            Parser.SaveRuleSets(ruleSets);

            Assert.True(RuleSetsFileExists());

            List<RuleSet> loadedRuleSets =
                Parser.LoadRuleSets(GetRuleSetsFilePath());

            // Assert
            Assert.NotEmpty(loadedRuleSets);
            Assert.Equal(ruleSets.Count, loadedRuleSets.Count);
        }
        finally
        {
            File.Delete(GetRuleSetsFilePath());
        }
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
