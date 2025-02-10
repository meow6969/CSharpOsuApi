using System.Text.Json.Serialization;
using CSharpOsuApi.JsonUtils;
using CSharpOsuApi.Models.BeatmapModels.Metadata;
using CSharpOsuApi.Models.OsuEnums;

// ReSharper disable ClassNeverInstantiated.Global

namespace CSharpOsuApi.Models.BeatmapModels;

public class BeatmapsetExtended : BeatmapsetGeneratorModel
{
    [JsonPropertyName("beatmaps")]
    public BeatmapExtended[]? Beatmaps { get; init; }
    
    public List<RulesetEnum> AllGameModes { get {
        if (Beatmaps == null) return [];
        List<RulesetEnum> gameModes = [];
        foreach (BeatmapExtended beatmap in Beatmaps)
        {
            if (!gameModes.Contains(beatmap.Mode)) gameModes.Add(beatmap.Mode);
        }

        return gameModes;
    } }
    
    [JsonPropertyName("availability")]
    public required BeatmapAvailability Availability { get; init; }
    [JsonPropertyName("bpm")]
    public required float Bpm { get; init; }
    [JsonPropertyName("can_be_hyped")]
    public required bool CanBeHyped { get; init; }
    [JsonPropertyName("deleted_at")]
    [JsonConverter(typeof(Converters.TimestampDatetimeJsonConverter))]
    public DateTime? DeletedAt { get; init; }
    [JsonPropertyName("discussion_enabled")]
    public required bool DiscussionEnabled { get; init; }
    [JsonPropertyName("discussion_locked")]
    public required bool DiscussionLocked { get; init; }
    [JsonPropertyName("hype")]
    public BeatmapHype? Hype { get; init; }
    [JsonPropertyName("is_scoreable")]
    public required bool IsScoreable { get; init; }
    [JsonPropertyName("last_updated")]
    [JsonConverter(typeof(Converters.TimestampDatetimeJsonConverter))]
    public required DateTime LastUpdated { get; init; }
    [JsonPropertyName("legacy_thread_url")]
    public string? LegacyThreadUrl { get; init; }
    [JsonPropertyName("nominations_summary")]
    public required Nomination.NominationsSummary NominationsSummary { get; init; }
    [JsonPropertyName("ranked")]
    [JsonConverter(typeof(Converters.RankStatusIntToRankedEnumJsonConverter))]
    public required RankedEnum Ranked { get; init; }
    [JsonPropertyName("ranked_date")]
    [JsonConverter(typeof(Converters.TimestampDatetimeJsonConverter))]
    public DateTime? RankedDate { get; init; }
    [JsonPropertyName("storyboard")]
    public required bool Storyboard { get; init; }
    [JsonPropertyName("submitted_date")]
    [JsonConverter(typeof(Converters.TimestampDatetimeJsonConverter))]
    public DateTime? SubmittedDate { get; init; }
    [JsonPropertyName("tags")]
    public required string Tags { get; init; }
}