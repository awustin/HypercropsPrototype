using UnityEngine;

using Assets.Classes.System;
using Assets.Classes.System.Common;

// TOOD: Initialise crop with CropName, CropSpecies and CropFarmingMethod
// TODO: Add precision to plant position
// TODO: Add collision detection between ghosts, crops and the rest of the objects
public class GameEventOnFarmingMode : MonoBehaviour
{
    public GameEventSender Sender;
    public FarmManager Farm;
    public GameState State;
    public CardsManager Cards;
    public ObjectFactory Factory;

    public CropGhost _currentGhost;
    private CropDescriptor _currentCropDescriptor;

    void OnEnable()
    {
        Sender = GameEventSender.Instance;
        State = GameState.Instance;
        Farm = FarmManager.Instance;
        Cards = CardsManager.Instance;
        Factory = ObjectFactory.Instance;

        Sender.StartFarmMode += OnStartFarmMode;
        Sender.TryPlantEvent += OnTryPlantEvent;
        Sender.CancelFarmModeEvent += OnCancelFarmMode;
    }

    void OnDisable()
    {
        Sender.StartFarmMode -= OnStartFarmMode;
        Sender.TryPlantEvent -= OnTryPlantEvent;
        Sender.CancelFarmModeEvent -= OnCancelFarmMode;
    }

    void Update()
    {
        if (State.IsUIInteraction)
        {
            return;
        }
    }

    public void OnStartFarmMode(object sender, StartFarmModeArguments e)
    {
        _currentCropDescriptor = e.CropDescripor;
        _currentGhost = Factory.MakeCropGhost(transform.position, CropSize.Normal, transform).GetComponent<CropGhost>();

        State.SetFarmingGameMode();
    }

    public void OnTryPlantEvent()
    {
        State.SetDefaultGameMode();

        if (_currentGhost.IsAllowed && _currentCropDescriptor != null)
        {
            Farm.StartCrop(_currentCropDescriptor, _currentGhost.PlantPoint);
            Cards.DiscardLastUsed();
        }
        else
        {
            Farm.DiscardCurrentCrop();
        }

        Clear();
    }

    public void OnCancelFarmMode()
    {
        if (State.IsFarmingGameMode())
        {
            Farm.DiscardCurrentCrop();
            State.SetLastCardSelected(null);
            State.SetDefaultGameMode();
            Clear();
        }
    }

    private void Clear()
    {
        _currentCropDescriptor = null;
        _currentGhost = null;

        if (_currentGhost != null)
        {
            Destroy(_currentGhost.gameObject);
        }
    }
}
