using System.Collections;
using UnityEngine;

public static class GameObjectExtensions
{
    public static GameObject GetFirstChild(this GameObject parent)
    {
        if (parent == null) return null; // Handle null parent

        Transform parentTransform = parent.transform;
        if (parentTransform.childCount == 0) return null; // No children

        return parentTransform.GetChild(0).gameObject;
    }

    public static GameObject AddChild(this GameObject parent, GameObject child)
    {
        if (parent == null || child == null)
        {
            Debug.LogError("Parent or child is null. Cannot add child.");
            return null;
        }

        child.transform.SetParent(parent.transform); // Set parent
        child.transform.localPosition = Vector3.zero; // Reset position to parent's origin
        child.transform.localRotation = Quaternion.identity; // Reset rotation
        child.transform.localScale = Vector3.one; // Reset scale

        return child;
    }

    public static IEnumerator DelayAction(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }
}
