using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Hypercrops.Model.Cards;

public class CardsDeckButton : MonoBehaviour, IPointerClickHandler
{
    public CardsManager Cards;

    void Start()
    {
        Cards = CardsManager.Instance;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Cards.DrawCard();
    }
}
