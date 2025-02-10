using System.Text.Json.Serialization;
using CSharpOsuApi.JsonUtils;
using CSharpOsuApi.Models.BeatmapModels.Metadata;
using CSharpOsuApi.Models.OsuEnums;

namespace CSharpOsuApi.Models.BeatmapModels;

public abstract class BeatmapsetGeneratorModel : OsuClass
{
    [JsonPropertyName("artist")]
    public required string Artist { get; init; }
    [JsonPropertyName("artist_unicode")]
    public required string ArtistUnicode { get; init; }
    [JsonPropertyName("covers")]
    public required BeatmapCovers Covers { get; init; }
    [JsonPropertyName("creator")]
    public required string Creator { get; init; }
    [JsonPropertyName("favourite_count")]
    public required int FavouriteCount { get; init; }
    [JsonPropertyName("id")]
    public required int Id { get; init; }
    [JsonPropertyName("nsfw")]
    public required bool Nsfw { get; init; }
    [JsonPropertyName("offset")]
    public required int Offset { get; init; }
    [JsonPropertyName("play_count")]
    public required int PlayCount { get; init; }
    [JsonPropertyName("preview_url")]
    public required string PreviewUrl { get; init; }
    [JsonPropertyName("source")]
    public required string Source { get; init; }
    [JsonPropertyName("status")]
    [JsonConverter(typeof(Converters.RankStatusStringToRankedEnumJsonConverter))]
    public required RankedEnum Status { get; init; }
    [JsonPropertyName("spotlight")]
    public required bool Spotlight { get; init; }
    [JsonPropertyName("title")]
    public required string Title { get; init; }
    [JsonPropertyName("title_unicode")]
    public required string TitleUnicode { get; init; }
    [JsonPropertyName("user_id")]
    public required int UserId { get; init; }
    [JsonPropertyName("video")]
    public required bool Video { get; init; }
    [JsonPropertyName("current_nominations")]
    public Nomination[]? CurrentNominations { get; init; }
    [JsonPropertyName("has_favourited")]
    public bool? HasFavourited { get; init; }
    [JsonPropertyName("pack_tags")]
    public string[]? PackTags { get; init; }
    [JsonPropertyName("track_id")]
    public int? TrackId { get; init; }
}

public class Beatmapset : BeatmapsetGeneratorModel
{
    [JsonPropertyName("beatmaps")]
    public Beatmap[]? Beatmaps { get; init; }
    
    public List<RulesetEnum> AllGameModes { get {
        if (Beatmaps == null) return [];
        List<RulesetEnum> gameModes = [];
        foreach (Beatmap beatmap in Beatmaps)
        {
            if (!gameModes.Contains(beatmap.Mode)) gameModes.Add(beatmap.Mode);
        }

        return gameModes;
    } }
}