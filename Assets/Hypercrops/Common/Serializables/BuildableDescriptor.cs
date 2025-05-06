using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Assets.Hypercrops.Common.Enums;

namespace Assets.Hypercrops.Common.Serializables
{
    [Serializable]
    public class BuildableDescriptor
    {
        [
            JsonProperty("Type"),
            JsonConverter(typeof(StringEnumConverter))
        ]
        public BuildableType Type;
        [
            JsonProperty("LayoutType"),
            JsonConverter(typeof(StringEnumConverter))
        ]
        public BuildableLayoutType LayoutType;
        public BuildableEffectDescriptor Effect;
        public string Description;

        public BuildableDescriptor
        (
            BuildableType type,
            BuildableLayoutType layoutType,
            bool rotatable,
            BuildableEffectDescriptor effect,
            string onClick
        )
        {
            Type = type;
            LayoutType = layoutType;
            Effect = effect;
        }
        
        override public string ToString()
        {
            return $"Descriptor for {Type}\n\nLayout type: {LayoutType}\n{Effect}";
        }
    }

    [Serializable]
    public class BuildableEffectDescriptor
    {
        [
            JsonProperty("Type"),
            JsonConverter(typeof(StringEnumConverter))
        ]
        public BuildableEffectType Type;
        public string Name;
        [
            JsonProperty("Period"),
            JsonConverter(typeof(StringEnumConverter))
        ]
        public BuildableEffectPeriod Period;
        public float Radius;
        public string Description;

        public BuildableEffectDescriptor
        (
            BuildableEffectType type,
            string name,
            BuildableEffectPeriod period,
            float radius,
            string description
        )
        {
            Type = type;
            Name = name;
            Period = period;
            Radius = radius;
            Description = description;
        }

        public override string ToString()
        {
            return $"Effect name: {Name}\nEffect type: {Type}\nPeriod: {Period}";
        }
    }
}