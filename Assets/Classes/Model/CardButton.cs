using UnityEngine;
using UnityEngine.EventSystems;

public class CardButton : UIElementBehaviour, IPointerClickHandler
{
    [HideInInspector] public GameEventSender Sender;

    override public void OnPointerEnter(PointerEventData data)
    {
        base.OnPointerEnter(data);
        // TODO: Animate card
    }

    public void OnPointerClick(PointerEventData data)
    {
        GameObject card = data.pointerPress;
        State.SetLastSelected(card);

        // TODO: Handle card behaviour based on type
        // TODO: Don't use tags or ActionName. Use data packed in GameObjects instead
        // LastSelected = e.Target;

        // if (e.ActionName == "CropCard")
        // {
        //     // Start farming mode
        //     Card CardScript = e.Target.GetComponent<Card>();
        //     string cropName = CardScript.name;

        //     Sender.BroadcastFarmingModeEvent(new Vector3(0, 0, 0), cropName);
        // }
        // else if (e.ActionName == "AdvanceTime")
        // {
        //     Sender.BroadcastAdvanceTimeEvent();
        // }
    }
}
