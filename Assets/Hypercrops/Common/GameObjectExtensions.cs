using UnityEngine;

public static class GameObjectExtensions
{
    /// <summary>
    /// Retrieves the first GameObject child of a given GameObject.
    /// </summary>
    /// <param name="parent">The parent GameObject.</param>
    /// <returns>The first child GameObject, or null if no children exist.</returns>
    public static GameObject GetFirstChild(this GameObject parent)
    {
        if (parent == null) return null; // Handle null parent

        Transform parentTransform = parent.transform;
        if (parentTransform.childCount == 0) return null; // No children

        return parentTransform.GetChild(0).gameObject;
    }

    /// <summary>
    /// Adds a child GameObject to a parent GameObject.
    /// </summary>
    /// <param name="parent">The parent GameObject.</param>
    /// <param name="child">The GameObject to add as a child.</param>
    /// <returns>The child GameObject, or null if parent or child is null.</returns>
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
}
