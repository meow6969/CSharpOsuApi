using System.Text.Json.Serialization;

namespace CSharpOsuApi.Models.BeatmapModels.Metadata;

public class BeatmapFailtimes
{
    [JsonPropertyName("exit")]
    public int[]? Exit { get; init; }
    [JsonPropertyName("fail")]
    public int[]? Fail { get; init; }
}