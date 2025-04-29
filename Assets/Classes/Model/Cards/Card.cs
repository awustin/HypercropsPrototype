using System;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour, IEquatable<Card>
{
    public string Label;
    public string CardName;
    public int Id;
    public int DeckId;
    public CardType Type;
    public CardStatus Status;
    public int Order;
    public int Number;

    #nullable enable
    public string? PrefabName;
    #nullable disable

    void Start()
    {
        RectTransform CanvasRT = transform.Find("CardCanvas").GetComponent<RectTransform>();

        CanvasRT.localScale = Vector3.one;
        CanvasRT.sizeDelta = new Vector2(150, 200);

        // TODO: Card has to be drag and drop
    }

    public void InitialiseCard(
        int id,
        CardType type,
        string cardName,
        string label,
        string prefabName
    )
    {
        CardName = $"Card:{cardName}";
        Id = id;
        Type = type;
        Label = label;
        PrefabName = prefabName;
        Number = Id;
        Status = CardStatus.FaceDown;

        SetName();
        SetNumber();
    }

    public void TurnUpInHand(int order)
    {
        Status = CardStatus.TurnUp;
        SetOrder(order);
        Status = CardStatus.Idle;
    }

    public void ChangePositionInHand(int order)
    {
        // TODO: animate and set idle on completion
    }

    public void SetDeckId(int value)
    {
        DeckId = value;
    }

    public void SetOrder(int value)
    {
        // TODO: animate
        Order = value;
        transform.localPosition = DeckUtils.MapOrderToPosition(Order);
    }

    public bool Equals(Card other)
    {
        return DeckId == other.DeckId;
    }

    public void Reset()
    {
        enabled = false;
        name = null;
        Id = 0;
        Label = null;
        PrefabName = null;
        Number = 0;
        Status = CardStatus.FaceDown;
        CardName = null;
    }

    private void SetName()
    {
        TMP_Text NameLabel = transform.Find("CardCanvas/Traits/Name/Label").gameObject.GetComponent<TMP_Text>();

        NameLabel.text = Label;
    }

    private void SetNumber()
    {
        TMP_Text NumberLabel = transform.Find("CardCanvas/Traits/Number/Label").gameObject.GetComponent<TMP_Text>();

        NumberLabel.text = Number.ToString();
    }
}
