using UnityEngine;
using System.Linq;
using System;

using Assets.Hypercrops.Common.Serializables;
using System.Collections.Generic;

namespace Assets.Hypercrops.Events
{
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
        public event EventHandler<EnableFeatureArguments> EnableFeature;
        public event Action RestartScene;
        public event Action EndScene;

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

        public void BroadcastEvent(string eventName, params object[] arguments)
        {
            List<object> args = arguments.ToList();
            IterativeValidator validator = new(args);

            switch (eventName)
            {
                case "WalkEvent":
                    if (validator.N(1).T(out Vector3 target).IsValid())
                        WalkEvent?.Invoke(this, new WalkEventArguments(target));
                    break;

                case "TryPlantCrop":
                    TryPlantCrop?.Invoke();
                    break;

                case "TryPlaceBuilding":
                    TryPlaceBuilding?.Invoke();
                    break;

                case "StartFarmMode":
                    if (validator.N(2).T(out Vector3 point).T(out CropDescriptor cropDescriptor).IsValid())
                        StartFarmMode?.Invoke(this, new StartFarmModeArguments(point, cropDescriptor));
                    break;

                case "StartBuildMode":
                    if (validator.N(1).T(out BuildableDescriptor descriptor).IsValid())
                        StartBuildMode?.Invoke(this, new StartBuildModeArguments(descriptor));
                    break;

                case "CancelFarmMode":
                    CancelFarmMode?.Invoke();
                    break;

                case "CancelBuildMode":
                    CancelBuildMode?.Invoke();
                    break;

                case "AdvanceTimeEvent":
                    AdvanceTimeEvent?.Invoke();
                    break;

                case "CropDeath":
                    if (validator.N(1).T(out GameObject cropObject).IsValid())
                        CropDeathEvent?.Invoke(this, new CropDeathArguments(cropObject));
                    break;

                case "NewDay":
                    NewDayEvent?.Invoke();
                    break;

                case "EnableFeature":
                    if (validator.N(1).T(out string featureName).IsValid())
                        EnableFeature?.Invoke(this, new EnableFeatureArguments(featureName));
                    break;

                case "RestartScene":
                    RestartScene?.Invoke();
                    break;

                case "EndScene":
                    EndScene?.Invoke();
                    break;

                default:
                    Debug.LogWarning($"Unknown event: {eventName}");
                    break;
            }
        }
    }
}
