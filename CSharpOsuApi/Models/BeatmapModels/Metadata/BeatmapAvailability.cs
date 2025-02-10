using System.Text.Json.Serialization;

namespace CSharpOsuApi.Models.BeatmapModels.Metadata;

public class BeatmapAvailability : OsuClass
{
    [JsonPropertyName("download_disabled")]
    public required bool DownloadDisabled { get; init; }
    [JsonPropertyName("more_information")]
    public string? MoreInformation { get; init; }
}