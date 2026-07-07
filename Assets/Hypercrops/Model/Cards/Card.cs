using System;
using TMPro;
using UnityEngine;
using System.Collections;

using Assets.Hypercrops.Common.Enums;
using Assets.Hypercrops.Utils;

namespace Assets.Hypercrops.Model.Cards
{
    public class Card : MonoBehaviour, IEquatable<Card>
    {
        public string Label;
        public string CardName;
        public int Id;
        public int DeckId;
        public CardType Type;
        public CardRarity Rarity;
        public CardStatus Status;
        public int Order;
        public int Number;
        public bool IsSelected;

        #nullable enable
        public string? MapEntityName;
        public string? InvokationRule1;
        public string? InvokationRule2;
        public string? InvokationRule3;
        public string? InvokationRule4;
        #nullable disable

        void Start()
        {
            RectTransform CanvasRT = transform.Find("CardCanvas").GetComponent<RectTransform>();

            CanvasRT.localScale = Vector3.one;
            CanvasRT.sizeDelta = new Vector2(150, 200);

            // TODO: Card has to be drag and drop
        }

        public void InitialiseCard(
            int id,
            CardType type,
            CardRarity rarity,
            string cardName,
            string label,
            string mapEntityName
        )
        {
            CardName = $"Card:{cardName}";
            Id = id;
            Type = type;
            Rarity = rarity;
            Label = label;
            MapEntityName = mapEntityName;
            Number = Id;
            Status = CardStatus.FaceDown;
            IsSelected = false;

            SetName();
            SetNumber();
        }

        public IEnumerator FlipCard(int order)
        {
            if (Status == CardStatus.TurnUp)
                yield break;

            Status = CardStatus.TurnUp;
            gameObject.SetActive(false);

            yield return new WaitForSeconds(0.3f);

            SetOrder(order);
            gameObject.SetActive(true);
            Status = CardStatus.Idle;
        }

        public void SetDeckId(int value)
        {
            DeckId = value;
        }

        public void SetOrder(int order)
        {
            // TODO: animate
            Order = order;
            transform.localPosition = DeckUtils.MapOrderToPosition(Order);
        }

        public void SetToOrderPosition()
        {
            // TODO: animate
            transform.localPosition = DeckUtils.MapOrderToPosition(Order);
        }

        public void StartEffect()
        {
            // TODO: create map entity
        }

        public bool Equals(Card other)
        {
            return DeckId == other.DeckId;
        }

        public void Reset()
        {
            enabled = false;
            name = null;
            Id = 0;
            Label = null;
            MapEntityName = null;
            Number = 0;
            Status = CardStatus.FaceDown;
            CardName = null;
        }

        private void SetName()
        {
            TMP_Text NameLabel = transform.Find("CardCanvas/Traits/Name/Label").gameObject.GetComponent<TMP_Text>();

            NameLabel.text = Label;
        }

        private void SetNumber()
        {
            TMP_Text NumberLabel = transform.Find("CardCanvas/Traits/Number/Label").gameObject.GetComponent<TMP_Text>();

            NumberLabel.text = Number.ToString();
        }
    }
}
