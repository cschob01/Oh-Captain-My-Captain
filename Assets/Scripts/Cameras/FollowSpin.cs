using UnityEngine;

public class FollowSpin : MonoBehaviour
{

    // Update is called once per frame
    void FixedUpdate()
    {
        float degrees = Ship.Instance.spin * Mathf.Rad2Deg * Time.fixedDeltaTime;
        transform.Rotate(0, 0, degrees);
    }
}
