
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using System;
using Assets.Classes.System;

// TODO: Add MaterialsLoaded to ObjectCache
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

    [HideInInspector] public DataLoader Loader;
    public Dictionary<string, Material> MaterialsLoaded = new();

    private ObjectCache _cropsCache;
    private ObjectCache _cardsCache;

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
        }
    }

    void OnApplicationQuit()
    {
        // Reset prefabs
        List<string> cardPrefabNames = new() { "Crop", "Infrastructure", "Tech" };

        cardPrefabNames.ForEach(cardName =>
        {
            GameObject cardPrefab = Resources.Load<GameObject>($"Prefabs/Cards/CardPrefab {cardName}");
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
                () => Resources.Load<GameObject>($"Prefabs/Crop/Ghosts/CropGhost{size}")
            );

        return Instantiate(prefab, pos, Quaternion.identity, parent ? parent : transform);
    }

    public GameObject MakeGenericCrop(Vector3 pos, Transform? parent)
    {
        GameObject prefab = _cropsCache
            .Entry("CropNormal")
            .LoadOnMiss
            (
                () => Resources.Load<GameObject>($"Prefabs/Crop/CropNormal")
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
                () => Resources.Load<GameObject>($"Prefabs/Crop/Species/{species}/{cropPhase}")
            );
        
        // TODO: Debug with and without this method call
        AddSharedMaterials(
            prefab,
            cropDescriptor.GetMaterials(cropPhase),
            $"Prefabs/Crop/Species/{species}/Materials"
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
                    GameObject loaded = Resources.Load<GameObject>($"Prefabs/Cards/CardPrefab {cardDescriptor.type}");
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

    private void AddSharedMaterials(GameObject prefab, List<string> materials, string loadPath)
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
            if (!MaterialsLoaded.ContainsKey(materialName))
            {
                Material loaded = Resources.Load<Material>($"{loadPath}/" + materialName);
                MaterialsLoaded.Add(materialName, loaded);
            }

            materialsToAdd.Add(MaterialsLoaded[materialName]);
        }

        renderer.sharedMaterials = materialsToAdd.ToArray();
        renderer.shadowCastingMode = ShadowCastingMode.Off;
    }
}
