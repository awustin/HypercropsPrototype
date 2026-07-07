using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

using Assets.Hypercrops.System;
using Assets.Hypercrops.State;
using Assets.Hypercrops.Events;
using Assets.Hypercrops.Model.MapEntities;

namespace Assets.Hypercrops.Model.Cards
{
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
        public GameObject DeckPrefab;
        public List<GameObject> CurrentCards = new();
        public GameState State;
        public ObjectFactory Factory;
        public GameEventSender Sender;
        public CardsInvoker Invoker;
        public MapEntitiesManager MapEntities;

        private CardsDeck _deck;

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

        public void OnEnable()
        {
            State = GameState.Instance;
            Factory = ObjectFactory.Instance;

            if (_deck == null)
                _deck = DeckPrefab.GetComponent<CardsDeck>();

            if (Sender == null)
                Sender = GameEventSender.Instance;

            if (MapEntities == null)
                MapEntities = MapEntitiesManager.Instance;

            Sender.StartCardStage += OnCardStageStart;
            Sender.SelectCard += OnCardSelected;
            Sender.DeselectCard += OnCardDeselected;
            Sender.InvokeCard += OnCardInvoked;
        }

        public void OnDisable()
        {
            Sender.StartCardStage -= OnCardStageStart;  
            Sender.SelectCard -= OnCardSelected;
            Sender.DeselectCard -= OnCardDeselected;
            Sender.InvokeCard -= OnCardInvoked;
        }

        public void DrawCard()
        {
            if (CurrentCards.Count == MaxNumberOfCards)
            {
                return;
            }

            GameObject card = _deck.Pop();

            if (card == null)
            {
                return;
            }

            Card CardScript = card.GetComponent<Card>();

            StartCoroutine(CardScript.FlipCard(CurrentCards.Count));
            CurrentCards.Add(card);
        }

        public void DiscardLastUsed()
        {
            GameObject target = State.LastCardSelected;

            if (target.GetComponent<Card>() == null)
            {
                return;
            }

            GameObject currentCard = FindCurrentCard(target);

            if (currentCard == null)
            {
                return;
            }

            State.SetLastCardSelected(null);
            RemoveCurrentCardAnReindex(currentCard);
            Destroy(currentCard);

            // TODO: Animate
        }

        private void OnCardStageStart()
        {
            _deck.InitialiseDeck();
            ResetHand();

            StartCoroutine(SpawnCardsInHand());
        }

        private void OnCardSelected(object sender, CardArgument Args)
        {
            GameObject cardObject = Args.CardObject;
            Card card = cardObject.GetComponent<Card>();

            if (!card.IsSelected)
                card.IsSelected = true;

            if (Invoker.ActiveCard == null)
            {
                Invoker.SetToActive(cardObject);
            }
            else
            {
                Invoker.SetToInvoked(cardObject);
            }

            State.SetLastCardSelected(cardObject);
        }

        public void OnCardDeselected(object sender, CardArgument Args)
        {
            GameObject cardObject = Args.CardObject;
            Card card = cardObject.GetComponent<Card>();

            if (card.IsSelected)
                card.IsSelected = false;

            if (Invoker.IsActiveCard(cardObject))
            {
                Invoker.DeselectActive();
            }
            else
            {
                Invoker.DeselectInvoked(cardObject);
            }
        }

        public void OnCardInvoked()
        {
            if (Invoker.ActiveCard == null || !Invoker.IsCardInvokable)
                return;

            Card activeCardScript = Invoker.ActiveCard.GetComponent<Card>();

            activeCardScript.StartEffect();
            MapEntities.ShowMessage(activeCardScript.MapEntityName);
            Invoker.ConsumeCards(CurrentCards);
            State.SetLastCardSelected(null);

            ReindexHand();
            Sender.BroadcastEvent("StartFieldStage");
        }

        private IEnumerator SpawnCardsInHand()
        {
            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < MaxNumberOfCards; i++)
            {
                if (CurrentCards.Count == MaxNumberOfCards)
                    yield break;

                GameObject card = _deck.Pop();
                Card CardScript = card.GetComponent<Card>();

                yield return CardScript.FlipCard(CurrentCards.Count);

                CurrentCards.Add(card);
            }

        }

        private void ReindexHand()
        {
            for (int index = 0; index < CurrentCards.Count; index++)
            {
                Card CardScript = CurrentCards[index].GetComponent<Card>();

                CardScript.SetOrder(index);
            }
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

            for (int index = 0; index < CurrentCards.Count; index++)
            {
                Card CardScript = CurrentCards[index].GetComponent<Card>();

                CardScript.SetOrder(index);
            }
        }

        private void ResetHand()
        {
            if (CurrentCards.Count == 0)
            {
                return;
            }
            
            for (int index = 0; index < CurrentCards.Count; index++)
            {
                GameObject card = CurrentCards[index];

                CurrentCards.Remove(card);
                Destroy(card);
            }

            _deck.ShuffleDeck();
        }
    }
}