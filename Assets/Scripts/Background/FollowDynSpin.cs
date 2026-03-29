using UnityEngine;

public class FollowDynSpin : MonoBehaviour
{
    void FixedUpdate()
    {
        float degrees = Ship.Instance.spin * Mathf.Rad2Deg * Time.fixedDeltaTime;
        Vector3 ship_center = new Vector3(Ship.Instance.center.x, Ship.Instance.center.y, 0);
        transform.RotateAround(ship_center, Vector3.forward, degrees);
    }
}
