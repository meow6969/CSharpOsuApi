using System.Text.Json.Serialization;

namespace CSharpOsuApi.Models.BeatmapModels.Metadata;

// ReSharper disable once ClassNeverInstantiated.Global
public class BeatmapCovers : OsuClass
{
    [JsonPropertyName("cover")]
    public required string Cover { get; init; }
    [JsonPropertyName("cover@2x")]
    public required string Cover2X { get; init; }
    [JsonPropertyName("card")]
    public required string Card { get; init; }
    [JsonPropertyName("card@2x")]
    public required string Card2X { get; init; }
    [JsonPropertyName("list")]
    public required string List { get; init; }
    [JsonPropertyName("list@2x")]
    public required string List2X { get; init; }
    [JsonPropertyName("slimcover")]
    public required string Slimcover { get; init; }
    [JsonPropertyName("slimcover2x")]
    public string? Slimcover2X { get; init; }
}