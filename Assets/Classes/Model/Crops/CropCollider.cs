using UnityEngine;

public class CropCollider : MonoBehaviour
{
    // TODO: Implement collider functionality. Try capsule collider based on CropSize.
    public GameObject ColliderObject;
    private CapsuleCollider _capsuleCollider;

    public void Initialise(CropSize size)
    {
        _capsuleCollider = ColliderObject.GetComponent<CapsuleCollider>();

        UpdateColliderSize(size);
    }

    public void UpdateColliderSize(CropSize size)
    {
        float radius = (int) size;

        _capsuleCollider.radius = radius;
    }
}
