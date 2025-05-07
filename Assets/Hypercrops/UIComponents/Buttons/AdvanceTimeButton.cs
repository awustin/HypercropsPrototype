using UnityEngine.EventSystems;

using Assets.Hypercrops.Events;

// For prototype debugging
public class AdvanceTimeButton : UIElementBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData data)
    {
        GameEventSender.Instance.BroadcastAdvanceTimeEvent();
    }
}
