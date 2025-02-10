using System.Text.Json.Serialization;
using CSharpOsuApi.JsonUtils;
using CSharpOsuApi.Models.BeatmapModels.Metadata;
using CSharpOsuApi.Models.OsuEnums;

namespace CSharpOsuApi.Models.BeatmapModels;

public abstract class BeatmapGeneratorModel : OsuClass
{
    [JsonPropertyName("beatmapset_id")]
    public required int BeatmapsetId { get; init; }
    [JsonPropertyName("difficulty_rating")]
    public required float DifficultyRating { get; init; }
    [JsonPropertyName("id")]
    public required int Id { get; init; }
    [JsonPropertyName("mode")]
    [JsonConverter(typeof(Converters.RulesetModeJsonConverter))]
    public required RulesetEnum Mode { get; init; }
    [JsonPropertyName("status")]
    [JsonConverter(typeof(Converters.RankStatusStringToRankedEnumJsonConverter))]
    public required RankedEnum Status { get; init; }
    [JsonPropertyName("total_length")]
    public required int TotalLength { get; init; }
    [JsonPropertyName("user_id")]
    public required int UserId { get; init; }
    [JsonPropertyName("version")]
    public required string Version { get; init; }
    [JsonPropertyName("checksum")]
    public string? Checksum { get; init; }
    [JsonPropertyName("failtimes")]
    public BeatmapFailtimes? Failtimes { get; init; }
    [JsonPropertyName("max_combo")]
    public int? MaxCombo { get; init; }
}

public class Beatmap : BeatmapGeneratorModel
{
    [JsonPropertyName("beatmapset")]
    public Beatmapset? Beatmapset { get; set; }
}