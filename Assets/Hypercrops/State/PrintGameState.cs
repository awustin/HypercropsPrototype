using UnityEngine;

public class PrintGameState : MonoBehaviour
{
    public GameState State;
    public GameMode CurrentGameMode;
    public bool IsWalking;
    public bool IsUIInteraction;
    public int NumberOfCardsInHand;
    public GameObject LastCardSelected;
    public string Time;
    public int DayInWorld;
    public int YearInWorld;

    void Start()
    {
        State = GameState.Instance;
    }

    void Update()
    {
        CurrentGameMode = State.CurrentGameMode;
        IsWalking = State.IsWalking;
        IsUIInteraction = State.IsUIInteraction;
        NumberOfCardsInHand = State.NumberOfCardsInHand;
        LastCardSelected = State.LastCardSelected;
        Time = $"{State.HourInWorld:D2}:{State.MinuteInWorld:D2}";
        DayInWorld = State.DayInWorld;
        YearInWorld = State.YearInWorld;
    }
}
