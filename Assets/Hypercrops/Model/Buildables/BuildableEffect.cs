using UnityEngine;

using Assets.Hypercrops.Common.Enums;

namespace Assets.Hypercrops.Model.Buildables
{
    public class BuildableEffect : MonoBehaviour
    {
        public BuildableEffectType Type;
        public string Name;
        public BuildableEffectPeriod Period;
        public float Radius;
        public string Description;

        public void Initialise
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
    }
}
