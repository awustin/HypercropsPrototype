using System;
using System.Collections.Generic;
using UnityEngine;

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
