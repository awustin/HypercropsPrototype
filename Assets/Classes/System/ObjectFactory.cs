
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
        // TODO: Create class ObjectCache to handle loaded objects
        // TODO: Separate AddSharedMaterials from MakePrefab
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
            $"Prefabs/Crop/Species/{species}/Materials",
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
        AddSharedMaterials(args.Key, args.Materials, args.MaterialsPath);

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

    // TOOD: Do I REALLY need to do this?
    private void AddSharedMaterials(string key, List<string> materials, string loadPath)
    {
        // Materials are found in the path provided
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
                Material loaded = Resources.Load<Material>($"{loadPath}/" + materialName);
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

        return Instantiate(ObjectsLoaded[key], position, Quaternion.identity, parent ? parent : transform);
    }

    private void CreateUniqueCard(int id)
    {
        if (CardsLoaded.ContainsKey(id))
        {
            return;
        }

        CardDescriptor cardData = Loader.GetCardDescriptor(id);
        GameObject cardPrefab = Resources.Load<GameObject>("Prefabs/Cards/CardPrefab " + cardData.type.ToString());
        Card CardScript = cardPrefab.GetComponent<Card>();

        CardScript.enabled = true;
        CardScript.InitialiseCard(
            cardData.id,
            cardData.type,
            cardData.name,
            cardData.label,
            cardData.prefabName
        );

        CardsLoaded.Add(id, cardPrefab);
    }

    private GameObject? MakeCardInstance(int id, Transform? parent)
    {
        if (!CardsLoaded.ContainsKey(id))
        {
            return null;
        }

        GameObject? instance = Instantiate(CardsLoaded[id], parent ? parent : transform);

        if (instance != null)
        {
            instance.name = instance.GetComponent<Card>().CardName;
        }

        return instance;
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

    private struct MakePrefabArguments
    {
        public string Key;
        public string Name;
        public string Path;
        public Vector3 Position;
        public List<string> Materials;
        public string MaterialsPath;
        public Transform? Parent;
    
        public MakePrefabArguments(
            string key,
            string name,
            string path,
            Vector3 position,
            List<string> materials,
            string materialsPath,
            Transform? parent
        )
        {
            Key = key;
            Name = name;
            Path = path;
            Position = position;
            Materials = materials;
            MaterialsPath = materialsPath;
            Parent = parent;
        }
    }
}
