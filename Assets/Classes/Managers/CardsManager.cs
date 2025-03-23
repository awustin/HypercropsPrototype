using System.Collections.Generic;
using UnityEngine;

public class CardsManager : MonoBehaviour
{
    private static CardsManager _instance;
    public static CardsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<CardsManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("CardsManager");
                    _instance = singletonObject.AddComponent<CardsManager>();
                }
            }

            return _instance;
        }
    }

    public GameState State;
    public int MaxNumberOfCards = 4;
    public List<int> CropCardIdsList = new() { 100 };
    public List<int> InfrastructureCardIdsList = new() { 600 };
    public List<int> TechCardIdsList = new() { 1000 };
    public List<GameObject> CardsInHand = new();

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        State = GameState.Instance;
        State.SetNumberOfCardsInHand(MaxNumberOfCards);
        DrawCardsForPlayer();
    }

    private void DrawCardsForPlayer()
    {
        // Todo: Read cards data and any other param to calculate draw
        DestroyCurrentCards();
        PickRandomCards(CropCardIdsList, 2);
        PickRandomCards(InfrastructureCardIdsList, 1);
        PickRandomCards(TechCardIdsList, 1);
        InstantiateCards();
    }

    private void DestroyCurrentCards()
    {
        if (CardsInHand.Count == 0)
        {
            return;
        }

        // Todo: destroy instances
    }

    private void PickRandomCards(List<int> cardIds, int value)
    {
        if (State.NumberOfCardsInHand == MaxNumberOfCards || value <= 0)
        {
            return;
        }

        // Todo: add cards to hand
    }

    private void InstantiateCards()
    {
        // Todo: Instantiate 2D game objects to be added to the scene
        return;
    }
}
