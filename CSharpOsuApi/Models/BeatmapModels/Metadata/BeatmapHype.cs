using System.Text.Json.Serialization;

namespace CSharpOsuApi.Models.BeatmapModels.Metadata;

public class BeatmapHype : OsuClass
{
    [JsonPropertyName("current")]
    public int? Current { get; init; }
    [JsonPropertyName("required")]
    public int? Required { get; init; }
}