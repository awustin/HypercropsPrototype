using UnityEngine;
using TMPro;

public class WorldTimeManager : MonoBehaviour
{
    // TODO: New year event
    // FIXME: Calibrate me
    //World has a 12 world-hour day.
    // 1 real second -> 1 world minute
    // 1 real minute -> 1 world hour
    // 12 real minutes -> 1 world day

    public GameState State;
    public Clock WorldClock;
    public GameObject uiComponentDay;
    public int Day = 1;
    public int Year = 1;
    public WorldTimeScale TimeScale = WorldTimeScale.Normal;
    private WorldTimeScale _timeScaleTracker;
    private int _dayTracker;
    private int _tickFrequency;
    private int _currentSecond;
    private bool _isDayTick;

    void Start()
    {
        State = GameState.Instance;

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

        if (_dayTracker != Day)
        {
            _dayTracker = Day;
            UpdateUI();
        }

        WorldTimeTick();

        if (_isDayTick)
        {
            TriggerNewDay();
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
    }
}
