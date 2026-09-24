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

    #region ParseRuleSets
    [Fact]
    public void ParseRuleSets_Valid()
    {
        string json = JsonConvert.SerializeObject(TestContentHelper.BuildTestRuleSets());

        List<RuleSet> parsed = Parser.ParseRuleSets(json);

        Assert.Equal(4, parsed.Count);
    }

    [Theory]
    [InlineData("не json")]
    [InlineData("null")]
    [InlineData("[{\"Conditions\":[],\"Consequence\":{\"Object\":\"a\",\"Value\":\"b\"}}]")]
    [InlineData("[{\"Conditions\":[{\"Object\":\"a\",\"Value\":\" \"}],\"Consequence\":{\"Object\":\"a\",\"Value\":\"b\"}}]")]
    [InlineData("[null]")]
    public void ParseRuleSets_Invalid_Throws(string json)
    {
        Assert.Throws<FormatException>(() => Parser.ParseRuleSets(json));
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
