using UnityEngine;

public class WorldTimeManager : MonoBehaviour
{
    // TODO: Make the clock advance
    // FIXME: Calibrate me
    //World has a 12 world-hour day.
    // 1 real second -> 1 world minute
    // 1 real minute -> 1 world hour
    // 12 real minutes -> 1 world day

    public GameObject uiComponent;
    public GameState State;
    public float WorldTimeMultiplier;
    public int HoursInDay;
    public int DaysInYear;
    [HideInInspector] public Clock WorldClock;

    void Start()
    {
        State = GameState.Instance;
        WorldClock = uiComponent.GetComponent<Clock>();
        WorldClock.Initialise(0 ,0);
        WorldClock.HoursInDay = HoursInDay;
        State.SetSecondsInScene(0);
    }

    void Update()
    {
        WorldTimeTick();
    }

    public void WorldTimeTick()
    {
        int seconds = (int) Time.timeSinceLevelLoad;

        if (State.SecondsInScene == seconds) {
            return;
        }

        State.SetSecondsInScene(seconds);
        WorldClock.Tick();
    }
}
