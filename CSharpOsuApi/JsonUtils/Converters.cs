using System.Text.Json;
using System.Text.Json.Serialization;
using CSharpOsuApi.Models.OsuEnums;

// ReSharper disable MemberCanBePrivate.Global

namespace CSharpOsuApi.JsonUtils;

public abstract class Converters
{
    private static readonly Dictionary<int, RulesetEnum> IntToRulesetEnum = new Dictionary<int, RulesetEnum>()
    {
        { 0, RulesetEnum.Osu },
        { 1, RulesetEnum.Taiko },
        { 2, RulesetEnum.Fruits },
        { 3, RulesetEnum.Mania }
    };
    
    public class RulesetModeIntJsonConverter : JsonConverter<RulesetEnum>
    {
        public override RulesetEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            int mode = reader.GetInt32();

            return IntToRulesetEnum[mode];
        }

        public override void Write(Utf8JsonWriter writer, RulesetEnum rulesetToConvert, 
            JsonSerializerOptions options) => writer.WriteNumberValue((int)rulesetToConvert);
    }
    
    public class RulesetModeIntArrayJsonConverter : JsonConverter<RulesetEnum[]>
    {
        public override RulesetEnum[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }
            reader.Read();
            List<RulesetEnum> rulesetEnums = [];
            while (reader.TokenType != JsonTokenType.EndArray)
            {
                rulesetEnums.Add(JsonSerializer.Deserialize<RulesetEnum>(ref reader, options));
                reader.Read();
            }

            return rulesetEnums.ToArray();
        }

        public override void Write(Utf8JsonWriter writer, RulesetEnum[] rulesetToConvert, 
            JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            foreach (RulesetEnum rankedEnum in rulesetToConvert.Reverse())
            {
                JsonSerializer.Serialize(writer, (int)rankedEnum, options);
            }
            
            writer.WriteEndArray();
        }
    }
    
    public static readonly Dictionary<string, RulesetEnum> StringToRulesetEnum = new()
    {
        { "osu", RulesetEnum.Osu },
        { "taiko", RulesetEnum.Taiko },
        { "fruits", RulesetEnum.Fruits },
        { "mania", RulesetEnum.Mania }
    };
        
    public static readonly Dictionary<RulesetEnum, string> RulesetEnumToString = 
        StringToRulesetEnum.ToDictionary(x => x.Value, x => x.Key);
    
    public class RulesetModeJsonConverter : JsonConverter<RulesetEnum>
    {
        public override RulesetEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? mode = reader.GetString();
            if (mode == null) throw new NullReferenceException($"RulesetModeJsonConverter: mode is null");

            return StringToRulesetEnum[mode];
        }

        public override void Write(Utf8JsonWriter writer, RulesetEnum rulesetToConvert, 
            JsonSerializerOptions options) => writer.WriteStringValue(RulesetEnumToString[rulesetToConvert]);
    }
    
    public class RulesetModeArrayJsonConverter : JsonConverter<RulesetEnum[]>
    {
        public override RulesetEnum[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }
            reader.Read();
            List<RulesetEnum> rulesetEnums = [];
            while (reader.TokenType != JsonTokenType.EndArray)
            {
                rulesetEnums.Add(JsonSerializer.Deserialize<RulesetEnum>(ref reader, 
                    JsonOptions.RulesetConverter));
                reader.Read();
            }

            return rulesetEnums.ToArray();
        }

        public override void Write(Utf8JsonWriter writer, RulesetEnum[] rulesetToConvert, 
            JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            foreach (RulesetEnum rankedEnum in rulesetToConvert.Reverse())
            {
                JsonSerializer.Serialize(writer, RulesetEnumToString[rankedEnum], options);
            }
            
            writer.WriteEndArray();
        }
    }
    
    public class TimestampDatetimeJsonConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? mode = reader.GetString();
            if (mode == null) throw new NullReferenceException("TimestampDatetimeJsonConverter: mode is null");

            return DateTime.Parse(mode);
        }

        public override void Write(Utf8JsonWriter writer, DateTime dateTimeToConvert, 
            JsonSerializerOptions options) => 
                writer.WriteStringValue(dateTimeToConvert.ToString("yyyy-MM-ddTHH:mm:ssK"));
    }
    
    public static readonly Dictionary<string, RankedEnum> StringToRankedEnum = new Dictionary<string, RankedEnum>()
    {
        { "graveyard", RankedEnum.Graveyard },
        { "wip", RankedEnum.Wip },
        { "pending", RankedEnum.Pending },
        { "ranked", RankedEnum.Ranked },
        { "approved", RankedEnum.Approved },
        { "qualified", RankedEnum.Qualified },
        { "loved", RankedEnum.Loved }
    };
        
    public static readonly Dictionary<RankedEnum, string> RankedEnumToString = 
        StringToRankedEnum.ToDictionary(x => x.Value, x => x.Key);
    
    public class RankStatusStringToRankedEnumJsonConverter : JsonConverter<RankedEnum>
    {
        public override RankedEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? mode = reader.GetString();
            if (mode == null) throw new NullReferenceException("RankStatusStringToRankedEnumJsonConverter: mode is null");

            return StringToRankedEnum[mode];
        }

        public override void Write(Utf8JsonWriter writer, RankedEnum rankedEnumToConvert, JsonSerializerOptions options)
        {
            writer.WriteStringValue(RankedEnumToString[rankedEnumToConvert]);
        }
    }
    
    public class RankStatusStringArrayToRankedEnumArrayJsonConverter : JsonConverter<RankedEnum[]>
    {
        public override RankedEnum[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }
            reader.Read();
            List<RankedEnum> rankedEnums = [];
            while (reader.TokenType != JsonTokenType.EndArray)
            {
                rankedEnums.Add(JsonSerializer.Deserialize<RankedEnum>(ref reader, options));
                reader.Read();
            }

            return rankedEnums.ToArray();
        }

        public override void Write(Utf8JsonWriter writer, RankedEnum[] rankedEnumsToConvert, 
            JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            foreach (RankedEnum rankedEnum in rankedEnumsToConvert.Reverse())
            {
                JsonSerializer.Serialize(writer, rankedEnum, options);
            }
            
            writer.WriteEndArray();
        }
    }
    
    public static readonly Dictionary<int, RankedEnum> IntToRankedEnum = new()
    {
        { -2, RankedEnum.Graveyard },
        { -1, RankedEnum.Wip },
        { 0, RankedEnum.Pending },
        { 1, RankedEnum.Ranked },
        { 2, RankedEnum.Approved },
        { 3, RankedEnum.Qualified },
        { 4, RankedEnum.Loved }
    };
        
    public static readonly Dictionary<RankedEnum, int> RankedEnumToInt = 
        IntToRankedEnum.ToDictionary(x => x.Value, x => x.Key);
    
    public class RankStatusIntToRankedEnumJsonConverter : JsonConverter<RankedEnum>
    {
        public override RankedEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            int mode = reader.GetInt32();

            return IntToRankedEnum[mode];
        }

        public override void Write(Utf8JsonWriter writer, RankedEnum rankedEnumToConvert, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(RankedEnumToInt[rankedEnumToConvert]);
        }
    }
}