using UnityEngine;

using Assets.Hypercrops.Common.Enums;
using Assets.Hypercrops.Events;
using Assets.Hypercrops.State;

// TODO: Add dead phase to crop
// TODO: Add on click to make it interactable
namespace Assets.Hypercrops.Model.Crops
{
    public class Crop : MonoBehaviour
    {
        public GameEventSender Sender;
        public GameState State;
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
            Sender.ProduceHarvestPoints += OnProduceHarvestPoints;
        }

        void OnDisable()
        {
            Sender.AdvanceTimeEvent -= OnAdvanceTimeEvent;
            Sender.NewDayEvent -= OnNewDay;
            Sender.ProduceHarvestPoints -= OnProduceHarvestPoints;
        }

        public void Initialise
        (
            CropSpecies species,
            CropFarmingMethod farmingMethod,
            CropSize size
        )
        {
            State = GameState.Instance;
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

        private void OnProduceHarvestPoints()
        {
            if (Phases.Current == CropPhase.Ready)
            {
                State.CropsPoints.Add((float) Species * Health.Life);
                Phases.SetHarvested();
            }
        }
    }
}