using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class CardsDeck : MonoBehaviour
{
    public ObjectFactory Factory;
    public DataLoader Loader;
    public int NumberTotalCards = 10;
    public int NumberLeftCards;
    private List<CardWeight> _currentCardWeights;
    private List<int> _deck = new();

    void Awake()
    {
        name = "CardsDeck";
        Factory = ObjectFactory.Instance;
        Loader = DataLoader.Instance;
        _currentCardWeights = Loader.GetInitialCardWeights();
        NumberLeftCards = NumberTotalCards;
    }

    public void InitialiseDeck()
    {
        _deck.Clear();

        float totalWeight = _currentCardWeights.Sum(x => x.weight);
        List<int> allCards = new();

        foreach (CardWeight cardWeight in _currentCardWeights)
        {
            int count = Mathf.RoundToInt(cardWeight.weight / totalWeight * NumberTotalCards);

            for (int i = 0; i < count; i++)
            {
                allCards.Add(cardWeight.id);
            }
        }

        if (allCards.Count < NumberTotalCards)
        {
            int diff = NumberTotalCards - allCards.Count;
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
            int deckId = _deck.Count;
            _deck.RemoveAt(0);
            NumberLeftCards --;

            GameObject created = Factory.MakeCard(chosenId, GameObject.Find("CardsPanel").transform);
            Card CardScript = created.GetComponent<Card>();

            CardScript.SetDeckId(deckId);

            return created;
        }
        else
        {
            Debug.LogWarning("Deck is empty!");
            return null;
        }
    }
}
