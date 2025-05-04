using System;
using Newtonsoft.Json;

namespace Assets.Hypercrops.System.CommonSerializable
{
    [Serializable]
    public class BuildableDescriptor
    {
        [
            JsonProperty("Type"),
            JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))
        ]
        public BuildableType Type;
        [
            JsonProperty("InteractionType"),
            JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))
        ]
        public BuildableInteractionType InteractionType;
        public bool Rotatable;
        public BuildableEffect Effect;
        public string OnClick;

        public BuildableDescriptor
        (
            BuildableType type,
            BuildableInteractionType interactionType,
            bool rotatable,
            BuildableEffect effect,
            string onClick
        )
        {
            Type = type;
            InteractionType = interactionType;
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
            JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))
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