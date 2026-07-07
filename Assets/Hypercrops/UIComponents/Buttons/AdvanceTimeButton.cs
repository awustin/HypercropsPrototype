using UnityEngine.EventSystems;

using Assets.Hypercrops.Events;
using Assets.Hypercrops.State;

// For prototype debugging
public class AdvanceTimeButton : GameObjectUIBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData data)
    {
        GameEventSender.Instance.BroadcastEvent("AdvanceTimeEvent");
    }
}
