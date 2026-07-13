using UnityEngine;

// OnBoard is the foundation for on-ship physics
// Rather than moving the spaceship, objects inside that are given this script
// will move relative to the ship to create the illusion of the ship's
// movement. This avoids working with 2 moving colliders in Unity physics.
public class OnBoard_Projectile : OnBoard
{
    private Projectile Projectile;
    private float BounceFactor = 1f;

    private bool inContact;
    private float stuckDetector;
    private float escapedDetector;

    private void Awake()
    {
        Projectile = GetComponent<Projectile>();
        object_rb = GetComponent<Rigidbody2D>();
    }

    protected void FixedUpdate()
    {
        TransformOnBoard();
        TransformDenseMass();
        EnsureExistence();
    }

    private void EnsureExistence()
    {
        if (inContact)
        {
            stuckDetector += Time.fixedDeltaTime;
            escapedDetector = 0;
        }
        else
        {
            escapedDetector += Time.fixedDeltaTime;
            stuckDetector = 0;
        }

        if (stuckDetector > 1f)
        {
            Debug.Log("Projectile dectected as stuck in wall. Destroying...");
            Destroy(gameObject);
        }
        if (escapedDetector > 10f)
        {
            Debug.Log("Projectile detected as having escaped map. Destroying...");
            Destroy(gameObject);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            inContact = true;
            CollideWall(collision);
        }
        else
        {
            CollideOther(collision);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Walls")) inContact = false;
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
