using System.Collections.Generic;
using UnityEngine;

namespace Assets.Hypercrops.State
{
    [CreateAssetMenu(fileName = "GameState", menuName = "Scriptable Objects/GameState")]
    public class GameState : ScriptableObject
    {
        // TODO: Divide rest of the project in NAMESPACES
        private static GameState _instance;
        public static GameState Instance
        {
            get
            {
                _instance = _instance != null ? _instance : FindFirstObjectByType<GameState>();

                if (_instance == null)
                {
                    _instance = CreateInstance<GameState>();
                    DontDestroyOnLoad(_instance);
                }

                return _instance;
            }
        }

        public GameMode CurrentGameMode;
        public bool IsWalking;
        public bool IsUIInteraction;
        public int NumberOfCardsInHand;
        public GameObject LastCardSelected;
        public int HourInWorld;
        public int MinuteInWorld;
        public int DayInWorld;
        public int YearInWorld;
        public List<string> LevelFeatures = new();

        void Awake()
        {
            name = "Game State Singleton";
            CurrentGameMode = GameMode.Default;
            IsUIInteraction = false;
        }

        public void SetDefaultGameMode()
        {
            CurrentGameMode = GameMode.Default;
        }

        public void SetFarmingGameMode()
        {
            CurrentGameMode = GameMode.Farming;
        }

        public void SetBuildingGameMode()
        {
            CurrentGameMode = GameMode.Building;
        }

        public bool IsDefaultGameMode()
        {
            return CurrentGameMode == GameMode.Default;
        }

        public bool IsFarmingGameMode()
        {
            return CurrentGameMode == GameMode.Farming;
        }

        public bool IsBuildingGameMode()
        {
            return CurrentGameMode == GameMode.Building;
        }

        public void SetNumberOfCardsInHand(int value)
        {
            NumberOfCardsInHand = value;
        }

        public void DecreaseNumberOfCardsInHand()
        {
            if (NumberOfCardsInHand > 0)
            {
                NumberOfCardsInHand --;
            }
        }

        public void IncreaseNumberOfCardsInHand()
        {
            NumberOfCardsInHand ++;
        }

        public void SetLastCardSelected(GameObject target)
        {
            LastCardSelected = target;
        }

        public void SetTimeInWorld(int hour, int minute)
        {
            HourInWorld = hour;
            MinuteInWorld = minute;
        }

        public void SetTimeInWorld(string timeString)
        {
            string[] timeParts = timeString.Split(':');

            if (timeParts.Length != 2)
            {
                HourInWorld = 0;
                MinuteInWorld = 0;
                return;
            }

            if (!int.TryParse(timeParts[0], out HourInWorld))
            {
                HourInWorld = 0;
            }

            if (!int.TryParse(timeParts[1], out MinuteInWorld))
            {
                MinuteInWorld = 0;
            }
        }

        public void SetDayInWorld(int day)
        {
            DayInWorld = day;
        }

        public void SetYearInWorld(int year)
        {
            YearInWorld = year;
        }

        public void AddLevelFeature(string feature)
        {
            if (!LevelFeatures.Contains(feature))
            {
                LevelFeatures.Add(feature);
            }
        }

        public void RemoveLevelFeature(string feature)
        {
            if (LevelFeatures.Contains(feature))
            {
                LevelFeatures.Remove(feature);
            }

        }

        public void ClearLevelFeatures()
        {
            LevelFeatures.Clear();
        }
    }
}