using System;
using System.Collections.Generic;
using UnityEngine;

using Assets.Hypercrops.Common;
using LFType = Assets.Hypercrops.Common.Enums.LevelFeatureType;
using Assets.Hypercrops.State;
using Assets.Hypercrops.Events;

namespace Assets.Hypercrops.Model.LevelFeatures
{
    public class LevelFeaturesManager : MonoBehaviour
    {
        private static LevelFeaturesManager _instance;
        public static LevelFeaturesManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<LevelFeaturesManager>();

                    if (_instance == null)
                    {
                        GameObject singletonObject = new("LevelFeaturesManager");
                        _instance = singletonObject.AddComponent<LevelFeaturesManager>();
                    }
                }

                return _instance;
            }
        }
        public GameEventSender Sender;
        public GameState State;
        public DataLoader Loader;
        private readonly Dictionary<LFType, LevelFeature> _features = new ();

        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                name = "LevelFeaturesManager";
                Loader = DataLoader.Instance;
                State = GameState.Instance;

                Initialise();
            }
        }

        void OnEnable()
        {
            Sender = GameEventSender.Instance;

            Sender.EnableFeature += OnEnableFeature;
        }

        void OnDisable()
        {
            Sender.EnableFeature -= OnEnableFeature;
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

        private void OnEnableFeature(object sender, EnableFeatureArguments args)
        {
            Enum.TryParse(args.FeatureType, out LFType type);

            if (!_features.ContainsKey(type))
            {
                _features.Add(type, new (type, true));
                UpdateGameState(type, true);
            }
            else if (!_features[type].IsEnabled)
            {
                _features[type].IsEnabled = true;
                UpdateGameState(type, true);
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
