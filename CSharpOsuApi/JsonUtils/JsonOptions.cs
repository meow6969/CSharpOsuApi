using System.Text.Json;

namespace CSharpOsuApi.JsonUtils;

public static class JsonOptions
{
    public static readonly JsonSerializerOptions PrettyPrint = new ()
    {
        WriteIndented = true
    };
    
    public static readonly JsonSerializerOptions RulesetConverter = new ()
    {
        Converters = { new Converters.RulesetModeJsonConverter() }
    };
}