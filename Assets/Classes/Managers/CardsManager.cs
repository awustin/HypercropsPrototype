using System.Collections.Generic;
using System.Linq;
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
    public GameObject CardsDeckObject;
    public List<GameObject> CurrentCards = new();
    public GameState State;
    public ObjectFactory Factory;
    [HideInInspector] public CardsDeck Deck;

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
        Deck = CardsDeckObject.GetComponent<CardsDeck>();

        State.SetNumberOfCardsInHand(CurrentCards.Count);
        Deck.InitialiseDeck();
        InitialiseHand();

        // TODO: Add capacity to DISCARD a card
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

    public void DrawCard()
    {
        DrawFromTopOfDeck();
    }

    public void DiscardLastSelected()
    {
        GameObject target = State.LastSelected;

        if (target.GetComponent<Card>() == null)
        {
            return;
        }

        GameObject currentCard = FindCurrentCard(target);

        if (currentCard == null)
        {
            return;
        }

        State.SetLastSelected(null);
        RemoveCurrentCardAnReindex(currentCard);
        Destroy(currentCard);

        // TODO: Animate
    }

    public void CreateCardDebug(int id)
    {
        GameObject cardInstance = Factory.MakeCard(id, GameObject.Find("CardsPanel").transform);
        CurrentCards.Add(cardInstance);
    }

    private GameObject FindCurrentCard(GameObject cardTarget)
    {
        return CurrentCards.SingleOrDefault(_card =>
            _card.GetComponent<Card>().Equals(cardTarget.GetComponent<Card>())
        );
    }

    private void RemoveCurrentCardAnReindex(GameObject target)
    {
        CurrentCards.Remove(target);
        State.DecreaseNumberOfCardsInHand();

        for (int index = 0; index < CurrentCards.Count; index++)
        {
            Card CardScript = CurrentCards[index].GetComponent<Card>();

            CardScript.SetOrder(index);
        }
    }

    private void DestroyCurrentCards()
    {
        if (CurrentCards.Count == 0)
        {
            return;
        }

        // TODO: destroy instances
    }

    private void DrawFromTopOfDeck()
    {
        if (CurrentCards.Count == MaxNumberOfCards)
        {
            return;
        }

        GameObject card = Deck.GetFromTop();
        Card CardScript = card.GetComponent<Card>();

        CardScript.TurnUpInHand(CurrentCards.Count);
        CurrentCards.Add(card);
        State.IncreaseNumberOfCardsInHand();
    }
}
