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
                    GameObject singletonObject = new("CardsManager");
                    _instance = singletonObject.AddComponent<CardsManager>();
                }
            }

            return _instance;
        }
    }

    public int MaxNumberOfCards = 4;
    public Dictionary<int, GameObject> CardsInHand = new();
    public GameState State;
    public CardsDeck Deck;
    public ObjectFactory Factory;

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
        Factory = ObjectFactory.Instance;
        Deck = CardsDeck.Instance;

        State.SetNumberOfCardsInHand(CardsInHand.Count);
        Deck.InitialiseDeck();
        InitialiseHand();

        // TODO: Add card to advance time
        // TODO: Destroy card on use
    }

    public void InitialiseHand()
    {
        // Read cards data and any other param to calculate draw
        DestroyCurrentCards();

        for (int i = 0; i < MaxNumberOfCards; i++)
        {
            DrawFromTopOfDeck();
        }
    }

    public void UseCardAndDiscard(GameObject selected)
    {
        // Apply effect/trigger event
        // Find card in dictionary and remove
        // Re order and animate
        // Destroy instance
    }

    public void CreateCardDebug(int id)
    {
        GameObject cardInstance = Factory.MakeCard(id, GameObject.Find("CardsPanel").transform);
        CardsInHand.Add(cardInstance.GetInstanceID(), cardInstance);
    }

    private void DestroyCurrentCards()
    {
        if (CardsInHand.Count == 0)
        {
            return;
        }

        // TODO: destroy instances
    }

    private void DrawFromTopOfDeck()
    {
        if (CardsInHand.Count == MaxNumberOfCards)
        {
            return;
        }

        GameObject card = Deck.GetFromTop();
        Card CardScript = card.GetComponent<Card>();

        CardScript.TurnUpInHand(CardsInHand.Count);
        CardsInHand.Add(card.GetInstanceID(), card);
        State.IncreaseNumberOfCardsInHand();
    }
}
