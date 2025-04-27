using UnityEngine;
using System.Collections.Generic;

public class Crop : MonoBehaviour
{
    [HideInInspector] public GameEventSender Sender;
    [HideInInspector] public ObjectFactory Factory;
    public CropHealth Health;
    public CropCollider Collider;
    public CropPhases Phases;
    public string Name;
    public CropSize Size = CropSize.Normal;
    public bool IsAllowed;
    public GameObject CropPhaseInstance;

    void Start()
    {
        // TODO: Smaller size
        // TODO: Add dead phase to crop
        // TODO: Add on click to make it interactable
        Factory = ObjectFactory.Instance;

        Name = name;
        IsAllowed = false;
        CropPhaseInstance = Factory.MakeCropPhase(Name, "Ghost", transform.position, transform);
        Collider.Initialise(Size);
    }

    public void SetAllowed(bool value)
    {
        if (Phases.Current != CropPhase.Ghost)
        {
            return;
        }

        if (IsAllowed && !value)
        {
            Factory.SetGhostMaterial(CropPhaseInstance, false);
            IsAllowed = false;
            return;
        }

        if (!IsAllowed && value)
        {
            Factory.SetGhostMaterial(CropPhaseInstance, true);
            IsAllowed = true;
            return;
        }
    }

    public void IncrementCropPhase(Vector3? pos)
    {
        if (Phases.IsLast())
        {
            return;
        }

        Destroy(CropPhaseInstance);
        Phases.NextPhase();

        CropPhaseInstance = Factory.MakeCropPhase(
            Name,
            Phases.Current.ToString(),
            pos != null ? pos.Value : transform.position,
            transform
        );
    }

    public void WaterCrop()
    {
        Health.IsWatered = true;
    }

    public void OnAdvanceTimeEvent()
    {
        // Next stage of crops
        IncrementCropPhase(transform.position);
    }

    public void OnNewDay()
    {
        IncrementCropPhase(transform.position);
        Health.IsWatered = false;
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
}
