using UnityEngine;

namespace Assets.Hypercrops.Model.Utils
{
    public static class HypercropsModelUtils
    {
        public static Vector3 SnapPoint(Vector3 point)
        {
            return new Vector3(
                Mathf.Round(point.x / 0.5f) * 0.5f,
                Mathf.Round(point.y / 0.5f) * 0.5f,
                Mathf.Round(point.z / 0.5f) * 0.5f
            );
        }

        public static string PositionToKey(Vector3 position)
        {
            return $"{position.x}-{position.y}-{position.z}";
        }
    }
}