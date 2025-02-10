using System.Text.Json.Serialization;
using CSharpOsuApi.JsonUtils;
using CSharpOsuApi.Models.OsuEnums;

// ReSharper disable ClassNeverInstantiated.Global

namespace CSharpOsuApi.Models.BeatmapModels.Metadata;

public class Nomination
{
    // TODO: check the beatmapset nominations response field
    public class BeatmapNomination : OsuClass
    {
        [JsonPropertyName("beatmapset_id")]
        public required int BeatmapsetId { get; init; }
        [JsonPropertyName("rulesets")]
        [JsonConverter(typeof(Converters.RulesetModeArrayJsonConverter))]
        public required RulesetEnum[] Rulesets { get; init; }
        [JsonPropertyName("reset")]
        public required bool Reset { get; init; }
        [JsonPropertyName("user_id")]
        public required int UserId { get; init; }
    }
    
    public class BeatmapNominationsSummaryGeneratorModel : OsuClass
    {
        [JsonPropertyName("main_ruleset")]
        [JsonConverter(typeof(Converters.RulesetModeIntJsonConverter))]
        public required RulesetEnum MainRuleset { get; init; }
        [JsonPropertyName("non_main_ruleset")]
        [JsonConverter(typeof(Converters.RulesetModeIntJsonConverter))]
        public required RulesetEnum NonMainRuleset { get; init; }
    }
    
    
    // TODO: double check this type 
    public class NominationsSummary : OsuClass
    {
        [JsonPropertyName("current")]
        public required int Current { get; init; }
        [JsonPropertyName("eligible_main_rulesets")]
        [JsonConverter(typeof(Converters.RulesetModeArrayJsonConverter))]
        public required RulesetEnum[] EligibleMainRulesets { get; init; }
        [JsonPropertyName("required_meta")]
        public required BeatmapNominationsSummaryGeneratorModel RequiredMeta { get; init; }
    }
}

