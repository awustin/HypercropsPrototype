using UnityEngine;
using System;

using Assets.Hypercrops.Common.Serializables;

namespace Assets.Hypercrops.Events
{
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
        public CropDescriptor Descriptor { get; set; }

        public StartFarmModeArguments(Vector3 point, CropDescriptor cropDescriptor)
        {
            Point = point;
            Descriptor = cropDescriptor;
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

    public class StartBuildModeArguments : EventArgs
    {
        public BuildableDescriptor Descriptor { get; set; }

        public StartBuildModeArguments(BuildableDescriptor buildableDescriptor)
        {
            Descriptor = buildableDescriptor;
        }
    }
}
