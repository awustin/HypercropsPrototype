using System;
using System.Collections.Generic;
using UnityEngine;

using Assets.Hypercrops.Common;
using LFType = Assets.Hypercrops.Common.Enums.LevelFeatureType;

namespace Assets.Hypercrops.Model.LevelFeatures
{
    public class LevelFeaturesManager : MonoBehaviour
    {
        public DataLoader Loader;
        private readonly Dictionary<LFType, LevelFeature> _features = new ();

        void Start()
        {
            Loader = DataLoader.Instance;
            Initialise();
        }

        public void Initialise()
        {
            if (_features.Count == 0)
            {
                foreach (var entry in Loader.LoadLevelFeatureDescriptors())
                {
                    Enum.TryParse(entry.Key, out LFType type);

                    _features.Add(type, new (type, entry.Value));
                }
            }
        }
    }
}
