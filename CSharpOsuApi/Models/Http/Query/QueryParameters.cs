using System.Text.Json.Serialization;

namespace CSharpOsuApi.Models.Http.Query;

public class QueryParameters : OsuClass
{
    [JsonPropertyName("sort")]
    public string? Sort { get; init; }
    [JsonPropertyName("limit")]
    public int? Limit { get; init; }
    [JsonPropertyName("start")]
    public string? Start { get; init; }
    [JsonPropertyName("end")]
    public string? End { get; init; }
    [JsonPropertyName("cursor_string")]
    public string? CursorString { get; init; }
}