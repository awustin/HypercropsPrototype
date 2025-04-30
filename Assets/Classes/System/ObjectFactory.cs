
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using System;

// TODO: When a card is made, instead of modifying the prefab, I can just create a prefab for each card.
namespace Assets.Classes.System
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

        private ObjectCache<GameObject> _cropsCache;
        private ObjectCache<GameObject> _cardsCache;
        private ObjectCache<Material> _materialsCache;

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
                Loader.LoadGameDescriptors();

                _cropsCache = new();
                _cardsCache = new();
                _materialsCache = new();
            }
        }

        void OnApplicationQuit()
        {
            // Reset prefabs
            List<string> cardPrefabNames = new() { "Crop", "Infrastructure", "Tech" };

            cardPrefabNames.ForEach(cardName =>
            {
                GameObject cardPrefab = Resources.Load<GameObject>($"{CARDS_PREFABS_PATH}/CardPrefab {cardName}");
                Card CardScript = cardPrefab.GetComponent<Card>();

                CardScript.Reset();
            });
        }

        #nullable enable
        public GameObject MakeCropGhost(Vector3 pos, CropSize? size, Transform? parent)
        {
            GameObject prefab = _cropsCache
                .Entry($"CropGhost{size}")
                .LoadOnMiss
                (
                    () => Resources.Load<GameObject>($"{CROP_PREFABS_PATH}/Ghosts/CropGhost{size}")
                );

            return Instantiate(prefab, pos, Quaternion.identity, parent ? parent : transform);
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

        public GameObject? MakeCropPhase(string cropName, CropPhase cropPhase, Vector3 pos, Transform? parent)
        {
            CropDescriptor cropDescriptor = Loader.GetCropDescriptor(cropName);

            if (cropDescriptor == null)
            {
                return null;
            }

            Enum.TryParse(cropName, out Species species);
            string key = $"{species}:{cropPhase}"; // Wheat:Seed

            GameObject prefab = _cropsCache
                .Entry(key)
                .LoadOnMiss(
                    () => Resources.Load<GameObject>($"{SPECIES_PREFABS_PATH}/{species}/{cropPhase}")
                );
            
            // TODO: Debug with and without this method call
            AddSharedMaterials(
                species.ToString(),
                prefab,
                cropDescriptor.GetMaterials(cropPhase),
                $"{SPECIES_PREFABS_PATH}/{species}/Materials"
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
            CardDescriptor cardDescriptor = Loader.GetCardDescriptor(id);
            GameObject prefab = _cardsCache
                .Entry(id.ToString())
                .LoadOnMiss
                (
                    () =>
                    {
                        GameObject loaded = Resources.Load<GameObject>($"{CARDS_PREFABS_PATH}/CardPrefab {cardDescriptor.type}");
                        Card CardScript = loaded.GetComponent<Card>();

                        CardScript.enabled = true;
                        CardScript.InitialiseCard(
                            cardDescriptor.id,
                            cardDescriptor.type,
                            cardDescriptor.name,
                            cardDescriptor.label,
                            cardDescriptor.prefabName
                        );

                        return loaded;
                    }
                );
            GameObject instance = Instantiate(prefab, parent ? parent : transform);
            instance.name = instance.GetComponent<Card>().CardName;

            return instance;
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
}