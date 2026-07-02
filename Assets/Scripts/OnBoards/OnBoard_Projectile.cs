using UnityEngine;

// OnBoard is the foundation for on-ship physics
// Rather than moving the spaceship, objects inside that are given this script
// will move relative to the ship to create the illusion of the ship's
// movement. This avoids working with 2 moving colliders in Unity physics.
public class OnBoard_Projectile : OnBoard
{
    private Projectile Projectile;
    private float BounceFactor = 1f;

    private void Awake()
    {
        Projectile = GetComponent<Projectile>();
        object_rb = GetComponent<Rigidbody2D>();
    }

    protected void FixedUpdate()
    {
        TransformOnBoard();
        TransformDenseMass();
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
        foreach (ContactPoint2D contact in collision.contacts)
        {
            Vector2 wall_force = GetWallForce(contact);
            Vector2 mo_force = ProjectOnto(momentum, contact.normal);

            if (Vector2.Dot(wall_force - mo_force, contact.normal) > -.001f)
            {
                momentum = ProjectOnto(momentum, Vector2.Perpendicular(contact.normal))
                            + wall_force 
                            + (wall_force - ProjectOnto(momentum, contact.normal) * BounceFactor);

                //transform.position += (Vector3)contact.normal * .04f;
            }
        }
    }

}
