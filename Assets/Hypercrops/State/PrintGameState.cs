using System.Collections.Generic;
using UnityEngine;


namespace Assets.Hypercrops.State
{
    public class PrintGameState : MonoBehaviour
    {
        public GameState State;
        public GameMode CurrentGameMode;
        public bool IsWalking;
        public bool IsUIInteraction;
        public GameObject LastCardSelected;
        public string Time;
        public int DayInWorld;
        public int YearInWorld;
        public List<string> LevelFeatures;
        public List<float> CropsPoints;
        public List<float> BioStoragePoints;

        [Header("Game Round")]
        public string CurrentRoundStage;
        public bool IsCardInvokable;

        void Start()
        {
            State = GameState.Instance;
        }

        void Update()
        {
            CurrentGameMode = State.CurrentGameMode;
            IsWalking = State.IsWalking;
            IsUIInteraction = State.IsUIInteraction;
            LastCardSelected = State.LastCardSelected;
            Time = $"{State.HourInWorld:D2}:{State.MinuteInWorld:D2}";
            DayInWorld = State.DayInWorld;
            YearInWorld = State.YearInWorld;
            LevelFeatures = State.LevelFeatures;
            CropsPoints = State.CropsPoints;
            BioStoragePoints = State.BioStoragePoints;
            CurrentRoundStage = State.CurrentRoundStage;
            IsCardInvokable = State.IsCardInvokable;
        }
    }
}