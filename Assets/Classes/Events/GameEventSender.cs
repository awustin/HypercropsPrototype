using UnityEngine;
using System;

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
    public event EventHandler<FarmingModeEventArguments> FarmingModeEvent;
    public event Action AdvanceTimeEvent;

    public void BroadcastWalkEvent(Vector3 target)
    {
        WalkEvent?.Invoke(this, new WalkEventArguments(target));
    }

    public void BroadcastTryPlantEvent()
    {
        TryPlantEvent?.Invoke();
    }

    public void BroadcastFarmingModeEvent(Vector3 point, string cropName)
    {
        FarmingModeEvent?.Invoke(this, new FarmingModeEventArguments(point, cropName));
    }

    public void BroadcastAdvanceTimeEvent()
    {
        AdvanceTimeEvent?.Invoke();
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
