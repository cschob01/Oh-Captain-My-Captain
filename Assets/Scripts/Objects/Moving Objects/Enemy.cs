using UnityEngine;
using UnityEngine.InputSystem;

public class Enemy : MovingObject
{
    public GameObject objective;

    public float acc = .04f;
    public float max_vel = 3;

    // Update is called once per frame
    protected override void SetVel()
    {
        Vector2 pos = transform.position;
        Vector2 target = objective.transform.position;
        Vector2 dir = (target - pos).normalized;
        vel += acc * dir;

        //Rotate velocity
        float cos = Mathf.Cos(Ship.Instance.spin * Time.fixedDeltaTime);
        float sin = Mathf.Sin(Ship.Instance.spin * Time.fixedDeltaTime);
        vel = new Vector2(
            vel.x * cos - vel.y * sin,
            vel.x * sin + vel.y * cos
        );
        vel = Vector2.ClampMagnitude(vel, max_vel);
    }
}
