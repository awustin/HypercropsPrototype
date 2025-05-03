using UnityEngine;
using UnityEngine.EventSystems;

// For prototype debugging
public class AdvanceTimeButton : UIElementBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData data)
    {
        GameEventSender.Instance.BroadcastAdvanceTimeEvent();
    }
}
