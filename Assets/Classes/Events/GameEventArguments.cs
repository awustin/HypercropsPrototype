using UnityEngine;
using System;

using Assets.Classes.System.Common;

public class WalkEventArguments : EventArgs
{
    public Vector3 Target { get; set; }


    public WalkEventArguments(Vector3 target)
    {
        Target = target;
    }
}

public class StartFarmModeArguments : EventArgs
{
    public Vector3 Point { get; set; }
    public CropDescriptor CropDescripor { get; set; }

    public StartFarmModeArguments(Vector3 point, CropDescriptor cropDescriptor)
    {
        Point = point;
        CropDescripor = cropDescriptor;
    }
}

public class CropDeathArguments : EventArgs
{
    public GameObject Crop { get; set; }

    public CropDeathArguments(GameObject crop)
    {
        Crop = crop;
    }
}