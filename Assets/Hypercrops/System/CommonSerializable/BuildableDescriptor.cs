using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Assets.Hypercrops.Common.Enums;

namespace Assets.Hypercrops.System.CommonSerializable
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
            JsonProperty("InteractionType"),
            JsonConverter(typeof(StringEnumConverter))
        ]
        public BuildableInteractionType InteractionType;
        [
            JsonProperty("LayoutType"),
            JsonConverter(typeof(StringEnumConverter))
        ]
        public BuildableLayoutType LayoutType;
        public bool Rotatable;
        public BuildableEffect Effect;
        public string OnClick;
        public string Description;

        public BuildableDescriptor
        (
            BuildableType type,
            BuildableInteractionType interactionType,
            BuildableLayoutType layoutType,
            bool rotatable,
            BuildableEffect effect,
            string onClick
        )
        {
            Type = type;
            InteractionType = interactionType;
            LayoutType = layoutType;
            Rotatable = rotatable;
            Effect = effect;
            OnClick = onClick;
        }
        
        override public string ToString()
        {
            return $"Descriptor for {Type}\n\nRotatable: {Rotatable}\nInteraction type: {InteractionType}\n{Effect}";
        }
    }

    [Serializable]
    public class BuildableEffect
    {
        public string Name;
        [
            JsonProperty("Period"),
            JsonConverter(typeof(StringEnumConverter))
        ]
        public BuildableEffectPeriod Period;
        public float Radius;

        public BuildableEffect(string name, BuildableEffectPeriod period, float radius)
        {
            Name = name;
            Period = period;
            Radius = radius;
        }

        public override string ToString()
        {
            return $"Effect name: {Name}\nPeriod: {Period}";
        }
    }
}