using UnityEngine;
using UnityEngine.InputSystem;

public class CropGhost : MonoBehaviour
{
    public InputAction ScreenPointAction;
    public Camera MainCamera;
    public LayerMask FarmableLayer;
    public GameObject Player;
    public FarmManager Farm;
    public GameObject Visuals;
    public bool IsAllowed;
    public Vector3 PlantPoint;
    public float PlantRadius = 10f;
    
    // Trackers
    private bool _isAllowedTracker;
    private Vector2 _screenPointTracker;

    void Start()
    {
        Farm = GameObject.Find("FarmManager").GetComponent<FarmManager>();
        MainCamera = Camera.main;
        ScreenPointAction = InputSystem.actions.FindActionMap("Player").FindAction("ScreenPoint");
        FarmableLayer = LayerMask.GetMask("Default");
        Player = GameObject.Find("Player");

        SetGhostColor();
    }

    void Update()
    {
        TrackVariables();
    }

    private void TrackVariables()
    {
        if (_isAllowedTracker != IsAllowed)
        {
            _isAllowedTracker = IsAllowed;
            SetGhostColor();
        }

        Vector2 screenPoint = ScreenPointAction.ReadValue<Vector2>();

        if (_screenPointTracker != screenPoint)
        {
            _screenPointTracker = screenPoint;
            FollowPointer();
        }
    }

    private void SetGhostColor()
    {
        MeshRenderer renderer = Visuals.GetComponent<MeshRenderer>();
        string colorHex = IsAllowed ? "#00FF65" : "#FF5000";

        if (ColorUtility.TryParseHtmlString(colorHex, out Color color))
        {
            color.a = 0.3f;
            renderer.material.color = color;
        }
    }

    private void FollowPointer()
    {
        Vector2 screenPoint = ScreenPointAction.ReadValue<Vector2>();
        Vector2 viewportPoint = MainCamera.ScreenToViewportPoint(screenPoint);
        Ray ray = MainCamera.ViewportPointToRay(viewportPoint);

        if (Physics.Raycast(ray, out RaycastHit hit, 100, FarmableLayer))
        {
            Vector3 snapped = FarmUtils.SnapPoint(hit.point);
            SetPosition(snapped);

            if (!IsPlantableAtPosition(snapped, hit.collider.gameObject))
            {
                IsAllowed = false;

                return;
            }

            IsAllowed = true;
            PlantPoint = snapped;
        }
    }

    private void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    private bool IsPlantableAtPosition(Vector3 pos, GameObject target)
    {
        return !Farm.IsPlantInPosition(pos) &&
            FarmUtils.IsPlantablePoint(pos, Player, target, PlantRadius);
    }

}
