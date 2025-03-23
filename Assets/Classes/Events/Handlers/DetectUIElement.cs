using UnityEngine;
using UnityEngine.EventSystems;

public class DetectUIElement :
    MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    [HideInInspector]
    public GameState State;

    void Start()
    {
        State = GameState.Instance;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        State.IsUIInteraction = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        State.IsUIInteraction = false;
    }
}
