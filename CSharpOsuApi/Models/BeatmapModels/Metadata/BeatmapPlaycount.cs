using System.Text.Json.Serialization;

namespace CSharpOsuApi.Models.BeatmapModels.Metadata;

// ReSharper disable ClassNeverInstantiated.Global

public class BeatmapPlaycount : OsuClass
{
    [JsonPropertyName("beatmap_id")]
    public int BeatmapId { get; init; }
    [JsonPropertyName("beatmap")]
    public Beatmap? Beatmap { get; init; }
    [JsonPropertyName("beatmapset")]
    public Beatmapset? Beatmapset { get; init; }
    [JsonPropertyName("count")]
    public int Count { get; init; }
    [JsonIgnore]
    public int PlayCount => Count;
}