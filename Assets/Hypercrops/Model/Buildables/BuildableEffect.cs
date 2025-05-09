using UnityEngine;

using Assets.Hypercrops.Common.Enums;
using Assets.Hypercrops.Events;

namespace Assets.Hypercrops.Model.Buildables
{
    // TODO: Make a "task runner" to subscribe PerformAction effects and another one for "enabled features" for EnableFeature effects
    public class BuildableEffect : MonoBehaviour
    {
        public GameEventSender Sender;
        public BuildableEffectType Type;
        public LevelFeatureType LevelFeature;
        public string Name;
        public BuildableEffectPeriod Period;
        public float Radius;
        public string Description;

        void Awake()
        {
            Sender = GameEventSender.Instance;
        }

        public void Initialise
        (
            BuildableEffectType type,
            LevelFeatureType levelFeature,
            string name,
            BuildableEffectPeriod period,
            float radius,
            string description
        )
        {
            Type = type;
            LevelFeature = levelFeature;
            Name = name;
            Period = period;
            Radius = radius;
            Description = description;

            StartEffect();
        }

        private void StartEffect()
        {
            if (Type == BuildableEffectType.EnableFeature && LevelFeature != LevelFeatureType.None)
            {
                Sender.BroadcastEvent("EnableFeature", LevelFeature.ToString());
            }
        }
    }
}
