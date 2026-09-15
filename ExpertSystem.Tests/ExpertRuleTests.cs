using ExpertSystem.Core;
using ExpertSystem.Core.Models;
using ExpertSystem.Tests.Helpers;

namespace ExpertSystem.Tests;

public class ExpertRulesTest
{
    #region Rule

    [Fact]
    public void IsStateMatchesRule_Matches()
    {
        Match state = new()
        {
            Object = "Тип",
            Value = "Игровой"
        };

        Rule rule = new()
        {
            Condition = new()
            {
                Object = "Тип",
                Value = "Игровой"
            },
            Consequence = new()
            {
                Object = "Процессор",
                Value = "Intel Core i7"
            }
        };

        bool isMatches = Expert.IsStateMatchesRule(state, rule);

        Assert.True(isMatches);
    }

    [Fact]
    public void IsStateMatchesRule_NotMatches()
    {
        Match state = new()
        {
            Object = "Тип",
            Value = "Офисный"
        };

        Rule rule = new()
        {
            Condition = new()
            {
                Object = "Тип",
                Value = "Игровой"
            },
            Consequence = new()
            {
                Object = "Процессор",
                Value = "Intel Core i7"
            }
        };

        bool isMatches = Expert.IsStateMatchesRule(state, rule);

        Assert.False(isMatches);
    }

    #endregion

    #region RuleSet

    [Fact]
    public void IsStateMatchesRuleSet_Matches()
    {
        Match state = new()
        {
            Object = "Игра",
            Value = "osu!"
        };

        RuleSet ruleSet = new()
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
        };

        bool isMatch = Expert.IsStateMatchesRuleSet(state, ruleSet);

        Assert.True(isMatch);
    }

    [Fact]
    public void IsStateMatchesRuleSet_NotMatches()
    {
        Match state = new()
        {
            Object = "Игра",
            Value = "F1"
        };

        RuleSet ruleSet = new()
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
        };

        bool isMatch = Expert.IsStateMatchesRuleSet(state, ruleSet);

        Assert.False(isMatch);
    }

    #endregion

    #region RuleSets

    [Fact]
    public void IsStatesMatchesRuleSet_Matches()
    {
        List<Match> states = new()
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
        };

        RuleSet ruleSet = new()
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
        };

        bool isMatch = Expert.IsStateMatchesRuleSet(states, ruleSet);

        Assert.True(isMatch);
    }

    [Fact]
    public void IsStatesMatchesRuleSet_NotMatches()
    {
        List<Match> states = new()
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
        };

        RuleSet ruleSet = new()
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
        };

        bool isMatch = Expert.IsStateMatchesRuleSet(states, ruleSet);

        Assert.False(isMatch);
    }

    #endregion

    #region Think

    [Fact]
    public void ExpertThinkTest()
    {
        List<RuleSet> ruleSets = TestContentHelper.BuildTestRuleSets();
        List<Match> startState = TestContentHelper.BuildStartState();

        Match? finalState = Expert.Think(ruleSets, startState);

        Assert.NotNull(finalState);
        Assert.Equal("Блок питания", finalState.Object);
        Assert.Equal("500w", finalState.Value);
    }

    #endregion
}
