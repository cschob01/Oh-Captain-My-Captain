using Unity.VisualScripting;
using UnityEngine;

// OnBoard is the foundation for on-ship physics
// Rather than moving the spaceship, objects inside that are given this script
// will move relative to the ship to create the illusion of the ship's
// movement. This avoids working with 2 moving colliders in Unity physics.
public class OnBoard : MonoBehaviour
{
    public MovingObject movingObject = null;

    // Momentum will be used to simulate applied forces on the object
    public Vector2 momentum = Vector2.zero;

    protected Rigidbody2D object_rb;

    private float epsilon = .01f; 

    void FixedUpdate()
    {
        TransformOnBoard();
    }

    void TransformOnBoard() // Moves object according to ship's movement
    {
        Vector2 next_pos = transform.position;

        /////////////////////////////////////////////////////////
        //Spin: Rotate object around ship center
        /////////////////////////////////////////////////////////
        float cos = Mathf.Cos(Ship.Instance.spin * Time.fixedDeltaTime);
        float sin = Mathf.Sin(Ship.Instance.spin * Time.fixedDeltaTime);
        next_pos -= Ship.Instance.center;    // Get relative to center
        next_pos = new Vector2(              // Rotate
            next_pos.x * cos - next_pos.y * sin,
            next_pos.x * sin + next_pos.y * cos
        );
        next_pos += Ship.Instance.center;    // Bring back

        // Rotate momentum to compensate for spin
        momentum = new Vector2(
            momentum.x * cos - momentum.y * sin,
            momentum.x * sin + momentum.y * cos
        );

        // Rotate vel to compensate for spin
        if (movingObject != null)
        {
            movingObject.vel = new Vector2(
                movingObject.vel.x * cos - movingObject.vel.y * sin,
                movingObject.vel.x * sin + movingObject.vel.y * cos
            );
        }

        /////////////////////////////////////////////////////////
        //Vel: Move object according to ship's velocity
        /////////////////////////////////////////////////////////
        next_pos += (momentum - Ship.Instance.vel) * Time.fixedDeltaTime;

        transform.position = next_pos;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
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

        if (movingObject != null) // Modify vel + Momentum
        {
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
                        movingObject.vel.x = Mathf.Max(wall_force.x - momentum.x, 0);
                        momentum.x = Mathf.Min(momentum.x, wall_force.x);
                    }
                    else // Facing Left wall
                    {
                        movingObject.vel.x = Mathf.Min(wall_force.x - momentum.x, 0);
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
                        movingObject.vel.y = Mathf.Max(wall_force.y - momentum.y, 0);
                        momentum.y = Mathf.Min(momentum.y, wall_force.y);
                    }
                    else // Facing Down wall
                    {
                        movingObject.vel.y = Mathf.Min(wall_force.y - momentum.y, 0);
                        momentum.y = Mathf.Max(momentum.y, wall_force.y);
                    }

                }
            }
        }
        else
        {
            // Idle objects simply pushed by ship
            if (touching[0])
            {
                // If wall is moving AWAY, do nothing
                float relVelX = momentum.x - wall_force.x;
                if (Mathf.Abs(relVelX) < epsilon || relVelX * (touching[2] ? -1 : 1) > 0)
                    return;

                momentum.x = wall_force.x;
            }
            if (touching[1])
            {
                // If wall is moving AWAY, do nothing
                float relVelY = momentum.y - wall_force.y;
                if (Mathf.Abs(relVelY) < epsilon || relVelY * (touching[3] ? -1 : 1) > 0)
                    return;

                momentum.y = wall_force.y;
            }
        }
    }

    bool[] GetContactPoints(Collision2D collision)
    {
        bool[] touching = { false, false, false, false };

        //
        foreach (ContactPoint2D contact in collision.contacts)
        {
            Vector2 normal = contact.normal;

            if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y))
            {
                if (normal.x > 0)
                {
                    touching[0] = true;
                }
                else
                {
                    touching[0] = true;
                    touching[2] = true;
                }
            }
            else
            {
                if (normal.y > 0)
                {
                    touching[1] = true;
                }
                else
                {
                    touching[1] = true;
                    touching[3] = true;
                }
            }
        }

        return touching;
    }

}
