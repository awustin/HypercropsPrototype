using System.Collections.Generic;
using UnityEngine;

public class CropUtils : ScriptableObject
{
    private static CropUtils _instance;

    public static CropUtils Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = CreateInstance<CropUtils>();
            }

            return _instance;
        }
    }

    private readonly Dictionary<CropHealthReductionType, float> _cropHealthReductionTypes = new();

    void Awake()
    {
        name = "Crops Utils Singleton";
        _cropHealthReductionTypes.Clear();
        _cropHealthReductionTypes.Add(CropHealthReductionType.Low, 0.005f);
        _cropHealthReductionTypes.Add(CropHealthReductionType.Normal, 0.01f);
        _cropHealthReductionTypes.Add(CropHealthReductionType.High, 0.02f);
    }

    public float GetCropHealthReduction(CropHealthReductionType type)
    {
        return _cropHealthReductionTypes[type];
    }
}
