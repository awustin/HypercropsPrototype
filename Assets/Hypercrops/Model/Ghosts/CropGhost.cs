using UnityEngine;

using Assets.Hypercrops.Common.Enums;
using Assets.Hypercrops.Model.Crops;

namespace Assets.Hypercrops.Model.Ghosts
{
    // TODO: Fix ghosts prefabs to match base structure
    public class CropGhost : Ghost
    {
        public FarmManager Farm;

        protected override void StartDerived()
        {
            Farm = FarmManager.Instance;
        }

        public void Activate(CropSize cropSize)
        {
            Activate<CropGhost>(cropSize.ToString());
            ActionLayer = LayerMask.GetMask("Default");
        }

        protected override bool IsPointActionable(Vector3 target)
        {
            return Farm.IsPlantablePoint(target);
        }
    }
}
