using System;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour, IEquatable<Card>
{
    public string Label;
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

    public void InitialiseCard(CardData cardData)
    {
        name = $"Card/{cardData.name}";
        Id = cardData.id;
        Type = cardData.type;
        Label = cardData.label;
        PrefabName = cardData.prefabName;
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
