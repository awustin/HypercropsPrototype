using System;

[Serializable]
public class CardDescriptor
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
