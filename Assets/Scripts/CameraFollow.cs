using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;    // The target that the camera will follow (your character)
    public Vector3 offset;      // The offset from the target's position

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target not assigned in CameraFollow script.");
            return;
        }

        // Set a default offset if not assigned
        if (offset == Vector3.zero)
        {
            offset = transform.position - target.position;
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Follow the target with the offset
            transform.position = target.position + offset;
        }
    }
}
