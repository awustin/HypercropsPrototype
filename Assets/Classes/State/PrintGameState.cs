using UnityEngine;

public class PrintGameState : MonoBehaviour
{
    public GameState State;

    public GameMode CurrentGameMode;
    public bool IsWalking;
    public bool IsUIInteraction;
    public int NumberOfCardsInHand;
    public GameObject LastSelected;

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
        LastSelected = State.LastSelected;
    }
}
