using ExpertSystem.Core.Models;

namespace ExpertSystem.Tests.Helpers;

public static class TestContentHelper
{
    public static List<Match> BuildStartState()
        => new()
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

    public static List<RuleSet> BuildTestRuleSets()
        => new()
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
                        Object = "Процессор",
                        Value = "Intel Core i3"
                    }
                },
                Consequence = new()
                {
                    Object = "Видеокарта",
                    Value = "Nvidia GeForce GTX1050"
                }
            },
            new()
            {
                Conditions = new()
                {
                    new()
                    {
                        Object = "Видеокарта",
                        Value = "nvidia geforce gtx1050"
                    }
                },
                Consequence = new()
                {
                    Object = "Блок питания",
                    Value = "500w"
                }
            },
            new()
            {
                Conditions = new()
                {
                    new()
                    {
                        Object = "Видеокарта",
                        Value = "nvidia geforce rtx5090"
                    }
                },
                Consequence = new()
                {
                    Object = "Блок питания",
                    Value = "1000w"
                }
            }
        };
}
