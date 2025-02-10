using System.Text.Json.Serialization;

namespace CSharpOsuApi.Models.Http.Query;

public class OAuthTokenResponse : OsuClass
{
    [JsonPropertyName("token_type")]
    public required string TokenType { get; init; }
    [JsonPropertyName("expires_in")]
    public required int ExpiresIn { get; init; }
    [JsonIgnore]
    public DateTime ExpiresAt { get; set; }
    public long ExpiresAtUnixTime { get; set; }
    [JsonPropertyName("access_token")]
    public required string AccessToken { get; init; }
    [JsonPropertyName("refresh_token")]
    public required string RefreshToken { get; init; }
}