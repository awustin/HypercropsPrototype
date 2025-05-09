using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using Assets.Hypercrops.State;
using Assets.Hypercrops.Events;

// TODO: Improve user interactions e.g. debounce
// TODO: Rethink game modes manager

namespace Assets.Hypercrops.Handlers
{
    public class InteractionsHandler : MonoBehaviour
    {
        [HideInInspector] public GameState State;
        [HideInInspector] public Camera MainCamera;
        [HideInInspector] public LayerMask DefaultLayer;
        [HideInInspector] public GameEventSender Sender;
        private InputAction _interact;
        private InputAction _screenPoint;
        private InputAction _cancel;
        private InputAction _restartScene;
        private readonly List<InputAction> _buttonActions = new();

        void Start()
        {
            State = GameState.Instance;
            MainCamera = Camera.main;
            DefaultLayer = LayerMask.GetMask("Default");
            Sender = GameEventSender.Instance;
            _interact = InputSystem.actions.FindActionMap("Player").FindAction("Interact");
            _screenPoint = InputSystem.actions.FindActionMap("Player").FindAction("ScreenPoint");
            _cancel = InputSystem.actions.FindActionMap("Player").FindAction("Cancel");
            _restartScene = InputSystem.actions.FindActionMap("Player").FindAction("RestartScene");

            _buttonActions.Add(_interact);
            _buttonActions.Add(_cancel);
            _buttonActions.Add(_restartScene);
        }

        void Update()
        {
            if (NoActions())
            {
                return;
            }

            if (IsRestartSceneAction())
            {
                Sender.BroadcastEvent("RestartScene");
                return;
            }

            if (IsInteractAction() && State.IsUIInteraction)
            {
                return;
            }

            if (IsCancelAction())
            {
                StartCancel();
                return;
            }

            Vector2 screenPoint = _screenPoint.ReadValue<Vector2>();
            Vector2 startPoint = MainCamera.ScreenToViewportPoint(screenPoint);

            if (IsOutsideViewport(startPoint))
            {
                return;
            }

            if (RayOnDefaultLayer(startPoint, out RaycastHit hit))
            {
                StartInteract(hit);
            }
        }

        private void StartInteract(RaycastHit hit)
        {
            GameObject targetObject = hit.collider.gameObject;
            Vector3 point = hit.point;

            // If game plan is default and clicked on the ground, move
            if (State.IsDefaultGameMode() && targetObject.CompareTag("Ground"))
            {
                Sender.BroadcastEvent("WalkEvent", point);
                return;
            }

            // If game plan is farming, try planting a seed
            if (State.IsFarmingGameMode())
            {
                Sender.BroadcastEvent("TryPlantCrop");
                return;
            }

            // If game plan is building, try placing a buildable
            if (State.IsBuildingGameMode())
            {
                Sender.BroadcastEvent("TryPlaceBuilding");
                return;
            }
        }

        private void StartCancel()
        {
            if (State.IsFarmingGameMode())
            {
                Sender.BroadcastEvent("CancelFarmMode");
            }
            else if (State.IsBuildingGameMode())
            {
                Sender.BroadcastEvent("CancelBuildMode");
            }
        }

        private bool IsOutsideViewport(Vector2 point)
        {
            return point.x < 0 || point.x > 1 || point.y < 0 || point.y > 1;
        }

        private bool RayOnDefaultLayer(Vector2 viewportPoint, out RaycastHit hit)
        {
            Ray ray = MainCamera.ViewportPointToRay(viewportPoint);
            return Physics.Raycast(ray, out hit, 100, DefaultLayer);
        }

        private bool NoActions()
        {
            return _buttonActions.TrueForAll(action => !action.WasPressedThisFrame());
        }

        private bool IsInteractAction()
        {
            return _interact.WasPressedThisFrame();
        }

        private bool IsCancelAction()
        {
            return _cancel.WasPressedThisFrame();
        }

        private bool IsRestartSceneAction()
        {
            return _restartScene.WasPressedThisFrame();
        }
    }
}
