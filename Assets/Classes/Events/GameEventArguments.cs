using UnityEngine;
using System;

public class WalkEventArguments : EventArgs
{
    public Vector3 Target { get; set; }


    public WalkEventArguments(Vector3 target)
    {
        Target = target;
    }
}

public class FarmingModeEventArguments : EventArgs
{
    public Vector3 Point { get; set; }
    public string CropName { get; set; }

    public FarmingModeEventArguments(Vector3 point, string cropName)
    {
        Point = point;
        CropName = cropName;
    }
}