using UnityEngine.EventSystems;

using Assets.Hypercrops.Events;
using Assets.Hypercrops.State;

// For prototype debugging
public class EndLevelButton : GameObjectUIBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData data)
    {
        GameEventSender.Instance.BroadcastEvent("EndScene");
    }
}
