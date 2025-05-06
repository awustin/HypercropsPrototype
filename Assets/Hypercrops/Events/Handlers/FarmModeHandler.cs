using UnityEngine;

using Assets.Hypercrops.Common.Enums;
using Assets.Hypercrops.System;
using Assets.Hypercrops.System.CommonSerializable;
using Assets.Hypercrops.State;
using Assets.Hypercrops.Model.Crops;
using Assets.Hypercrops.Model.Cards;
using Assets.Hypercrops.Model.Ghosts;

// TOOD: DONT destroy the ghost. Save it in a cache.
// TODO: Add precision to plant position
// TODO: Add collision detection between ghosts, crops and the rest of the objects
public class FarmModeHandler : MonoBehaviour
{
    public GameEventSender Sender;
    public FarmManager Farm;
    public GameState State;
    public CardsManager Cards;
    public ObjectFactory Factory;
    public GameObject GhostObject;

    private CropGhost _cropGhost;
    private CropDescriptor _currentCropDescriptor;

    void OnEnable()
    {
        Sender = GameEventSender.Instance;
        State = GameState.Instance;
        Farm = FarmManager.Instance;
        Cards = CardsManager.Instance;
        Factory = ObjectFactory.Instance;
        _cropGhost = GhostObject.GetComponent<CropGhost>();

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
        _cropGhost.Activate(CropSize.Normal);

        State.SetFarmingGameMode();
    }

    public void OnTryPlantCrop()
    {
        State.SetDefaultGameMode();

        if (_cropGhost.IsAllowed && _currentCropDescriptor != null)
        {
            Farm.StartCrop(_currentCropDescriptor, _cropGhost.ActionPoint);
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
        _cropGhost.Deactivate();
    }
}
