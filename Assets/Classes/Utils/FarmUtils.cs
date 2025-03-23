using UnityEngine;

public static class FarmUtils
{
    public static Vector3 SnapPoint(Vector3 point)
    {
        return new Vector3(
            Mathf.Round(point.x / 2f) * 2f,
            Mathf.Round(point.y / 2f) * 2f,
            Mathf.Round(point.z / 2f) * 2f
        );
    }

    public static bool IsPlantablePoint(Vector3 target, GameObject player, GameObject targetObject, float radius)
    {
        // Check available space and generate new position
        // Do plantable checks. Add a check for when a location is already planted
        if (target.y > 0.1 || !targetObject.CompareTag("Ground"))
        {
            return false;
        }

        if (!VectorUtils.IsInSphere(target, player.transform.position, radius))
        {
            return false;
        }

        return true;
    }

    public static string PositionToKey(Vector3 position)
    {
        return $"{position.x}-{position.y}-{position.z}";
    }
}