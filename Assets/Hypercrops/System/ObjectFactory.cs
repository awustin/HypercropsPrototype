using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

using Assets.Hypercrops.Common.Enums;
using Assets.Hypercrops.System.CommonSerializable;
using Assets.Hypercrops.Model.Crops;
using Assets.Hypercrops.Model.Cards;

// TODO: Refactor all Make methods, so that there's only ONE HypercropsInstance method that creates a given object
// TODO: After that, separate Make methods and put them into their corresponding Model entity. That way this assembly won't depend on Model (avoid circular deps)
// TODO: When a card is made, instead of modifying the prefab, I can just create a prefab for each card.
namespace Assets.Hypercrops.System
{
    public class ObjectFactory : MonoBehaviour
    {
        private static ObjectFactory _instance;
        public static ObjectFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<ObjectFactory>();

                    if (_instance == null)
                    {
                        GameObject singletonObject = new("ObjectFactory");
                        _instance = singletonObject.AddComponent<ObjectFactory>();
                    }
                }

                return _instance;
            }
        }
        public DataLoader Loader;

        [Header("Paths realtive to Assets/Resources")]
        public readonly static string BASE_PREFAB_PATH = "Prefabs";
        public readonly static string CARDS_PREFABS_PATH = $"{BASE_PREFAB_PATH}/Cards";
        public readonly static string CROP_PREFABS_PATH = $"{BASE_PREFAB_PATH}/Crop";
        public readonly static string SPECIES_PREFABS_PATH = $"{BASE_PREFAB_PATH}/Crop/Species";
        public readonly static string FARMING_METHOD_PREFABS_PATH = $"{BASE_PREFAB_PATH}/Crop/FarmingMethod";
        public readonly static string BUILDABLE_PREFABS_PATH = $"{BASE_PREFAB_PATH}/Buildable/Units";
        public readonly static string BUILDABLE_GHOST_PREFABS_PATH = $"{BASE_PREFAB_PATH}/Buildable/Layouts";

        private ObjectCache<GameObject> _cropsCache;
        private ObjectCache<GameObject> _cardsCache;
        private ObjectCache<Material> _materialsCache;
        private readonly ObjectCache<GameObject> _buildablesCache = new();
        private readonly ObjectCache<GameObject> _hypercropsPrefabsLookup = new();

        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                Loader = DataLoader.Instance;

                _cropsCache = new();
                _cardsCache = new();
                _materialsCache = new();
            }
        }

        void OnApplicationQuit()
        {
            // Reset prefabs
            List<string> cardPrefabNames = new() { "Crop", "Buildable", "Tech" };

            cardPrefabNames.ForEach(cardName =>
            {
                GameObject cardPrefab = Resources.Load<GameObject>($"{CARDS_PREFABS_PATH}/CardPrefab {cardName}");
                Card CardScript = cardPrefab.GetComponent<Card>();

                CardScript.Reset();
            });
        }

        #nullable enable

        public GameObject? HypercropsInstance<T>(string key, Vector3 pos, Transform? parent)
        {
            return typeof(T).Name switch
            {
                "BuildableGhost" => MakeBuildableGhost(key, new InstanceArguments(pos, parent)),
                "CropGhost" => MakeCropGhost(key, new InstanceArguments(pos, parent)),
                _ => null,
            };
        }

        public GameObject MakeGenericCrop(Vector3 pos, Transform? parent)
        {
            GameObject prefab = _cropsCache
                .Entry("CropNormal")
                .LoadOnMiss
                (
                    () => Resources.Load<GameObject>($"{CROP_PREFABS_PATH}/CropNormal")
                );

            return Instantiate(prefab, pos, Quaternion.identity, parent ? parent : transform);
        }

        public GameObject MakeCrop(CropDescriptor descriptor, Vector3 pos, Transform? parent)
        {
            GameObject prefab = _cropsCache
                .Entry("CropNormal")
                .LoadOnMiss
                (
                    () => Resources.Load<GameObject>($"{CROP_PREFABS_PATH}/CropNormal")
                );

            GameObject instance = Instantiate(prefab, pos, Quaternion.identity, parent ? parent : transform);
            Crop cropScript = instance.GetComponent<Crop>();

            cropScript.Initialise(descriptor.Species, descriptor.FarmingMethod, descriptor.Size);

            instance.name = descriptor.Species.ToString();
            instance.SetActive(true);

            return instance;
        }

        public GameObject? MakeCropPhaseForSpecies(CropSpecies species, Vector3 pos, Transform? parent)
        {
            CropDescriptor cropDescriptor = GetCropDescriptorBySpeciesName(species.ToString());
            string key = $"{species}:Ready"; // Wheat:Ready

            GameObject prefab = _cropsCache
                .Entry(key)
                .LoadOnMiss(
                    () => Resources.Load<GameObject>($"{SPECIES_PREFABS_PATH}/{species}/Prefab")
                );
            
            // TODO: Debug with and without this method call
            AddSharedMaterials(
                species.ToString(),
                prefab,
                cropDescriptor.GetMaterials(),
                $"{SPECIES_PREFABS_PATH}/{species}/Materials"
            );

            GameObject instance = Instantiate(prefab, pos, Quaternion.identity, parent ? parent : transform);

            if (instance != null)
            {
                instance.name = key;
            }

            return instance;
        }

        public GameObject? MakeCropPhaseForFarmingMethod(CropFarmingMethod farmingMethod, CropPhase phase, Vector3 pos, Transform? parent)
        {
            string key = $"{farmingMethod}:{phase}";
            string loadPath = $"{FARMING_METHOD_PREFABS_PATH}/{farmingMethod}/{phase}/Prefab";

            GameObject prefab = _cropsCache
                .Entry(key)
                .LoadOnMiss
                (
                    () => Resources.Load<GameObject>(loadPath)
                );
            
            GameObject instance = Instantiate(prefab, pos, Quaternion.identity, parent ? parent : transform);

            if (instance != null)
            {
                instance.name = key;
            }

            return instance;
        }

        public GameObject MakeCard(int id, Transform? parent)
        {
            CardDescriptor cardDescriptor = GetCardDescriptorById(id);
            GameObject prefab = _cardsCache
                .Entry(id.ToString())
                .LoadOnMiss
                (
                    () =>
                    {
                        GameObject loaded = Resources.Load<GameObject>($"{CARDS_PREFABS_PATH}/CardPrefab {cardDescriptor.Type}");
                        Card CardScript = loaded.GetComponent<Card>();

                        CardScript.enabled = true;
                        CardScript.InitialiseCard(
                            cardDescriptor.Id,
                            cardDescriptor.Type,
                            cardDescriptor.Name,
                            cardDescriptor.Label,
                            cardDescriptor.Attribute
                        );

                        return loaded;
                    }
                );
            GameObject instance = Instantiate(prefab, parent ? parent : transform);
            instance.name = instance.GetComponent<Card>().CardName;

            return instance;
        }

        public GameObject? MakeBuildable(BuildableDescriptor descriptor, Vector3 pos, Transform? parent)
        {
            BuildableType type = descriptor.Type;

            GameObject prefab = _buildablesCache
                .Entry(type.ToString())
                .LoadOnMiss
                (
                    () => Resources.Load<GameObject>($"{BUILDABLE_PREFABS_PATH}/{type}/Main")
                );
            
            GameObject instance = Instantiate(prefab, pos, Quaternion.identity, parent ? parent : transform);
            // TODO: Actions on load. Initialise script

            if (instance != null)
            {
                instance.name = type.ToString();
            }

            return instance;
        }

        public CropDescriptor GetCropDescriptorBySpeciesName(string speciesName)
        {
            return Loader.LoadCropDescriptor(speciesName);
        }

        public BuildableDescriptor GetBuildableDescriptorByType(string type)
        {
            return Loader.LoadBuildableDescriptor(type);
        }

        public CardDescriptor GetCardDescriptorById(int cardId)
        {
            return Loader.LoadCardDescriptor(cardId);
        }

        private GameObject MakeBuildableGhost(string layoutType, InstanceArguments args)
        {
            GameObject prefab = _hypercropsPrefabsLookup
                .Entry($"BuildableGhost:{layoutType}")
                .LoadOnMiss
                (
                    () => Resources.Load<GameObject>($"{BUILDABLE_GHOST_PREFABS_PATH}/{layoutType}/Ghost")
                );

            return Instantiate(prefab, args.Position, Quaternion.identity, args.Parent);
        }

        private GameObject MakeCropGhost(string size, InstanceArguments args)
        {
            GameObject prefab = _hypercropsPrefabsLookup
                .Entry($"CropGhost:{size}")
                .LoadOnMiss
                (
                    () => Resources.Load<GameObject>($"{CROP_PREFABS_PATH}/Ghosts/CropGhost{size}")
                );

            return Instantiate(prefab, args.Position, Quaternion.identity, args.Parent);
        }
        private void AddSharedMaterials(string parentKey, GameObject prefab, List<string> materials, string loadPath)
        {
            // Materials are found in the path provided
            if (materials.Count == 0)
            {
                return;
            }

            MeshRenderer renderer = prefab.GetComponent<MeshRenderer>();
            List<Material> materialsToAdd = new();

            foreach (string materialName in materials)
            {
                Material mat = _materialsCache
                    .Entry($"{parentKey}:{materialName}")
                    .LoadOnMiss
                    (
                        () => Resources.Load<Material>($"{loadPath}/{materialName}")
                    );

                materialsToAdd.Add(mat);
            }

            renderer.sharedMaterials = materialsToAdd.ToArray();
            renderer.shadowCastingMode = ShadowCastingMode.Off;
        }
    }

    public struct InstanceArguments
    {
        public Vector3 Position;
        public Transform? Parent;

        public InstanceArguments(Vector3 position, Transform? parent)
        {
            Position = position;
            Parent = parent ? parent : ObjectFactory.Instance.transform;
        }
    }
}