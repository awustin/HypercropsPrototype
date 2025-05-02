using UnityEngine;

using Assets.Classes.Common.Enums;

// TODO: Fix phases
// TODO: Add dead phase to crop
// TODO: Add on click to make it interactable
public class Crop : MonoBehaviour
{
    [HideInInspector] public GameEventSender Sender;
    public CropHealth Health;
    public CropCollider Collider;
    public CropPhases Phases;
    public CropSpecies Species;
    public CropFarmingMethod FarmingMethod = CropFarmingMethod.Normal;
    public CropSize Size = CropSize.Normal;

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

    public void Initialise
    (
        CropSpecies species,
        CropFarmingMethod farmingMethod,
        CropSize size
    )
    {
        Species = species;
        FarmingMethod = farmingMethod;
        Size = size;

        Collider.Initialise(size);
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
