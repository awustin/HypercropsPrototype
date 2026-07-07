using UnityEngine;
using Assets.Hypercrops.Common.Enums;

namespace Assets.Hypercrops.Model.Cards
{
#nullable enable
    public class InvokationRule
    {
        public CardType MatchType = CardType.Unknown;
        public CardRarity MatchRarity = CardRarity.Unknown;
        public int? MatchId = null;

        public bool AssertOn(GameObject cardObject)
        {
            Card card = cardObject.GetComponent<Card>();

            if (MatchId != null)
                return card.Id == MatchId;
            
            return card.Type == MatchType && card.Rarity == MatchRarity;
        }
    }
}
