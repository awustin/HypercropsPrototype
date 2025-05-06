using UnityEngine;

using Assets.Hypercrops.Model.Ghosts;
using Assets.Hypercrops.Common.Enums;

namespace Assets.Hypercrops.Model.Buildables
{
    // TODO: Never destroy
    // TODO: Import BuildablesManager
    public class BuildableGhost : Ghost
    {
        public BuildablesManager Buildables;

        protected override void StartDerived()
        {
            Buildables = BuildablesManager.Instance;
        }

        public void Activate(BuildableLayoutType layoutType)
        {
            Activate<BuildableGhost>(layoutType.ToString());
            ActionLayer = LayerMask.GetMask("Default");
        }

        protected override bool IsPointActionable(Vector3 target)
        {
            return Buildables.IsBuildablePoint(target);
        }
    }
}