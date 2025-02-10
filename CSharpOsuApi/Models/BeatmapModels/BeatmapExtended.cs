using System.Text.Json.Serialization;
using CSharpOsuApi.JsonUtils;
using CSharpOsuApi.Models.OsuEnums;

// ReSharper disable ClassNeverInstantiated.Global

namespace CSharpOsuApi.Models.BeatmapModels;

public class BeatmapExtended : BeatmapGeneratorModel
{
    [JsonPropertyName("beatmapset")] 
    public BeatmapsetExtended? Beatmapset { get; set; }
    [JsonPropertyName("accuracy")]
    public required float Accuracy { get; init; }
    [JsonPropertyName("ar")]
    public required float Ar { get; init; }
    [JsonPropertyName("bpm")]
    public float? Bpm { get; init; }
    [JsonPropertyName("convert")]
    public required bool Convert { get; init; }
    [JsonPropertyName("count_circles")]
    public required int CountCircles { get; init; }
    [JsonPropertyName("count_sliders")]
    public required int CountSliders { get; init; }
    [JsonPropertyName("count_spinners")]
    public required int CountSpinners { get; init; }
    // ReSharper disable once InconsistentNaming
    [JsonPropertyName("cs")]
    public required float CS { get; init; }
    [JsonPropertyName("deleted_at")]
    [JsonConverter(typeof(Converters.TimestampDatetimeJsonConverter))]
    public DateTime? DeletedAt { get; init; }
    [JsonPropertyName("drain")]
    public required float Drain { get; init; }
    [JsonPropertyName("hit_length")]
    public required int HitLength { get; init; }
    [JsonPropertyName("is_scoreable")]
    public required bool IsScoreable { get; init; }
    [JsonPropertyName("last_updated")]
    [JsonConverter(typeof(Converters.TimestampDatetimeJsonConverter))]
    public required DateTime LastUpdated { get; init; }
    [JsonPropertyName("mode_int")]
    [JsonConverter(typeof(Converters.RulesetModeIntJsonConverter))]
    public required RulesetEnum ModeInt { get; init; }
    [JsonPropertyName("passcount")]
    public required int Passcount { get; init; }
    [JsonPropertyName("playcount")]
    public required int Playcount { get; init; }
    [JsonPropertyName("ranked")]
    [JsonConverter(typeof(Converters.RankStatusIntToRankedEnumJsonConverter))]
    public required RankedEnum Ranked { get; init; }
    [JsonPropertyName("url")]
    public required string Url { get; init; }
}