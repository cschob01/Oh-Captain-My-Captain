using UnityEngine;

// This enemy simply moves towards the player at all times, ignorant
// of walls and map layout
public class SimpleEnemy : MovingObject
{
    public GameObject objective;

    public float acc = .04f;
    public float max_vel = 3;

    // Called once every fixedUpdate
    protected override void SetVel()
    {
        Vector2 pos = transform.position;
        Vector2 target = objective.transform.position;
        Vector2 dir = (target - pos).normalized;
        vel += acc * dir;

        ////Clamp velocity
        vel = Vector2.ClampMagnitude(vel, max_vel);
    }
}
