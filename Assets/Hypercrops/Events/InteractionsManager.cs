using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using Assets.Hypercrops.State;

// TODO: Improve user interactions e.g. debounce
// TODO: Rethink game modes manager
public class InteractionsManager : MonoBehaviour
{
    [HideInInspector] public GameState State;
    [HideInInspector] public Camera MainCamera;
    [HideInInspector] public LayerMask DefaultLayer;
    [HideInInspector] public GameEventSender Sender;
    private InputAction _interact;
    private InputAction _screenPoint;
    private InputAction _cancel;
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

        _buttonActions.Add(_interact);
        _buttonActions.Add(_cancel);
    }

    void Update()
    {
        if (NoActions())
        {
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
            Sender.BroadcastWalkEvent(point);
            return;
        }

        // If game plan is farming, try planting a seed
        if (State.IsFarmingGameMode())
        {
            Sender.BroadcastTryPlantEvent();
        }
    }

    private void StartCancel()
    {
        // If game plan is farming, cancel farm mode
        if (State.IsFarmingGameMode())
        {
            Sender.BroadcastCancelFarmModeEvent();
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
}
