using UnityEngine;
using System;

using Assets.Hypercrops.System.CommonSerializable;

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
                    GameObject singletonObject = new GameObject("GameEventSender");
                    _instance = singletonObject.AddComponent<GameEventSender>();
                }
            }

            return _instance;
        }
    }
    public event EventHandler<WalkEventArguments> WalkEvent;
    public event Action TryPlantEvent;
    public event EventHandler<StartFarmModeArguments> StartFarmMode;
    public event Action AdvanceTimeEvent;
    public event Action CancelFarmModeEvent;
    public event EventHandler<CropDeathArguments> CropDeathEvent;
    public event Action NewDayEvent;

    public void BroadcastWalkEvent(Vector3 target)
    {
        WalkEvent?.Invoke(this, new WalkEventArguments(target));
    }

    public void BroadcastTryPlantEvent()
    {
        TryPlantEvent?.Invoke();
    }

    public void BroadcastStartFarmMode(Vector3 point, CropDescriptor cropDescriptor)
    {
        StartFarmMode?.Invoke(this, new StartFarmModeArguments(point, cropDescriptor));
    }

    public void BroadcastAdvanceTimeEvent()
    {
        AdvanceTimeEvent?.Invoke();
    }

    public void BroadcastCancelFarmModeEvent()
    {
        CancelFarmModeEvent?.Invoke();
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
