using System;
using Newtonsoft.Json;

using Assets.Hypercrops.Common.Enums;

namespace Assets.Hypercrops.Common.Serializables
{
    [Serializable]
    public class CardDescriptor
    {
        public int Id;

        [JsonProperty("Type")]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public CardType Type;

        [JsonProperty("Rarity")]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public CardRarity Rarity;
        public string Name;
        public string Label;
#nullable enable
        public string? MapEntityName;
        public string? InvokationRule1;
        public string? InvokationRule2;
        public string? InvokationRule3;
        public string? InvokationRule4;
#nullable disable

        public override string ToString()
        {
            return $"{Label} ({Name})";
        }
    }
}
