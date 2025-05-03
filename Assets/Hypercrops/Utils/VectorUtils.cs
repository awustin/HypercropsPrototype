using UnityEngine;

public static class VectorUtils
{
    public static bool IsInSphere(Vector3 point, Vector3 center, float radius)
    {
        float distance = Vector3.Distance(point, center);
        return distance <= radius;
    }

    public static Vector3 AddOnY(Vector3 point, float value)
    {
        return point + new Vector3(0, value, 0);
    }
}