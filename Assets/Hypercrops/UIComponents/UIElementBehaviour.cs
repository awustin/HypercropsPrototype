using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Hypercrops.State;

public class UIElementBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public GameState State;

    void Start()
    {
        State = GameState.Instance;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        State.IsUIInteraction = true;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        State.IsUIInteraction = false;
    }
}
