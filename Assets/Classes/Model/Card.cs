using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    public string label;
    public int id;
    public CardType cardType;
    public CardStatus cardStatus;
    public int order;
    public int number;

    public Card(string name, string label, int id, CardType type, int order, int number)
    {
        this.name = name;
        this.label = label;
        this.id = id;
        cardType = type;
        this.order = order;
        this.number = number;
    }

    void Start()
    {
        cardStatus = CardStatus.FaceDown;
        transform.localPosition = DeckUtils.MapOrderToPosition(order);
        RectTransform CanvasRT = transform.Find("CardCanvas").GetComponent<RectTransform>();

        CanvasRT.localScale = Vector3.one;
        CanvasRT.sizeDelta = new Vector2(150, 200);

        SetName();
        SetNumber();
    }

    public void TurnUpInHand(Vector3 position)
    {
        cardStatus = CardStatus.TurnUp;
        // TODO: animate and set idle on completion
    }

    public void ChangePositionInHand(Vector3 position)
    {
        // TODO: animate and set idle on completion
    }

    private void SetName()
    {
        TMP_Text NameLabel = transform.Find("CardCanvas/Traits/Name/Label").gameObject.GetComponent<TMP_Text>();

        NameLabel.text = this.label;
    }

    private void SetNumber()
    {
        TMP_Text NumberLabel = transform.Find("CardCanvas/Traits/Number/Label").gameObject.GetComponent<TMP_Text>();

        NumberLabel.text = number.ToString();
    }
}
