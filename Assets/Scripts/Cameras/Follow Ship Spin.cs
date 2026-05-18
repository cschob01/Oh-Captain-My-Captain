using UnityEngine;

public class FollowShipSpin : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.Rotate(0f, 0f, Ship.Instance.spin * Time.fixedDeltaTime);
    }
}
