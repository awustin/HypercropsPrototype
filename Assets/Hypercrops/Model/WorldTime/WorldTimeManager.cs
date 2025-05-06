using UnityEngine;
using TMPro;

using Assets.Hypercrops.Common.Enums;
using Assets.Hypercrops.State;

namespace Assets.Hypercrops.Model.WorldTime
{
    public class WorldTimeManager : MonoBehaviour
    {

        public const int StartDay = 1;
        public const int StartYear = 1;
        public const int DaysInYear = 100;
        public GameEventSender Sender;

        [Header("100 day year")]
        public GameState State;
        public Clock WorldClock;
        public GameObject uiComponentDay;
        public GameObject uiComponentYear;
        public int Day = StartDay;
        public int Year = StartYear;
        public WorldTimeScale TimeScale = WorldTimeScale.Normal;
        private WorldTimeScale _timeScaleTracker;
        private int _dayTracker;
        private int _yearTracker;
        private int _tickFrequency;
        private int _currentSecond;
        private bool _isDayTick;

        void Start()
        {
            State = GameState.Instance;
            Sender = GameEventSender.Instance;

            State.SetTimeInWorld(0, 0);
            WorldClock.Initialise(0, 0);

            SetTickFrequency();
            _currentSecond = 0;
        }

        void Update()
        {
            if (_timeScaleTracker != TimeScale)
            {
                _timeScaleTracker = TimeScale;
                SetTickFrequency();
            }

            if (_dayTracker != Day || _yearTracker != Year)
            {
                _dayTracker = Day;
                _yearTracker = Year;
                UpdateUI();
            }

            WorldTimeTick();

            if (_isDayTick)
            {
                TriggerNewDay();

                if (Day > DaysInYear)
                {
                    TriggerNewYear();
                }
            }
        }

        public void WorldTimeTick()
        {
            int currentSecond = MultiplyByPrecision(Time.timeSinceLevelLoad);

            if (_currentSecond == currentSecond)
            {
                return;
            }

            if (currentSecond % _tickFrequency == 0)
            {
                WorldClock.Tick(out _isDayTick);
                State.SetTimeInWorld(WorldClock.ToString());
            }

            _currentSecond = currentSecond;
        }

        public void TriggerNewDay()
        {
            Day ++;
            State.SetDayInWorld(Day);
            _isDayTick = false;

            Sender.BroadcastNewDayEvent();
        }

        public void TriggerNewYear()
        {
            Year ++;
            Day = 1;
            State.SetDayInWorld(Day);
            State.SetYearInWorld(Year);
        }

        public void SetTimeScale(WorldTimeScale timeScale)
        {
            TimeScale = timeScale;
        }

        private void SetTickFrequency()
        {
            _tickFrequency = MultiplyByPrecision((float) 1 / (int) TimeScale);
        }

        private int MultiplyByPrecision(float value)
        {
            return (int) Mathf.Round(value * (int) TimeScale);
        }

        private void UpdateUI()
        {
            uiComponentDay.GetComponent<TMP_Text>().text = Day.ToString();
            uiComponentYear.GetComponent<TMP_Text>().text = Year.ToString();
        }
    }
}
