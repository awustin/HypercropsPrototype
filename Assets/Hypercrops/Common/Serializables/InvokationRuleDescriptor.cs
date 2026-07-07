using System;
using Newtonsoft.Json;

using Assets.Hypercrops.Common.Enums;

namespace Assets.Hypercrops.Common.Serializables
{
    [Serializable]
    public class InvokationRule
    {
        public string Key;

        [JsonProperty("MatchType")]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public CardType MatchType;

        [JsonProperty("MatchRarity")]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public CardRarity MatchRarity;
#nullable enable
        public int? MatchId;

        public InvokationRule(string key, CardType matchType, CardRarity matchRarity)
        {
            Key = key;
            MatchType = matchType;
            MatchRarity = matchRarity;
        }

        public InvokationRule(string key, int matchId)
        {
            Key = key;
            MatchId = matchId;
        }
    }
}
