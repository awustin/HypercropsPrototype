using Assets.Hypercrops.Common.Enums;

namespace Assets.Hypercrops.Model.LevelFeatures
{
    public class LevelFeature
    {
        public bool IsEnabled = false;
        public LevelFeatureType Type
        {
            get { return _type; }
        }

        private readonly LevelFeatureType _type;

        public LevelFeature(LevelFeatureType type, bool isEnabled)
        {
            _type = type;
            IsEnabled = isEnabled;
        }
    }
}