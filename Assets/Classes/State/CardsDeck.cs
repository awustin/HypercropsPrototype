using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "CardsDeck", menuName = "Scriptable Objects/CardsDeck")]
public class CardsDeck : ScriptableObject
{
    private static CardsDeck _instance;
    public static CardsDeck Instance
    {
        get
        {
            _instance = _instance != null ? _instance : FindFirstObjectByType<CardsDeck>();

            if (_instance == null)
            {
                _instance = CreateInstance<CardsDeck>();
                DontDestroyOnLoad(_instance);
            }

            return _instance;
        }
    }
    public ObjectFactory Factory;
    public DataLoader Loader;
    public int NumberOfCardsInDeck = 10;
    private List<CardWeight> _currentCardWeights;
    private List<int> _deck = new();

    void Awake()
    {
        // TODO: Move this to MonoBehaviour in a GameObject that represents the deck in the UI
        name = "CardsDeck Singleton";
        Factory = ObjectFactory.Instance;
        Loader = DataLoader.Instance;
        _currentCardWeights = Loader.GetInitialCardWeights();
    }

    public void InitialiseDeck()
    {
        _deck.Clear();

        float totalWeight = _currentCardWeights.Sum(x => x.weight);
        List<int> allCards = new();

        foreach (CardWeight cardWeight in _currentCardWeights)
        {
            int count = Mathf.RoundToInt(cardWeight.weight / totalWeight * NumberOfCardsInDeck);

            for (int i = 0; i < count; i++)
            {
                allCards.Add(cardWeight.id);
            }
        }

        if (allCards.Count < NumberOfCardsInDeck)
        {
            int diff = NumberOfCardsInDeck - allCards.Count;
            for (int i = 0; i < diff; i++)
            {
                int cardChosen = _currentCardWeights.OrderBy(x => Random.value).First().id;
                allCards.Add(cardChosen);
            }
        }

        _deck = allCards.OrderBy(x => Random.value).ToList();
    }

    public void ShuffleDeck()
    {
        if (_deck == null || _deck.Count <= 1)
        {
            return;
        }

        // Fisher-Yates shuffle algorithm
        System.Random seed = new();
        int n = _deck.Count;

        while (n > 1)
        {
            n--;
            int k = seed.Next(n + 1);
            (_deck[n], _deck[k]) = (_deck[k], _deck[n]);
        }
    }

    public GameObject GetFromTop()
    {
        if (_deck.Count > 0)
        {
            int chosenId = _deck[0];
            _deck.RemoveAt(0);

            return Factory.MakeCard(chosenId, GameObject.Find("CardsPanel").transform);
        }
        else
        {
            Debug.LogWarning("Deck is empty!");
            return null;
        }
    }
}
