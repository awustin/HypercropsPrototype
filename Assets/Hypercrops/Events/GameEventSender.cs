using UnityEngine;
using System;

using Assets.Hypercrops.Common.Serializables;

public class GameEventSender : MonoBehaviour
{
    private static GameEventSender _instance;
    public static GameEventSender Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<GameEventSender>();

                if (_instance == null)
                {
                    GameObject singletonObject = new("GameEventSender");
                    _instance = singletonObject.AddComponent<GameEventSender>();
                }
            }

            return _instance;
        }
    }
    public event EventHandler<WalkEventArguments> WalkEvent;
    public event Action TryPlantCrop;
    public event Action TryPlaceBuilding;
    public event EventHandler<StartFarmModeArguments> StartFarmMode;
    public event EventHandler<StartBuildModeArguments> StartBuildMode;
    public event Action CancelFarmMode;
    public event Action CancelBuildMode;
    public event Action AdvanceTimeEvent;
    public event EventHandler<CropDeathArguments> CropDeathEvent;
    public event Action NewDayEvent;

    public void BroadcastWalkEvent(Vector3 target)
    {
        WalkEvent?.Invoke(this, new WalkEventArguments(target));
    }

    public void BroadcastTryPlantCrop()
    {
        TryPlantCrop?.Invoke();
    }

    public void BroadcastTryPlaceBuilding()
    {
        TryPlaceBuilding?.Invoke();
    }

    public void BroadcastStartFarmMode(Vector3 point, CropDescriptor cropDescriptor)
    {
        StartFarmMode?.Invoke(this, new StartFarmModeArguments(point, cropDescriptor));
    }

    public void BroadcastStartBuildMode(BuildableDescriptor descriptor)
    {
        StartBuildMode?.Invoke(this, new StartBuildModeArguments(descriptor));
    }

    public void BroadcastCancelFarmMode()
    {
        CancelFarmMode?.Invoke();
    }
    public void BroadcastCancelBuildMode()
    {
        CancelBuildMode?.Invoke();
    }

    public void BroadcastAdvanceTimeEvent()
    {
        AdvanceTimeEvent?.Invoke();
    }

    public void BroadcastCropDeathEvent(GameObject cropObject)
    {
        CropDeathEvent?.Invoke(this, new CropDeathArguments(cropObject));
    }

    public void BroadcastNewDayEvent()
    {
        NewDayEvent?.Invoke();
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
}
