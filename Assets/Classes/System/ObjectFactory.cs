
using UnityEngine;
using UnityEngine.Rendering;
using System;
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
                    GameObject singletonObject = new GameObject("ObjectFactory");
                    _instance = singletonObject.AddComponent<ObjectFactory>();
                }
            }

            return _instance;
        }
    }

    [HideInInspector] public DataLoader Loader;
    public Dictionary<string, GameObject> ObjectsLoaded = new Dictionary<string, GameObject>();
    public Dictionary<string, Material> MaterialsLoaded = new Dictionary<string, Material>();
    
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

    public GameObject? MakeCrop(Vector3 pos, Transform? parent)
    {
        CreateUniqueObject("Crop");

        return MakeInstance("Crop", pos, parent);
    }

    public void SetGhostMaterial(GameObject gameObject, bool isGreen)
    {
        MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();

        if (renderer.material == null)
        {
            Debug.Log("no material");
            return;
        }

        string colorHex = isGreen ?  "#00FF65" : "#FF5000";

        if (ColorUtility.TryParseHtmlString(colorHex, out Color color))
        {
            color.a = 0.3f;
            renderer.material.color = color;
        }
    }

    public GameObject? MakeCropPhase(string cropName, string cropStage, Vector3 pos, Transform? parent)
    {
        CropData crop = Loader.GetCropData(cropName);
        CropStageData stage = crop.GetStage(cropStage);

        if (stage == null)
        {
            return null;
        }

        List<string> materialsToAdd = (cropStage == "Ghost") ? new List<string>() { "MaterialGhost" } : stage.materials;

        return Make(stage.name, pos, materialsToAdd, parent);
    }

    private GameObject? Make(string name, Vector3 position, List<string> materials, Transform? parent)
    {
        CreateUniqueObject(name);
        AddSharedMaterials(name, materials);

        return MakeInstance(name, position, parent);
    }

    private void CreateUniqueObject(string name)
    {
        if (ObjectsLoaded.ContainsKey(name))
        {
            return;
        }

        GameObject created = Resources.Load<GameObject>("Prefabs/" + name);
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
        List<Material> materialsToAdd = new List<Material>();

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

    #nullable disable
}
