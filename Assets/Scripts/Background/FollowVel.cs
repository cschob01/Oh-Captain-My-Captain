using UnityEngine;

public class FollowVel : MonoBehaviour
{
    public float follow_speed = .15f;

    // Update is called once per frame
    void Update()
    {
        Vector3 vel = transform.position;
        vel.x -= Ship.Instance.vel.x * Time.fixedDeltaTime * follow_speed;
        vel.y -= Ship.Instance.vel.y * Time.fixedDeltaTime * follow_speed;
        transform.position = vel;
    }
}
