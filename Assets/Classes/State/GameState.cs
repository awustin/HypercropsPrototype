using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "Scriptable Objects/GameState")]
public class GameState : ScriptableObject
{
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
    public int SecondsInScene;
    public int DayInWorld;
    public int YearInWorld;

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

    public void SetSecondsInScene(int value)
    {
        SecondsInScene = value;
    }

    public void SetDayInWorld(int day)
    {
        DayInWorld = day;
    }

    public void SetYearInWorl(int year)
    {
        YearInWorld = year;
    }
}
