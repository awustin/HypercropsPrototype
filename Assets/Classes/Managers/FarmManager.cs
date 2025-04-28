using System.Collections.Generic;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    public Dictionary<string, GameObject> CropsPlanted = new();
    [HideInInspector] public GameObject CurrentCrop;
    [HideInInspector] public ObjectFactory Factory;

    void Awake()
    {
        name = "FarmManager";
        Factory = ObjectFactory.Instance;
    }

    public void StartCrop(string cropName, Vector3 position)
    {
        // TODO: Redefine key for crops. Does it make sense to keep a dictionary?
        // I could just use the children list and use a custom Equals method in Crop to find the one I need
        string key = FarmUtils.PositionToKey(position);

        if (!CropsPlanted.ContainsKey(key))
        {
            CurrentCrop = Factory.MakeCrop(position, transform);
            Crop cropScript = CurrentCrop.GetComponent<Crop>();

            cropScript.CropName = cropName;
            CurrentCrop.SetActive(true);
            CropsPlanted.Add(key, CurrentCrop);
        }
        else
        {
            Debug.LogWarning($"There is another crop at position {position}!");
        }
    }

    public void DiscardCurrentCrop()
    {
        Destroy(CurrentCrop);
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