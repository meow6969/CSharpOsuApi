using System.Text.Json.Serialization;
using CSharpOsuApi.JsonUtils;

namespace CSharpOsuApi.Models.UserModels;

public class User : OsuClass
{
    // TODO: add more of the attributes as defined here https://osu.ppy.sh/docs/index.html#user-optionalattributes
    [JsonPropertyName("avatar_url")]
    public required string AvatarUrl { get; init; }
    [JsonPropertyName("country_code")]
    public required string CountryCode { get; init; }
    [JsonPropertyName("default_group")]
    public string? DefaultGroup { get; init; }
    [JsonPropertyName("id")]
    public required int Id { get; init; }
    [JsonPropertyName("is_active")]
    public required bool IsActive { get; init; }
    [JsonPropertyName("is_bot")]
    public required bool IsBot { get; init; }
    [JsonPropertyName("is_deleted")]
    public required bool IsDeleted { get; init; }
    [JsonPropertyName("is_online")]
    public required bool IsOnline { get; init; }
    [JsonPropertyName("is_supporter")]
    public required bool IsSupporter { get; init; }
    [JsonPropertyName("last_visit")]
    [JsonConverter(typeof(Converters.TimestampDatetimeJsonConverter))]
    public DateTime? LastVisit { get; init; }
    [JsonPropertyName("pm_friends_only")]
    public required bool PmFriendsOnly { get; init; }
    [JsonPropertyName("profile_colour")]
    public string? ProfileColour { get; init; }
    [JsonPropertyName("username")]
    public required string Username { get; init; }
}