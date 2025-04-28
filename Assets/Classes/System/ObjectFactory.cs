
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using System;

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
    public Dictionary<string, GameObject> ObjectsLoaded = new();
    public Dictionary<string, Material> MaterialsLoaded = new();
    public Dictionary<int, GameObject> CardsLoaded = new();

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
        }
    }

#nullable enable

    // *** Public methods
    public GameObject? MakeCropGhost(Vector3 pos, CropSize? size, Transform? parent)
    {
        CreateUniqueObject($"CropGhost{size}", $"Prefabs/Crop/Ghosts/CropGhost{size}");

        return MakeInstance($"CropGhost{size}", pos, parent);
    }

    public GameObject? MakeGenericCrop(Vector3 pos, Transform? parent)
    {
        CreateUniqueObject("CropNormal", $"Prefabs/Crop/CropNormal");

        return MakeInstance("CropNormal", pos, parent);
    }

    public GameObject? MakeCropPhase(string cropName, CropPhase cropPhase, Vector3 pos, Transform? parent)
    {
        CropDescriptor cropDescriptor = Loader.GetCropDescriptor(cropName);
        Enum.TryParse(cropName, out Species species);

        if (cropDescriptor == null)
        {
            return null;
        }

        MakePrefabArguments args = new(
            $"{species}:{cropPhase}", // Wheat:Seed
            cropPhase.ToString(), // Seed
            $"Prefabs/Crop/Species/{species}/{cropPhase}", // Prefabs/Crop/Species/Wheat/Seed
            pos,
            cropDescriptor.GetMaterials(cropPhase),
            parent
        );
        
        return MakePrefab(args);
    }

    public GameObject? MakeCard(int id, Transform? parent)
    {
        CreateUniqueCard(id);

        return MakeCardInstance(id, parent);
    }

    // *** Private methods
    private GameObject? MakePrefab(MakePrefabArguments args)
    {
        CreateUniqueObject(args.Key, args.Path);
        AddSharedMaterials(args.Key, args.Materials);

        GameObject? instanced = MakeInstance(args.Key, args.Position, args.Parent);

        if (instanced != null)
        {
            instanced.name = args.Key;
        }

        return instanced;
    }

    private void CreateUniqueObject(string key, string loadPath)
    {
        if (ObjectsLoaded.ContainsKey(key))
        {
            return;
        }

        GameObject created = Resources.Load<GameObject>(loadPath);
        ObjectsLoaded.Add(key, created);
    }

    private void AddSharedMaterials(string key, List<string> materials)
    {
        if (materials.Count == 0 || !ObjectsLoaded.ContainsKey(key))
        {
            return;
        }

        GameObject baseObject = ObjectsLoaded[key];
        MeshRenderer renderer = baseObject.GetComponent<MeshRenderer>();
        List<Material> materialsToAdd = new();

        foreach (string materialName in materials)
        {
            if (!MaterialsLoaded.ContainsKey(materialName))
            {
                Material loaded = Resources.Load<Material>("Materials/" + materialName);
                MaterialsLoaded.Add(materialName, loaded);
            }

            materialsToAdd.Add(MaterialsLoaded[materialName]);
        }

        renderer.sharedMaterials = materialsToAdd.ToArray();
        renderer.shadowCastingMode = ShadowCastingMode.Off;
    }

    private GameObject? MakeInstance(string key, Vector3 position, Transform? parent)
    {
        if (!ObjectsLoaded.ContainsKey(key))
        {
            return null;
        }

        GameObject prefab = ObjectsLoaded[key];

        return Instantiate(prefab, position, Quaternion.identity, parent ? parent : transform);
    }

    private void CreateUniqueCard(int id)
    {
        if (CardsLoaded.ContainsKey(id))
        {
            return;
        }

        CardData cardData = Loader.GetCardData(id);
        GameObject cardPrefab = Resources.Load<GameObject>("Prefabs/Cards/CardPrefab " + cardData.type.ToString());
        Card CardScript = cardPrefab.GetComponent<Card>();

        CardScript.enabled = true;
        CardScript.InitialiseCard(cardData);
        CardsLoaded.Add(id, cardPrefab);
    }

    private GameObject? MakeCardInstance(int id, Transform? parent)
    {
        if (!CardsLoaded.ContainsKey(id))
        {
            return null;
        }

        GameObject prefab = CardsLoaded[id];

        return Instantiate(prefab, parent ? parent : transform);
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

    protected struct MakePrefabArguments
    {
        public string Key;
        public string Name;
        public string Path;
        public Vector3 Position;
        public List<string> Materials;
        public Transform? Parent;
    
        public MakePrefabArguments(
            string key,
            string name,
            string path,
            Vector3 position,
            List<string> materials,
            Transform? parent
        )
        {
            Key = key;
            Name = name;
            Path = path;
            Position = position;
            Materials = materials;
            Parent = parent;
        }
    }
}
