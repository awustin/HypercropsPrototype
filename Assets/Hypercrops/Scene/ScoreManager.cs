using UnityEngine;

using LF = Assets.Hypercrops.Common.Enums.LevelFeatureType;
using Assets.Hypercrops.Scene.LevelFeatures;
using Assets.Hypercrops.State;
using Assets.Hypercrops.Events;

namespace Assets.Hypercrops.Scene
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance
        {
            get
            {
                Instantiate();
                return _instance;
            }
        }
        public float ScoreProduction;
        public LevelFeaturesManager LevelFeatures;
        public GameState State;
        public GameEventSender Sender;

        private static ScoreManager _instance;

        void Awake()
        {
            if (LevelFeatures == null)
                LevelFeatures = LevelFeaturesManager.Instance;

            if (State == null)
                State = GameState.Instance;
            
            if (Sender == null)
                Sender = GameEventSender.Instance;

            ScoreProduction = 0f;
        }

        public void CollectPoints()
        {
            // TODO: how to collect points?
            // Collect from the manually harvested crops (production value x health)

            if (LevelFeatures.IsEnabled(LF.CollectHarvest))
            {
                // TODO: Calculate BioStorage factor from ALL BioStorages
                // TODO: Aggregate BioStorage points

                Sender.BroadcastEvent("ProduceHarvestPoints");
                // TODO: Aggregate harvest points

                // TODO: harvest * biostorage factor
            }
        }

        private static void Instantiate()
        {
            _instance = _instance != null ? _instance : FindFirstObjectByType<ScoreManager>();

            if (_instance != null)
            {
                return;
            }

            GameObject singletonObject = new("ScoreManager");
            _instance = singletonObject.AddComponent<ScoreManager>();
        }
    }
}