using System;
using Newtonsoft.Json;

using Assets.Classes.Common.Enums;

namespace Assets.Classes.System.CommonSerializable
{
    [Serializable]
    public class CardDescriptor
    {
        public int Id;

        [JsonProperty("Type")]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public CardType Type;
        public string Name;
        public string Label;

        #nullable enable
        public string? Attribute;
        #nullable disable

        public override string ToString()
        {
            return $"{Label} ({Name})";
        }
    }
}
