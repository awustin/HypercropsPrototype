using System.Collections.Generic;
using UnityEngine;

using FType = Assets.Hypercrops.Common.Enums.LevelFeatureType;

namespace Assets.Hypercrops.Model.LevelFeatures
{
    public class LevelFeaturesManager : MonoBehaviour
    {

        private Dictionary<FType, LevelFeature> _features = new ();

        void Start()
        {
            LevelFeature lf = new (FType.CollectHarvest, false);
        
            _features.Add(FType.CollectHarvest, lf);
        }
    }
}
