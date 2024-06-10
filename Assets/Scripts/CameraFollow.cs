using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The player's transform
    public Vector3 offset = new Vector3(0f, 2f, -5f); // The offset from the player's position

    void LateUpdate()
    {
        if (target != null)
        {
            // Set the camera's position to be the target's position plus the offset
            transform.position = target.position + offset;
            
            // Make the camera look at the target
            transform.LookAt(target);
        }
    }
}
