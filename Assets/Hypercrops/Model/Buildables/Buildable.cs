using UnityEngine;

using Assets.Hypercrops.Common.Enums;

namespace Assets.Hypercrops.Model.Buildables
{
    public class Buildable : MonoBehaviour
    {
        public BuildableType Type;
        public BuildableInteractionType InteractionType;
        public string Description;

        public void Initialise(BuildableType type, BuildableInteractionType interactionType, string description)
        {
            Type = type;
            InteractionType = interactionType;
            Description = description;
        }
    }
}
