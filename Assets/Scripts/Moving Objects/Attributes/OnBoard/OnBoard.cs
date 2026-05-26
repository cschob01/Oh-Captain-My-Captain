using Unity.VisualScripting;
using UnityEngine;

// OnBoard is the foundation for on-ship physics
// Rather than moving the spaceship, objects inside that are given this script
// will move relative to the ship to create the illusion of the ship's
// movement. This avoids working with 2 moving colliders in Unity physics.
public class OnBoard : MonoBehaviour
{

    // Momentum will be used to simulate applied forces on the object
    public Vector2 momentum = Vector2.zero;

    protected Rigidbody2D object_rb;

    protected float epsilon = .01f;

    protected void TransformOnBoard() // Moves object according to ship's movement
    {
        Vector2 next_pos = transform.position;

        /////////////////////////////////////////////////////////
        //Spin: Rotate object around ship center
        /////////////////////////////////////////////////////////
        float cos = Mathf.Cos(Ship.Instance.spin);
        float sin = Mathf.Sin(Ship.Instance.spin);
        next_pos -= Ship.Instance.center;    // Get relative to center
        next_pos = new Vector2(              // Rotate
            next_pos.x * cos - next_pos.y * sin,
            next_pos.x * sin + next_pos.y * cos
        );
        next_pos += Ship.Instance.center;    // Bring back

        // Rotate momentum to compensate for spin
        cos = Mathf.Cos(Ship.Instance.spin * Time.fixedDeltaTime);
        sin = Mathf.Sin(Ship.Instance.spin * Time.fixedDeltaTime);
        momentum = new Vector2(
            momentum.x * cos - momentum.y * sin,
            momentum.x * sin + momentum.y * cos
        );

        /////////////////////////////////////////////////////////
        //Vel: Move object according to ship's velocity
        /////////////////////////////////////////////////////////
        next_pos += (momentum - Ship.Instance.vel);


        //transform.position = next_pos;

        Debug.Log(transform.gameObject);
        object_rb.linearVelocity = next_pos - (Vector2)transform.position;
    }

    protected bool[] GetContactPoints(Collision2D collision)
    {
        bool[] touching = { false, false, false, false };

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
