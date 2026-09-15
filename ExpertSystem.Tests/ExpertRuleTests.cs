using ExpertSystem.Core;
using ExpertSystem.Core.Models;

namespace ExpertSystem.Tests;

public class ExpertRulesTest
{
    #region Rule

    [Fact]
    public void IsStateMathesRule_Matches()
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

        bool isMathes = Expert.IsStateMathesRule(state, rule);

        Assert.True(isMathes);
    }

    [Fact]
    public void IsStateMathesRule_NotMatches()
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

        bool isMathes = Expert.IsStateMathesRule(state, rule);

        Assert.False(isMathes);
    }

    #endregion

    #region RuleSet

    [Fact]
    public void IsStateMathesRuleSet_Mathes()
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

        bool isMatch = Expert.IsStateMathesRuleSet(state, ruleSet);

        Assert.True(isMatch);
    }

    [Fact]
    public void IsStateMathesRuleSet_NotMathes()
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

        bool isMatch = Expert.IsStateMathesRuleSet(state, ruleSet);

        Assert.False(isMatch);
    }

    #endregion

    #region RuleSets

    [Fact]
    public void IsStatesMathesRuleSet_Mathes()
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

        bool isMatch = Expert.IsStateMathesRuleSet(states, ruleSet);

        Assert.True(isMatch);
    }

    [Fact]
    public void IsStatesMathesRuleSet_NotMathes()
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

        bool isMatch = Expert.IsStateMathesRuleSet(states, ruleSet);

        Assert.False(isMatch);
    }

    #endregion
}
