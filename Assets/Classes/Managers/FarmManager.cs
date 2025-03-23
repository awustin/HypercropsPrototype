using System.Collections.Generic;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    public Dictionary<string, GameObject> CropsPlanted = new Dictionary<string, GameObject>();
    [HideInInspector] public GameObject SingleCrop;
    [HideInInspector] public ObjectFactory Factory;

    void Awake()
    {
        name = "FarmManager";
        Factory = ObjectFactory.Instance;
    }

    public void InstantiateFromGhost(string cropName, Vector3 point)
    {
        SingleCrop = Factory.MakeCrop(point, transform);
        Crop cropScript = SingleCrop.GetComponent<Crop>();

        cropScript.Name = cropName;

        SingleCrop.SetActive(true);
    }

    public void PlaceAndStartSeed(Vector3 position)
    {
        string key = FarmUtils.PositionToKey(position);

        if (!CropsPlanted.ContainsKey(key))
        {
            SingleCrop.GetComponent<Crop>().IncrementCropPhase(position);
            CropsPlanted.Add(key, SingleCrop);
        }
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

    public void RemoveCrop(string key)
    {
        if (CropsPlanted.ContainsKey(key))
        {
            GameObject crop = CropsPlanted[key];

            CropsPlanted.Remove(key);
            Destroy(crop);
        }
    }

    public bool IsPlantInPosition(string position)
    {
        return CropsPlanted.ContainsKey(position);
    }

    public void SetPosition(Vector3 position)
    {
        if (SingleCrop != null)
        {
            SingleCrop.transform.position = position;
        }
    }

    public void TrySetGhostAllowed(bool value)
    {
        if (SingleCrop == null)
        {
            return;
        }

        SingleCrop.GetComponent<Crop>()?.SetAllowed(value);
    }
}