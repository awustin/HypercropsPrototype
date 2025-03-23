using UnityEngine;
using UnityEngine.EventSystems;

public class GameEventButtonPressed : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public GameEventSender Sender;
    [SerializeField] public string ActionName;

    void OnEnable()
    {
        Sender = GameEventSender.Instance;
    }

    public void OnPointerClick(PointerEventData data)
    {
        Sender.BroadcastButtonEvent(data.pointerPress, ActionName);
    }
}
