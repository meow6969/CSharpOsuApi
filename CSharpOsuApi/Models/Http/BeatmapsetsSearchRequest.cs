using System.Text.Json.Serialization;
using CSharpOsuApi.Models.Http.Query;

namespace CSharpOsuApi.Models.Http;

public class BeatmapsetsSearchRequest : OsuClass
{
    [JsonPropertyName("search")]
    public QueryParameters? Search { get; init; }
    [JsonPropertyName("recommended_difficulty")]
    public float? RecommendedDifficulty { get; init; }
    [JsonPropertyName("cursor_string")]
    public string? CursorString { get; init; }  // returns null when theres no more stuff left
}