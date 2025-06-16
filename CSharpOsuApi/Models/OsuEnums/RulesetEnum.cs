using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CSharpOsuApi.Models.OsuEnums;

public enum RulesetEnum
{
    Osu = 0,
    Taiko = 1,
    Fruits = 2,
    Mania = 3
}

public static class RulesetEnumExtensions
{
    public static string OsuApiName(this RulesetEnum ruleset)
    {
        return ruleset.ToString().ToLower();
    }
}
