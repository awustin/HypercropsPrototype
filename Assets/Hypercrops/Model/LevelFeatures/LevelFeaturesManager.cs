using System;
using System.Collections.Generic;
using UnityEngine;

using Assets.Hypercrops.Common;
using LFType = Assets.Hypercrops.Common.Enums.LevelFeatureType;
using Assets.Hypercrops.State;

namespace Assets.Hypercrops.Model.LevelFeatures
{
    public class LevelFeaturesManager : MonoBehaviour
    {
        public GameState State;
        public DataLoader Loader;
        private readonly Dictionary<LFType, LevelFeature> _features = new ();

        void Start()
        {
            Loader = DataLoader.Instance;
            State = GameState.Instance;

            Initialise();
        }

        public void Initialise()
        {
            if (_features.Count == 0)
            {
                State.ClearLevelFeatures();

                foreach (var entry in Loader.LoadLevelFeatureDescriptors())
                {
                    Enum.TryParse(entry.Key, out LFType type);

                    _features.Add(type, new (type, entry.Value));
                    UpdateGameState(type, entry.Value);
                }
            }
        }

        private void UpdateGameState(LFType type, bool isEnabled)
        {
            if (isEnabled)
            {
                State.AddLevelFeature(type.ToString());
            }
            else
            {
                State.RemoveLevelFeature(type.ToString());
            }
        }
    }
}
