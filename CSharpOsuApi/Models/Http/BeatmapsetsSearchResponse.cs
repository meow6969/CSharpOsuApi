using System.Text.Json.Serialization;
using CSharpOsuApi.Models.BeatmapModels;
using CSharpOsuApi.Models.Http.Query;

namespace CSharpOsuApi.Models.Http;

public class BeatmapsetsSearchResponse : BeatmapsetsSearchRequest
{
    [JsonPropertyName("beatmapsets")]
    public required BeatmapsetExtended[] Beatmapsets { get; init; }
    [JsonPropertyName("search")]
    public new required QueryParameters Search { get; init; }
    [JsonPropertyName("recommended_difficulty")]
    public new required float RecommendedDifficulty { get; init; }
    [JsonPropertyName("error")]
    public string? Error { get; init; }  // idk wut this is but it gets returned ? it seems to always be null
    [JsonPropertyName("total")]
    public required int Total { get; init; }
    [JsonPropertyName("cursor")]
    public CursorPagination? Cursor { get; init; }  // null bcs its being deprecated
}
