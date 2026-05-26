using Unity.VisualScripting;
using UnityEngine;

// OnBoard is the foundation for on-ship physics
// Rather than moving the spaceship, objects inside that are given this script
// will move relative to the ship to create the illusion of the ship's
// movement. This avoids working with 2 moving colliders in Unity physics.
public class OnBoard_MovingObject : OnBoard
{
    public MovingObject movingObject {get; private set;}

    private void Awake()
    {
        object_rb = GetComponent<Rigidbody2D>();
        movingObject = GetComponent<MovingObject>();
    }

    protected void FixedUpdate()
    {
        TransformOnBoard();

        //Rotate vel to follow ship's spin
        float cos = Mathf.Cos(Ship.Instance.spin * Time.fixedDeltaTime);
        float sin = Mathf.Sin(Ship.Instance.spin * Time.fixedDeltaTime);
        movingObject.vel = new Vector2(
            movingObject.vel.x * cos - movingObject.vel.y * sin,
            movingObject.vel.x * sin + movingObject.vel.y * cos
        );

        object_rb.linearVelocity += movingObject.vel; // Add vel to linearVelocity
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log(transform.gameObject + " hit " + collision.gameObject);

        if (collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            CollideWall(collision);
        }
        else
        {
            CollideOther(collision);
        }
    }

    void CollideOther(Collision2D collision)
    {

    }

    // Colliding with wall will result in a change of the objects's momentum
    // and velocity (if applicable).
    void CollideWall(Collision2D collision)
    {
        bool[] touching = GetContactPoints(collision);

        // Getting force vector due to spin
        Vector2 pos = transform.position;
        Vector2 radius = pos - Ship.Instance.center;
        Vector2 tangent = new Vector2(-radius.y, radius.x);
        Vector2 spin_force = tangent * -Ship.Instance.spin;
        ////// Total momentum of bumped-into wall
        Vector2 wall_force = Ship.Instance.vel + spin_force;

        if (touching[0]) // Touching horizontally
        {
            if ((movingObject.vel.x < 0) == touching[2]) // Facing away from wall
            {
                // If wall is moving AWAY, do nothing (skip response)
                float relVelX = movingObject.vel.x + momentum.x - wall_force.x;
                if (Mathf.Abs(relVelX) < epsilon || relVelX * (touching[2] ? -1 : 1) > 0)
                    return;

                movingObject.vel.x = 0;
                momentum.x = wall_force.x;
            }
            else // Facing wall
            {
                if (touching[2]) // Facing Right wall
                {
                    float new_vel = Mathf.Min(wall_force.x - momentum.x, movingObject.vel.x);
                    movingObject.vel.x = Mathf.Max(new_vel, 0);
                    momentum.x = Mathf.Min(momentum.x, wall_force.x);
                }
                else // Facing Left wall
                {
                    float new_vel = Mathf.Max(wall_force.x - momentum.x, movingObject.vel.x);
                    movingObject.vel.x = Mathf.Min(new_vel, 0);
                    momentum.x = Mathf.Max(momentum.x, wall_force.x);
                }
            }
        }

        if (touching[1]) // Vertically touching
        {
            if ((movingObject.vel.y < 0) == touching[3]) // Facing away from wall
            {
                // If wall is moving AWAY, do nothing
                float relVelY = movingObject.vel.y + momentum.y - wall_force.y;
                if (Mathf.Abs(relVelY) < epsilon || relVelY * (touching[3] ? -1 : 1) > 0)
                    return;

                movingObject.vel.y = 0;
                momentum.y = wall_force.y;
            }
            else // Facing wall
            {
                if (touching[3]) // Facing Up wall
                {
                    float new_vel = Mathf.Min(wall_force.y - momentum.y, movingObject.vel.y);
                    movingObject.vel.y = Mathf.Max(new_vel, 0);
                    momentum.y = Mathf.Min(momentum.y, wall_force.y);
                }
                else // Facing Down wall
                {
                    float new_vel = Mathf.Max(wall_force.y - momentum.y, movingObject.vel.y);
                    movingObject.vel.y = Mathf.Min(new_vel, 0);
                    momentum.y = Mathf.Max(momentum.y, wall_force.y);
                }
            }
        }

    }

}
