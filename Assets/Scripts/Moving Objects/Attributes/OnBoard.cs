using UnityEngine;

public class OnBoard : MonoBehaviour
{
    public MovingObject movingObject = null;
    public Vector2 momentum = Vector2.zero;

    protected Rigidbody2D object_rb;

    public float epsilon = .01f;

    // Update is called once per frame
    void FixedUpdate()
    {
        TransformOnBoard();
    }

    void TransformOnBoard()
    {
        Vector2 next_pos = transform.position;

        /////////////////////////////////////////////////////////
        //Spin
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
        //Grav
        /////////////////////////////////////////////////////////
        // pos -= Ship.Instance.vel * Time.fixedDeltaTime;

        /////////////////////////////////////////////////////////
        //Vel
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

    void CollideWall(Collision2D collision)
    {
        bool[] touching = { false, false, false, false };

        foreach (ContactPoint2D contact in collision.contacts)
        {
            Vector2 normal = contact.normal;

            if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y))
            {
                if (normal.x > 0)
                {
                    //Debug.Log("Hit wall on LEFT side of player");
                    touching[0] = true;
                }
                else
                {
                    //Debug.Log("Hit wall on RIGHT side of player");
                    touching[0] = true;
                    touching[2] = true;
                }
            }
            else
            {
                if (normal.y > 0)
                {
                    //Debug.Log("Hit from BELOW (floor)");
                    touching[1] = true;
                }
                else
                {
                    //Debug.Log("Hit from ABOVE (ceiling)");
                    touching[1] = true;
                    touching[3] = true;
                }
            }
        }

        // Getting force vector due to spin
        Vector2 pos = transform.position;
        Vector2 radius = pos - Ship.Instance.center;
        Vector2 tangent = new Vector2(-radius.y, radius.x);
        Vector2 spin_force = tangent * -Ship.Instance.spin;
        ////// Total momentum of bumped-into wall
        Vector2 wal = Ship.Instance.vel + spin_force;

        if (movingObject != null) // Modify vel + Momentum
        {
            if (touching[0]) // Touching horizontally
            {
                if ((movingObject.vel.x < 0) == touching[2]) // Facing away from wall
                {
                    // If wall is moving AWAY, do nothing (skip response)
                    float relVelX = movingObject.vel.x + momentum.x - wal.x;
                    if (Mathf.Abs(relVelX) < epsilon || relVelX * (touching[2] ? -1 : 1) > 0)
                        return;

                    movingObject.vel.x = 0;
                    momentum.x = wal.x;
                }
                else // Facing wall
                {
                    if (touching[2]) // Facing Right wall
                    {
                        movingObject.vel.x = Mathf.Max(wal.x - momentum.x, 0);
                        momentum.x = Mathf.Min(momentum.x, wal.x);
                    }
                    else // Facing Left wall
                    {
                        movingObject.vel.x = Mathf.Min(wal.x - momentum.x, 0);
                        momentum.x = Mathf.Max(momentum.x, wal.x);
                    }
                }
            }

            if (touching[1])
            {
                if ((movingObject.vel.y < 0) == touching[3]) // Facing away from wall
                {
                    // If wall is moving AWAY, do nothing
                    float relVelY = movingObject.vel.y + momentum.y - wal.y;
                    if (Mathf.Abs(relVelY) < epsilon || relVelY * (touching[3] ? -1 : 1) > 0)
                        return;

                    movingObject.vel.y = 0;
                    momentum.y = wal.y;
                }
                else // Facing wall
                {
                    if (touching[3]) // Facing Up wall
                    {
                        movingObject.vel.y = Mathf.Max(wal.y - momentum.y, 0);
                        momentum.y = Mathf.Min(momentum.y, wal.y);
                    }
                    else // Facing Down wall
                    {
                        movingObject.vel.y = Mathf.Min(wal.y - momentum.y, 0);
                        momentum.y = Mathf.Max(momentum.y, wal.y);
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
                float relVelX = momentum.x - wal.x;
                if (Mathf.Abs(relVelX) < epsilon || relVelX * (touching[2] ? -1 : 1) > 0)
                    return;

                momentum.x = wal.x;
            }
            if (touching[1])
            {
                // If wall is moving AWAY, do nothing
                float relVelY = momentum.y - wal.y;
                if (Mathf.Abs(relVelY) < epsilon || relVelY * (touching[3] ? -1 : 1) > 0)
                    return;

                momentum.y = wal.y;
            }
        }
    }

}
