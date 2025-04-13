
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CropData
{
    public CropStageData Ghost;
    public CropStageData Seed;
    public CropStageData Growing;
    public CropStageData Ready;
    public CropStageData Dead;

    public CropStageData GetStage(string stageName)
    {
        return stageName switch
        {
            "Ghost" => Ghost,
            "Seed" => Seed,
            "Growing" => Growing,
            "Ready" => Ready,
            "Dead" => Dead,
            _ => new CropStageData(),
        };
    }

    public override string ToString()
    {
        return Ghost.name + "-" + Seed.name + "-" + Growing.name + "-" + Ready.name + "-" + Dead.name + "";
    }
}

[System.Serializable]
public class CropStageData
{
    public string name;
    public string meshCollider;
    public List<string> materials;
}

[System.Serializable]
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

[System.Serializable]
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

[System.Serializable]
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
