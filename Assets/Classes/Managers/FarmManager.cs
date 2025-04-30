using System.Collections.Generic;
using UnityEngine;
using Assets.Classes.System;

public class FarmManager : MonoBehaviour
{
    private static FarmManager _instance;
    public static FarmManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<FarmManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new("FarmManager");
                    _instance = singletonObject.AddComponent<FarmManager>();
                }
            }

            return _instance;
        }
    }

    public Dictionary<string, GameObject> CropsPlanted = new();
    [HideInInspector] public GameObject CurrentCrop;
    [HideInInspector] public ObjectFactory Factory;

    private ObjectCache<GameObject> _plantedCache = new();

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            name = "FarmManager";
            Factory = ObjectFactory.Instance;
        }
    }

    public void StartCrop(string cropName, Vector3 position)
    {
        // TODO: Redefine key for crops. Does it make sense to keep a dictionary?
        // I could just use the children list and use a custom Equals method in Crop to find the one I need
        string key = FarmUtils.PositionToKey(position);

        if (!CropsPlanted.ContainsKey(key))
        {
            CurrentCrop = Factory.MakeGenericCrop(position, transform);
            Crop cropScript = CurrentCrop.GetComponent<Crop>();

            CurrentCrop.name = cropName;
            CurrentCrop.SetActive(true);
            cropScript.CropName = cropName;
            CropsPlanted.Add(key, CurrentCrop);
        }
        else
        {
            Debug.LogWarning($"There is another crop at position {position}!");
        }
    }

    public void DiscardCurrentCrop()
    {
        CurrentCrop = null;
    }

    public GameObject GetCrop(string key)
    {
        if (CropsPlanted.ContainsKey(key))
        {
            return CropsPlanted[key];
        }
        else
        {
            Debug.LogWarning($"Key '{key}' not found in the dictionary.");
            return null;
        }
    }

    public bool IsPlantInPosition(string position)
    {
        return CropsPlanted.ContainsKey(position);
    }

    public void KillCrop(GameObject cropTarget)
    {
        string key = FarmUtils.PositionToKey(cropTarget.transform.position);

        if (CropsPlanted.ContainsKey(key))
        {
            GameObject crop = CropsPlanted[key];

            CropsPlanted.Remove(key);
            Destroy(crop);
        }
    }
}