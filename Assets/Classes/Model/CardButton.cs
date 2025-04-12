using UnityEngine;
using UnityEngine.EventSystems;

public class CardButton : UIElementBehaviour, IPointerClickHandler
{
    override public void OnPointerEnter(PointerEventData data)
    {
        base.OnPointerEnter(data);
        // TODO: Animate card
    }

    public void OnPointerClick(PointerEventData data)
    {
        GameObject card = data.pointerPress;
        Card CardScript = card.GetComponent<Card>();
        CardType type = CardScript.cardType;

        State.SetLastSelected(card);

        if (type == CardType.Crop)
        {
            string cropName = CardScript.name;
            GameEventSender.Instance.BroadcastFarmingModeEvent(new Vector3(0, 0, 0), cropName);

            return;
        }

        if (type == CardType.Infrastructure)
        {
            // TODO: Start building mode
            return;
        }
        
        if (type == CardType.Tech)
        {
            // TODO: Apply effect
            return;
        }
    }
}
