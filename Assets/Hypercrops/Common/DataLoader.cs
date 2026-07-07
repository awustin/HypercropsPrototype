using UnityEngine;
using System.Collections.Generic; 

using Assets.Hypercrops.Common.Serializables;
using Assets.Hypercrops.Utils;

namespace Assets.Hypercrops.Common
{
    public class DataLoader : MonoBehaviour
    {
        private static DataLoader _instance;
        public static DataLoader Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<DataLoader>();

                    if (_instance == null)
                    {
                        GameObject singletonObject = new("DataLoader");
                        _instance = singletonObject.AddComponent<DataLoader>();
                    }
                }

                return _instance;
            }
        }

        public string CropDescriptorsPath = "/Data/Crops/Descriptors";
        public string BuildableDescriptorsPath = "/Data/Buildables/Descriptors";
        public string CardDescriptorsPath = "/Data/Cards/Descriptors";
        public string InitialDeckPath = "/Data/InitialDeck.json";
        public string LevelFeaturesPath = "/Data/LevelFeatures/InitialLevelFeatures.json";

        private readonly ObjectCache<CropDescriptor> _cropDescriptorsCache = new();
        private readonly ObjectCache<BuildableDescriptor> _buildableDescriptorsCache = new();
        private readonly ObjectCache<CardDescriptor> _cardDescriptorCache = new();
        private DeckData _initialDeckData = new();
        private Dictionary<string, bool> _levelFeatures = new();

        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        public CropDescriptor LoadCropDescriptor(string speciesName)
        {
            return _cropDescriptorsCache
                .Entry(speciesName)
                .LoadOnMiss
                (
                    () => FileUtils.ReadAssetJSON<CropDescriptor>($"{CropDescriptorsPath}/{speciesName}.json")
                );
        }

        public BuildableDescriptor LoadBuildableDescriptor(string buildingType)
        {
            return _buildableDescriptorsCache
                .Entry(buildingType)
                .LoadOnMiss
                (
                    () => FileUtils.ReadAssetJSON<BuildableDescriptor>($"{BuildableDescriptorsPath}/{buildingType}.json")
                );
        }

        public CardDescriptor LoadCardDescriptor(int cardId)
        {
            return _cardDescriptorCache
                .Entry(cardId.ToString())
                .LoadOnMiss
                (
                    () => FileUtils.ReadAssetJSON<CardDescriptor>($"{CardDescriptorsPath}/{cardId}.json")
                );
        }

        public List<CardWeight> GetInitialCardWeights()
        {
            if (!_initialDeckData.IsLoaded)
            {
                _initialDeckData = FileUtils.ReadAssetJSON<DeckData>(InitialDeckPath);
                _initialDeckData.IsLoaded = true;
            }

            return _initialDeckData.cardWeights;
        }

        public Dictionary<string, bool> LoadLevelFeatureDescriptors()
        {
            if (_levelFeatures.Count == 0)
            {
                _levelFeatures =
                    FileUtils.ReadAssetJSON<Dictionary<string, bool>>(LevelFeaturesPath);
            }

            return _levelFeatures;
        }
    }
}
