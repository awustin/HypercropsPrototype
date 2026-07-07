using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Hypercrops.State
{
    public class GameObjectUIBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public GameState State;

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (State == null)
                State = GameState.Instance;

            State.IsUIInteraction = true;
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (State == null)
                State = GameState.Instance;

            State.IsUIInteraction = false;
        }
    }
}