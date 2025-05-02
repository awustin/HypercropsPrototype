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

    [HideInInspector] public GameObject CurrentCrop;
    [HideInInspector] public ObjectFactory Factory;

    private readonly ObjectCache<GameObject> _plantedCache = new();

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
            DebugCropGrid(10, 10, "Wheat");
        }
    }

    public void StartCrop(string cropName, Vector3 position)
    {
        // TODO: Redefine key for crops. Does it make sense to keep a dictionary?
        // I could just use the children list and use a custom Equals method in Crop to find the one I need
        string key = FarmUtils.PositionToKey(position);

        GameObject currentCrop = _plantedCache
            .Entry(key)
            .LoadOnMiss
            (
                () =>
                {
                    GameObject created = Factory.MakeGenericCrop(position, transform);

                    created.name = cropName;
                    created.GetComponent<Crop>().CropName = cropName;
                    created.SetActive(true);

                    return created;
                }
            );

        CurrentCrop = currentCrop;
    }

    public void DiscardCurrentCrop()
    {
        CurrentCrop = null;
    }

    public bool IsPlantInPosition(Vector3 position)
    {
        return _plantedCache.Entry(FarmUtils.PositionToKey(position)).IsHit;
    }

    public void KillCrop(GameObject cropTarget)
    {
        string key = FarmUtils.PositionToKey(cropTarget.transform.position);

        GameObject removed = _plantedCache.Entry(key).Delete();
        Destroy(removed);
    }

    private void DebugCropGrid(int rows, int columns, string cropName)
    {
        float gridSpacing = 3f;
        Vector3 offset = new(-2f, 0f, -2f);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 gridPosition = new(-col * gridSpacing, 0f, -row * gridSpacing);
                Vector3 centeredPosition = gridPosition;

                centeredPosition.x += gridSpacing / 2f - (gridSpacing * 0.5f);
                centeredPosition.z += gridSpacing / 2f - (gridSpacing * 0.5f);
                centeredPosition += offset;

                StartCrop(cropName, centeredPosition);
            }
        }
    }
}