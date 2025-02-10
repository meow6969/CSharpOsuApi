using System.Text.Json.Serialization;

namespace CSharpOsuApi.Models.Http.Query;

public class CursorPagination : OsuClass
{
    [JsonPropertyName("approved_date")]
    public long? ApprovedDate { get; init; }  // if the cursor reaches the end it returns null for everything
    [JsonPropertyName("id")]
    public int? Id { get; init; }
}