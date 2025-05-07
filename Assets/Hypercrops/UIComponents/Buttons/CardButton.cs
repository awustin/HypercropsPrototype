using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Hypercrops.Common.Enums;
using Assets.Hypercrops.Common.Serializables;
using Assets.Hypercrops.Events;
using Assets.Hypercrops.System;
using Assets.Hypercrops.Model.Cards;

public class CardButton : UIElementBehaviour, IPointerClickHandler
{
    override public void OnPointerEnter(PointerEventData data)
    {
        base.OnPointerEnter(data);
        // TODO: Animate card
    }

    public void OnPointerClick(PointerEventData data)
    {
        GameObject card = data.pointerPress;
        Card CardScript = card.GetComponent<Card>();
        CardType type = CardScript.Type;

        State.SetLastCardSelected(card);

        if (type == CardType.Crop)
        {
            string speciesName = CardScript.Attribute;

            CropDescriptor cropDescriptor = ObjectFactory.Instance.GetCropDescriptorBySpeciesName(speciesName);
            GameEventSender.Instance.BroadcastStartFarmMode(new Vector3(0, 0, 0), cropDescriptor);

            return;
        }

        if (type == CardType.Buildable)
        {
            string buildingType = CardScript.Attribute;

            BuildableDescriptor descriptor = ObjectFactory.Instance.GetBuildableDescriptorByType(buildingType);
            GameEventSender.Instance.BroadcastStartBuildMode(descriptor);

            return;
        }
        
        if (type == CardType.Tech)
        {
            // TODO: Apply effect
            return;
        }
    }
}
