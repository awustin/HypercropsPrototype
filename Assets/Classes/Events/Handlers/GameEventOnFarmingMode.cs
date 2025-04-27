using UnityEngine;
using UnityEngine.InputSystem;


// TODO: Add precision to plant position
// TODO: Add collision detection between ghosts, crops and the rest of the objects
public class GameEventOnFarmingMode : MonoBehaviour
{
    [HideInInspector] public InputAction ScreenPoint;
    [HideInInspector] public Camera MainCamera;
    [HideInInspector] public GameEventSender Sender;
    [HideInInspector] public FarmManager Farm;
    [HideInInspector] public LayerMask FarmableLayer;
    [HideInInspector] public GameState State;
    [HideInInspector] public CardsManager Cards;

    [Header("Settings")]
    [SerializeField] public float PlantRadius = 10f;
    [SerializeField] public Vector3 PlantPoint;
    private bool _ghostFollowEnabled = false;
    private bool _plantable = false;

    void Start()
    {
        State = GameState.Instance;
        Farm = GameObject.Find("FarmManager").GetComponent<FarmManager>();
        MainCamera = Camera.main;
        ScreenPoint = InputSystem.actions.FindActionMap("Player").FindAction("ScreenPoint");
        FarmableLayer = LayerMask.GetMask("Default");
        Cards = CardsManager.Instance;
    }

    void OnEnable()
    {
        Sender = GameEventSender.Instance;

        if (Sender != null)
        {
            Sender.TryPlantEvent += OnTryPlantEvent;
            Sender.FarmingModeEvent += OnFarmingModeEvent;
            Sender.CancelFarmModeEvent += OnCancelFarmMode;
        }
        else
        {
            Debug.LogWarning("No GameEventSender detected");
        }
    }

    void OnDisable()
    {
        Sender.TryPlantEvent -= OnTryPlantEvent;
        Sender.FarmingModeEvent -= OnFarmingModeEvent;
        Sender.CancelFarmModeEvent -= OnCancelFarmMode;
    }

    void Update()
    {
        if (State.IsUIInteraction)
        {
            return;
        }

        if (_ghostFollowEnabled)
        {
            GhostFollowPointer();
        }
    }

    void OnFarmingModeEvent(object sender, FarmingModeEventArguments e)
    {
        string cropName = e.CropName;

        State.SetFarmingGameMode();
        Farm.NewCrop(cropName, new Vector3(0, 0, 0));
        _ghostFollowEnabled = true;
    }

    void OnTryPlantEvent()
    {
        _ghostFollowEnabled = false;
        State.SetDefaultGameMode();

        if (_plantable)
        {
            Farm.PlaceAndStartSeed(PlantPoint);
            Cards.DiscardLastUsed();
        }
        else
        {
            Farm.DiscardCurrentCrop();
        }
    }

    void OnCancelFarmMode()
    {
        if (State.IsFarmingGameMode())
        {
            _ghostFollowEnabled = false;
            Farm.DiscardCurrentCrop();
            State.SetDefaultGameMode();
            State.SetLastCardSelected(null);
        }
    }

    private void GhostFollowPointer()
    {
        Vector2 screenPoint = ScreenPoint.ReadValue<Vector2>();
        Vector2 viewportPoint = MainCamera.ScreenToViewportPoint(screenPoint);
        Ray ray = MainCamera.ViewportPointToRay(viewportPoint);

        if (Physics.Raycast(ray, out RaycastHit hit, 100, FarmableLayer))
        {
            Vector3 snapped = FarmUtils.SnapPoint(hit.point);
            Farm.SetPosition(snapped);

            if (Farm.IsPlantInPosition(FarmUtils.PositionToKey(snapped)) ||
                !FarmUtils.IsPlantablePoint(
                    snapped,
                    gameObject,
                    hit.collider.gameObject,
                    PlantRadius
                )
            )
            {
                Farm.TrySetGhostAllowed(false);
                _plantable = false;
                return;
            }

            Farm.TrySetGhostAllowed(true);
            PlantPoint = snapped;
            _plantable = true;
        }
    }
}
