
using System;
using System.Collections.Generic;

[Serializable]
public class CropDescriptor
{
    public Species Species;
    public CropPhaseDescriptor Seed;
    public CropPhaseDescriptor Growing;
    public CropPhaseDescriptor Ready;
    public CropPhaseDescriptor Dead;

    public List<string> GetMaterials(CropPhase cropPhase)
    {
        return GetPhaseDescriptor(cropPhase).materials;
    }

    public override string ToString()
    {
        return $"Crop descriptor for {Species}. Seed {Seed}, Growing {Growing}, Ready {Ready}, Dead {Dead}";
    }

    private CropPhaseDescriptor GetPhaseDescriptor(CropPhase copPhase)
    {
        return copPhase switch
        {
            CropPhase.Seed => Seed,
            CropPhase.Growing => Growing,
            CropPhase.Ready => Ready,
            CropPhase.Dead => Dead,
            _ => new CropPhaseDescriptor(),
        };
    }
}

[Serializable]
public class CropPhaseDescriptor
{
    public string meshCollider;
    public List<string> materials;

    public override string ToString()
    {
        return $"Materials loaded: {materials.Count}";
    }
}
