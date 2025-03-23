using UnityEngine;
using UnityEngine.InputSystem;

/*
    IMPROVEMENTS:
    - Add debounce for player interactions
*/
public class InteractionsManager : MonoBehaviour
{
    [HideInInspector] public GameState State;
    [HideInInspector] public Camera MainCamera;
    [HideInInspector] public LayerMask DefaultLayer;
    [HideInInspector] public GameEventSender Sender;
    private InputAction _interact;
    private InputAction _screenPoint;

    [Header("Interaction Details")]
    [SerializeField] GameObject LastSelected;

    void Start()
    {
        State = GameState.Instance;
        MainCamera = Camera.main;
        DefaultLayer = LayerMask.GetMask("Default");
        Sender = GameEventSender.Instance;
        _interact = InputSystem.actions.FindActionMap("Player").FindAction("Interact");
        _screenPoint = InputSystem.actions.FindActionMap("Player").FindAction("ScreenPoint");
    }

    void Update()
    {
        if (_interact == null || !_interact.WasPressedThisFrame())
        {
            return;
        }

        if (State.IsUIInteraction)
        {
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
            LastSelected = hit.collider.gameObject;
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

    private bool IsOutsideViewport(Vector2 point)
    {
        return point.x < 0 || point.x > 1 || point.y < 0 || point.y > 1;
    }

    private bool RayOnDefaultLayer(Vector2 viewportPoint, out RaycastHit hit)
    {
        Ray ray = MainCamera.ViewportPointToRay(viewportPoint);
        return Physics.Raycast(ray, out hit, 100, DefaultLayer);
    }
}
