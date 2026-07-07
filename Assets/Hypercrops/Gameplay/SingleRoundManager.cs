using UnityEngine;
using TMPro;

using Assets.Hypercrops.State;
using Assets.Hypercrops.Gameplay.Enums;
using Assets.Hypercrops.Events;

namespace Assets.Hypercrops.Gameplay
{
    public class SingleRoundManager : MonoBehaviour
    {
        public GameState State;
        public RoundStage CurrentStage;
        public int CurrentTurn;
        public GameObject StageInfo;
        public GameEventSender Sender;
        public Camera MainCamera;
        public HexaGridGenerator HexaGrid;
        public GameObject CardsPanelObject;

        // Trackers
        private RoundStage _currentStage;
        private int _currentTurn;

        public void OnEnable()
        {
            if (State == null)
                State = GameState.Instance;

            if (StageInfo == null)
                StageInfo = GameObject.Find("StageInfo");

            if (Sender == null)
                Sender = GameEventSender.Instance;

            Sender.StartFieldStage += FieldStage;
        }

        public void OnDisable()
        {
            Sender.StartFieldStage -= FieldStage;
        }

        public void Update()
        {
            if (_currentStage != CurrentStage)
            {
                _currentStage = CurrentStage;
                SetCurrentStageInfo();
            }

            if (_currentTurn != CurrentTurn)
            {
                _currentTurn = CurrentTurn;
                SetCurrentTurnInfo();
            }
        }

        public void StartSingleRound()
        {
            CurrentStage = RoundStage.Cards;
            CurrentTurn = 1;
            SetCurrentStageInfo();
            SetCurrentTurnInfo();
            State.CurrentRoundStage = CurrentStage.ToString();

            MainCamera.GetComponent<VerticalScroll>().enabled = false;
            CardsPanelObject.SetActive(true);
            HexaGrid.InitialiseGrid();
            Sender.BroadcastEvent("StartCardStage");
        }

        private void FieldStage()
        {
            MainCamera.GetComponent<VerticalScroll>().enabled = true;
            CardsPanelObject.SetActive(false);

            //TODO: Tap on a cell to place a Map entity
        }

        private void SetCurrentStageInfo()
        {
            TMP_Text currentStageValue = StageInfo.transform.Find("Canvas/Traits/Stage/Value").gameObject.GetComponent<TMP_Text>();

            currentStageValue.text = CurrentStage.ToString();
        }

        private void SetCurrentTurnInfo()
        {
            TMP_Text currentStageValue = StageInfo.transform.Find("Canvas/Traits/Turn/Value").gameObject.GetComponent<TMP_Text>();

            currentStageValue.text = CurrentTurn.ToString();
        }
    }
}