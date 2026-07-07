using UnityEngine;
using UnityEngine.InputSystem;

using Assets.Hypercrops.Events;

namespace Assets.Hypercrops.Gameplay
{
    public class SingleRoundInputHandler : MonoBehaviour
    {
        public SingleRoundManager RoundManager;
        public InputAction Interact;
        public GameEventSender Sender;

        public void Start()
        {
            Interact = InputSystem.actions.FindActionMap("Player").FindAction("Interact");

            if (RoundManager == null)
                RoundManager = GameObject.Find("SingleRoundManager").GetComponent<SingleRoundManager>();

            if (Sender == null)
                Sender = GameEventSender.Instance;
        }
    }
}
