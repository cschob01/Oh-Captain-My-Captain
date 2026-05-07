using UnityEngine;

public class FollowSpin : MonoBehaviour
{
    void FixedUpdate()
    {
        // Keep object at angle of ship (Creates illusion of ship spining)
        transform.rotation = Quaternion.Euler(0f, 0f, Ship.Instance.global_angle);
    }
}
