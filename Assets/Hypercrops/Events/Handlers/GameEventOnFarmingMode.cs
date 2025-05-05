using UnityEngine;

using Assets.Hypercrops.System;
using Assets.Hypercrops.System.CommonSerializable;
using Assets.Hypercrops.State;
using Assets.Hypercrops.Model.Crops;
using Assets.Hypercrops.Model.Cards;
using Assets.Hypercrops.Model.Ghosts;

// TOOD: DONT destroy the ghost. Save it in a cache.
// TODO: Add precision to plant position
// TODO: Add collision detection between ghosts, crops and the rest of the objects
public class GameEventOnFarmingMode : MonoBehaviour
{
    public GameEventSender Sender;
    public FarmManager Farm;
    public GameState State;
    public CardsManager Cards;
    public ObjectFactory Factory;

    private CropGhost _currentGhost;
    private CropDescriptor _currentCropDescriptor;

    void OnEnable()
    {
        Sender = GameEventSender.Instance;
        State = GameState.Instance;
        Farm = FarmManager.Instance;
        Cards = CardsManager.Instance;
        Factory = ObjectFactory.Instance;

        Sender.StartFarmMode += OnStartFarmMode;
        Sender.TryPlantCrop += OnTryPlantCrop;
        Sender.CancelFarmMode += OnCancelFarmMode;
    }

    void OnDisable()
    {
        Sender.StartFarmMode -= OnStartFarmMode;
        Sender.TryPlantCrop -= OnTryPlantCrop;
        Sender.CancelFarmMode -= OnCancelFarmMode;
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
        _currentCropDescriptor = e.Descriptor;
        _currentGhost = Factory.MakeCropGhost(transform.position, CropSize.Normal, transform).GetComponent<CropGhost>();

        State.SetFarmingGameMode();
    }

    public void OnTryPlantCrop()
    {
        State.SetDefaultGameMode();

        if (_currentGhost.IsAllowed && _currentCropDescriptor != null)
        {
            Farm.StartCrop(_currentCropDescriptor, _currentGhost.ActionPoint);
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
        if (_currentGhost != null)
        {
            Destroy(_currentGhost.gameObject);
        }

        _currentCropDescriptor = null;
        _currentGhost = null;
    }
}
