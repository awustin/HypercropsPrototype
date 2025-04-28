
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

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
            Loader.LoadGameData();
        }
    }

#nullable enable

    // *** Public methods
    public GameObject? MakeCropGhost(Vector3 pos, CropSize? size, Transform? parent)
    {
        CreateUniqueObject($"CropGhost{size}", "Prefabs/Crop/Ghosts/");

        return MakeInstance($"CropGhost{size}", pos, parent);
    }

    public GameObject? MakeCrop(Vector3 pos, Transform? parent)
    {
        CreateUniqueObject("CropNormal", "Prefabs/Crop/");

        return MakeInstance("CropNormal", pos, parent);
    }

    public GameObject? MakeCropPhase(string cropName, string cropStage, Vector3 pos, Transform? parent)
    {
        CropData crop = Loader.GetCropData(cropName);
        CropStageData stage = crop.GetStage(cropStage);

        if (stage == null)
        {
            return null;
        }

        MakePrefabArguments args = new(
            stage.name,
            "Prefabs/Crop/Variants/",
            pos,
            cropStage == "Ghost" ? new List<string>() { "MaterialGhost" } : stage.materials,
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
        CreateUniqueObject(args.Name, args.Path);
        AddSharedMaterials(args.Name, args.Materials);

        return MakeInstance(args.Name, args.Position, args.Parent);
    }

    private void CreateUniqueObject(string name, string folder)
    {
        if (ObjectsLoaded.ContainsKey(name))
        {
            return;
        }

        GameObject created = Resources.Load<GameObject>($"{folder}{name}");
        ObjectsLoaded.Add(name, created);
    }

    private void AddSharedMaterials(string name, List<string> materials)
    {
        if (materials.Count == 0 || !ObjectsLoaded.ContainsKey(name))
        {
            return;
        }

        GameObject baseObject = ObjectsLoaded[name];
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

    private GameObject? MakeInstance(string name, Vector3 position, Transform? parent)
    {
        if (!ObjectsLoaded.ContainsKey(name))
        {
            return null;
        }

        GameObject prefab = ObjectsLoaded[name];

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
        public string Name;
        public string Path;
        public Vector3 Position;
        public List<string> Materials;
        public Transform? Parent;
    
        public MakePrefabArguments(
            string name,
            string path,
            Vector3 position,
            List<string> materials,
            Transform? parent
        )
        {
            Name = name;
            Path = path;
            Position = position;
            Materials = materials;
            Parent = parent;
        }
    }
}
