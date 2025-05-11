using UnityEngine.EventSystems;

using Assets.Hypercrops.Events;

// For prototype debugging
public class EndLevelButton : UIElementBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData data)
    {
        GameEventSender.Instance.BroadcastEvent("EndScene");
    }
}
