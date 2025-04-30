using UnityEngine;
using Assets.Classes.System;

// TODO: Add precision to plant position
// TODO: Add collision detection between ghosts, crops and the rest of the objects
// TODO: BUG when canceling one crop. The last added gets deleted as well
public class GameEventOnFarmingMode : MonoBehaviour
{
    public GameEventSender Sender;
    public FarmManager Farm;
    public GameState State;
    public CardsManager Cards;
    public ObjectFactory Factory;
    public CropGhost CurrentGhost;
    public string CurrentCropName;

    private GameObject _currentGhostObject;

    void Start()
    {
        State = GameState.Instance;
        Farm = GameObject.Find("FarmManager").GetComponent<FarmManager>();
        Cards = CardsManager.Instance;
        Factory = ObjectFactory.Instance;
    }

    void OnEnable()
    {
        Sender = GameEventSender.Instance;

        Sender.FarmingModeEvent += OnFarmingModeEvent;
        Sender.TryPlantEvent += OnTryPlantEvent;
        Sender.CancelFarmModeEvent += OnCancelFarmMode;
    }

    void OnDisable()
    {
        Sender.FarmingModeEvent -= OnFarmingModeEvent;
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

    public void OnFarmingModeEvent(object sender, FarmingModeEventArguments e)
    {
        CurrentCropName = e.CropName;
        _currentGhostObject = Factory.MakeCropGhost(transform.position, CropSize.Normal, transform);
        CurrentGhost = _currentGhostObject.GetComponent<CropGhost>();

        State.SetFarmingGameMode();
    }

    public void OnTryPlantEvent()
    {
        State.SetDefaultGameMode();
        Destroy(_currentGhostObject);

        if (CurrentGhost.IsAllowed && CurrentCropName != null)
        {
            Farm.StartCrop(CurrentCropName, CurrentGhost.PlantPoint);
            Cards.DiscardLastUsed();
        }
        else
        {
            Farm.DiscardCurrentCrop();
        }

        CurrentCropName = null;
        CurrentGhost = null;
    }

    public void OnCancelFarmMode()
    {
        if (State.IsFarmingGameMode())
        {
            Destroy(_currentGhostObject);
            Farm.DiscardCurrentCrop();
            State.SetDefaultGameMode();
            State.SetLastCardSelected(null);
            CurrentCropName = null;
            CurrentGhost = null;
        }
    }
}
