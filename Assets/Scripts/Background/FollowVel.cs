using UnityEngine;

public class FollowVel : MonoBehaviour
{
    public float follow_speed = .15f;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 pos = transform.position;
        pos.x -= Ship.Instance.vel.x * Time.fixedDeltaTime * follow_speed;
        pos.y -= Ship.Instance.vel.y * Time.fixedDeltaTime * follow_speed;
        transform.position = pos;
    }
}
