
using System;
using System.Collections.Generic;
using UnityEngine;

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

[Serializable]
public class CardData
{
    public int id;
    public CardType type;
    public string name;
    public string label;
    #nullable enable
    public string? prefabName;
    #nullable disable

    public override string ToString()
    {
        return name;
    }
}

[Serializable]
public class DeckData
{
    public List<CardWeight> cardWeights;
    public bool IsLoaded = false;

    public override string ToString()
    {
        string msg = "";

        foreach (CardWeight cardWeight in cardWeights)
        {
            msg = $"{msg}; {cardWeight}";
        }

        return msg;
    }
}

[Serializable]
public class CardWeight
{
    public int id;

    [Range(0f, 1f)]
    public float weight;

    public override string ToString()
    {
        return $"{id}-${weight}";
    }
}
