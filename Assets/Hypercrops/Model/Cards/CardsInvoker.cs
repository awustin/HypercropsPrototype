using UnityEngine;
using System.Collections.Generic;

using Assets.Hypercrops.Common;
using Assets.Hypercrops.State;

namespace Assets.Hypercrops.Model.Cards
{
    public class CardsInvoker : MonoBehaviour
    {
        public GameObject ActiveCard;
        public GameObject Invoked1;
        public GameObject Invoked2;
        public GameObject Invoked3;
        public GameObject Invoked4;
        public GameObject SlotsObject;
        public bool IsCardInvokable;
        public GameState State;

        private ObjectCache<InvokationRule> _rulesLookup = new();

        // Tracker
        private GameObject _activeCard;
        private GameObject _invoked1;
        private GameObject _invoked2;
        private GameObject _invoked3;
        private GameObject _invoked4;

        public void Start()
        {
            if (State == null)
                State = GameState.Instance;

            if (SlotsObject == null)
                SlotsObject = GameObject.Find("Slots");

            SlotsObject.SetActive(false);
        }

        public void Update()
        {
            if (_activeCard != ActiveCard)
            {
                _activeCard = ActiveCard;
                SlotsObject.SetActive(ActiveCard != null);
                EvaluateInvokationRules();
            }

            if (_invoked1 != Invoked1)
            {
                _invoked1 = Invoked1;
                EvaluateInvokationRules();
            }

            if (_invoked2 != Invoked2)
            {
                _invoked2 = Invoked2;
                EvaluateInvokationRules();
            }

            if (_invoked3 != Invoked3)
            {
                _invoked3 = Invoked3;
                EvaluateInvokationRules();
            }

            if (_invoked4 != Invoked4)
            {
                _invoked4 = Invoked4;
                EvaluateInvokationRules();
            }
        }

        public void SetToActive(GameObject cardObject)
        {
            if (ActiveCard != null)
                return;

            ActiveCard = cardObject;
            GameObject activeSlot = SlotsObject.transform.Find("ActiveSlot").gameObject;

            SnapCardToSlot(cardObject, activeSlot);
        }

        public void SetToInvoked(GameObject cardObject)
        {
            string slotStr;

            if (Invoked1 == null) { Invoked1 = cardObject; slotStr = "Slot1"; }
            else if (Invoked2 == null) { Invoked2 = cardObject; slotStr = "Slot2"; }
            else if (Invoked3 == null) { Invoked3 = cardObject; slotStr = "Slot3"; }
            else if (Invoked4 == null) { Invoked4 = cardObject; slotStr = "Slot4"; }
            else return;

            GameObject invokedSlot = SlotsObject.transform.Find(slotStr).gameObject;

            SnapCardToSlot(cardObject, invokedSlot);
        }

        public void DeselectActive()
        {
            ReleaseCardFromSlot(ActiveCard);

            if (Invoked1 != null)
            {
                ReleaseCardFromSlot(Invoked1);
                Invoked1.GetComponent<Card>().IsSelected = false;
                Invoked1 = null;
            }
            if (Invoked2 != null)
            {
                ReleaseCardFromSlot(Invoked2);
                Invoked2.GetComponent<Card>().IsSelected = false;
                Invoked2 = null;
            }
            if (Invoked3 != null)
            {
                ReleaseCardFromSlot(Invoked3);
                Invoked3.GetComponent<Card>().IsSelected = false;
                Invoked3 = null;
            }
            if (Invoked4 != null)
            {
                ReleaseCardFromSlot(Invoked4);
                Invoked4.GetComponent<Card>().IsSelected = false;
                Invoked4 = null;
            }

            ActiveCard = null;
        }

        public void DeselectInvoked(GameObject cardObject)
        {
            ReleaseCardFromSlot(cardObject);

            if (Invoked1 != null && Invoked1.Equals(cardObject)) { Invoked1 = null; }
            if (Invoked2 != null && Invoked2.Equals(cardObject)) { Invoked2 = null; }
            if (Invoked3 != null && Invoked3.Equals(cardObject)) { Invoked3 = null; }
            if (Invoked4 != null && Invoked4.Equals(cardObject)) { Invoked4 = null; }
        }

        public bool IsActiveCard(GameObject cardObject)
        {
            return ActiveCard != null &&
                cardObject.GetComponent<Card>().Equals(ActiveCard.GetComponent<Card>());
        }

        public void ConsumeCards(List<GameObject> currentCards)
        {
            List<GameObject> slots = new() { ActiveCard, Invoked1, Invoked2, Invoked3, Invoked4 };

            slots.ForEach
            (
                cardObject =>
                {
                    currentCards.Remove(cardObject);
                    Destroy(cardObject);
                }
            );

            ActiveCard = null;
            Invoked1 = null;
            Invoked2 = null;
            Invoked3 = null;
            Invoked4 = null;
            IsCardInvokable = false;

            SlotsObject.SetActive(false);
            State.IsCardInvokable = IsCardInvokable;
        }


        private void EvaluateInvokationRules()
        {
            if (ActiveCard == null)
            {
                IsCardInvokable = false;
                State.IsCardInvokable = false;
                return;
            }

            // TODO: For each rule in ActiveCard, check _rulesLookup by name and load from disk on miss
            // TODO: Implement rules correctly

            // For prototype only: count the cards in the discharge group
            Card card = ActiveCard.GetComponent<Card>();
            int countRequired = 0;
            int countCurrent = 0;

            if (card.InvokationRule1 != "") countRequired++;
            if (card.InvokationRule2 != "") countRequired++;
            if (card.InvokationRule3 != "") countRequired++;
            if (card.InvokationRule4 != "") countRequired++;

            if (Invoked1 != null) countCurrent++;
            if (Invoked2 != null) countCurrent++;
            if (Invoked3 != null) countCurrent++;
            if (Invoked4 != null) countCurrent++;

            IsCardInvokable = countCurrent == countRequired;
            State.IsCardInvokable = IsCardInvokable;
        }

        private void SnapCardToSlot(GameObject card, GameObject slot)
        {
            RectTransform cardCanvas = card.transform.Find("CardCanvas").GetComponent<RectTransform>();

            card.transform.SetParent(slot.transform, true);
            card.transform.localPosition = new(0, 0, 0);
            cardCanvas.pivot = new(0.5f, 0.5f);
            cardCanvas.anchoredPosition = new(0, 0);
        }

        private void ReleaseCardFromSlot(GameObject card)
        {
            RectTransform cardCanvas = card.transform.Find("CardCanvas").GetComponent<RectTransform>();

            cardCanvas.pivot = new(0, 0);
            cardCanvas.anchoredPosition = new(0, 0);

            card.transform.SetParent(GameObject.Find("CardsPanel").transform, true);
            card.transform.localPosition = new(0, 0, 0);

            card.GetComponent<Card>().SetToOrderPosition();
        }
    }
}
