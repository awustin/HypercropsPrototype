using UnityEngine;

public class WorldTimeManager : MonoBehaviour
{
    // TODO: New day and new year event
    // FIXME: Calibrate me
    //World has a 12 world-hour day.
    // 1 real second -> 1 world minute
    // 1 real minute -> 1 world hour
    // 12 real minutes -> 1 world day

    public GameObject uiComponent;
    public GameState State;
    public WorldTimeScale TimeScale = WorldTimeScale.Normal;
    public int HoursInDay;
    public int DaysInYear;
    [HideInInspector] public Clock WorldClock;
    private WorldTimeScale _timeScaleTracker;
    private int _tickFrequency;
    private int _currentSecond;

    void Start()
    {
        State = GameState.Instance;
        WorldClock = uiComponent.GetComponent<Clock>();

        State.SetTimeInWorld(0, 0);
        WorldClock.Initialise(0, 0);
        WorldClock.HoursInDay = HoursInDay;

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

        WorldTimeTick();
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
            WorldClock.Tick();
            State.SetTimeInWorld(WorldClock.ToString());
        }

        _currentSecond = currentSecond;
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
}
