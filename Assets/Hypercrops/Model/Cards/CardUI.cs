using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Hypercrops.Common.Enums;
using Assets.Hypercrops.Common.Serializables;
using Assets.Hypercrops.Events;
using Assets.Hypercrops.System;
using Assets.Hypercrops.State;

namespace Assets.Hypercrops.Model.Cards
{
    public class CardUI : GameObjectUIBehaviour, IPointerClickHandler
    {
        public Card CardScript;
        public GameEventSender Sender;

        public void Start()
        {
            if (Sender == null)
                Sender = GameEventSender.Instance;
        }

        override public void OnPointerEnter(PointerEventData data)
        {
            base.OnPointerEnter(data);
            // TODO: Animate card
        }

        public void OnPointerClick_DEPRECATED(PointerEventData data)
        {
            GameObject card = data.pointerPress;
            Card CardScript = card.GetComponent<Card>();
            CardType type = CardScript.Type;

            State.SetLastCardSelected(card);

            if (type == CardType.Crop)
            {
                string speciesName = CardScript.MapEntityName;

                CropDescriptor cropDescriptor = ObjectFactory.Instance.GetCropDescriptorBySpeciesName(speciesName);
                GameEventSender.Instance.BroadcastEvent("StartFarmMode", new Vector3(0, 0, 0), cropDescriptor);

                return;
            }

            if (type == CardType.Buildable)
            {
                string buildingType = CardScript.MapEntityName;

                BuildableDescriptor descriptor = ObjectFactory.Instance.GetBuildableDescriptorByType(buildingType);
                GameEventSender.Instance.BroadcastEvent("StartBuildMode", descriptor);

                return;
            }

            if (type == CardType.Tech)
            {
                // TODO: Apply effect
                return;
            }
        }

        public void OnPointerClick(PointerEventData data)
        {
            Sender.BroadcastEvent(CardScript.IsSelected ? "DeselectCard" : "SelectCard", gameObject);
        }
    }
}
