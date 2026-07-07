using UnityEngine;

using Assets.Hypercrops.Common.Enums;

namespace Assets.Hypercrops.Model.Buildables
{
    public class Buildable : MonoBehaviour
    {
        public BuildableType Type;
        public string Description;

        public void Initialise(BuildableType type, string description)
        {
            Type = type;
            Description = description;
        }
    }
}
