using Unity.VisualScripting;
using UnityEngine;

// OnBoard is the foundation for on-ship physics
// Rather than moving the spaceship, objects inside that are given this script
// will move relative to the ship to create the illusion of the ship's
// movement. This avoids working with 2 moving colliders in Unity physics.
public class OnBoard_Projectile : OnBoard
{
    private Projectile Projectile;
    private float BounceFactor = 1f;
    private bool[] PreviouslyTouching = new bool[2];

    private void Awake()
    {
        Projectile = GetComponent<Projectile>();
        object_rb = GetComponent<Rigidbody2D>();
    }

    protected void FixedUpdate()
    {
        TransformOnBoard();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (Projectile.BouncesLeft == 0)
        {
            Destroy(gameObject);
        }
        else
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
            {
                CollideWall(collision);
                Projectile.BouncesLeft--;
            }
            else
            {
                CollideOther(collision);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        PreviouslyTouching = new bool[2];
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

        // Idle objects simply pushed by ship
        //if (touching[0])
        //{
        //    // If wall is moving AWAY, do nothing
        //    float relVelX = momentum.x - wall_force.x;
        //    if (Mathf.Abs(relVelX) < epsilon || relVelX * (touching[2] ? -1 : 1) > 0)
        //        return;

        //    momentum.x = wall_force.x;
        //}
        //if (touching[1])
        //{
        //    // If wall is moving AWAY, do nothing
        //    float relVelY = momentum.y - wall_force.y;
        //    if (Mathf.Abs(relVelY) < epsilon || relVelY * (touching[3] ? -1 : 1) > 0)
        //        return;

        //    momentum.y = wall_force.y;
        //}

        if (touching[0] && !PreviouslyTouching[0])
        {
            momentum.x = wall_force.x + (wall_force.x - momentum.x * BounceFactor);
            PreviouslyTouching[0] = true;
        }
        if (touching[1] && !PreviouslyTouching[1])
        {
            momentum.y = wall_force.y + (wall_force.y - momentum.y * BounceFactor);
            PreviouslyTouching[1] = true;
        }

        foreach (ContactPoint2D contact in collision.contacts)
        {
            transform.position += (Vector3)contact.normal * .04f;
        }
    }

}
