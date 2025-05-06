using UnityEngine;

using Assets.Hypercrops.Common.Enums;

public class CropCollider : MonoBehaviour
{
    // TODO: Implement collider functionality. Try capsule collider based on CropSize.
    public GameObject ColliderObject;
    private BoxCollider _current;

    public void Initialise(CropSize size)
    {
        _current = ColliderObject.GetComponent<BoxCollider>();

        UpdateColliderSize(size);
    }

    public void UpdateColliderSize(CropSize size)
    {
        float l = (int) size;

        _current.size = new Vector3(l, 0.25f, l);
    }
}
