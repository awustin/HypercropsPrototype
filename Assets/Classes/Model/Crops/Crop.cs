using UnityEngine;

public class Crop : MonoBehaviour
{
    [HideInInspector] public GameEventSender Sender;
    [HideInInspector] public ObjectFactory Factory;
    public CropHealth Health;
    public CropCollider Collider;
    public CropPhases Phases;
    public string CropName;
    public CropSize Size = CropSize.Normal;

    void Start()
    {
        // TOOD: REPLACE EVERY CropName by new Enum Species
        // TODO: Add dead phase to crop
        // TODO: Add on click to make it interactable
        Factory = ObjectFactory.Instance;
        Collider.Initialise(Size);
    }

    void OnEnable()
    {
        Sender = GameEventSender.Instance;
        Sender.AdvanceTimeEvent += OnAdvanceTimeEvent;
        Sender.NewDayEvent += OnNewDay;
    }

    void OnDisable()
    {
        Sender.AdvanceTimeEvent -= OnAdvanceTimeEvent;
        Sender.NewDayEvent -= OnNewDay;
    }

    public void IncrementCropPhase()
    {
        if (Phases.IsLast())
        {
            return;
        }
    
        Phases.NextPhase();
    }

    public void WaterCrop()
    {
        Health.IsWatered = true;
    }

    public void OnAdvanceTimeEvent()
    {
        IncrementCropPhase();
    }

    public void OnNewDay()
    {
        IncrementCropPhase();
        Health.IsWatered = false;
    }
}
