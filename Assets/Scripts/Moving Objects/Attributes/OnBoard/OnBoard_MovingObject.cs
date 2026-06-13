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
        TransformDenseMass();

        //Rotate vel to follow ship's spin
        float cos = Mathf.Cos(Ship.Instance.spin * Time.fixedDeltaTime);
        float sin = Mathf.Sin(Ship.Instance.spin * Time.fixedDeltaTime);
        movingObject.ModifyVel(new Vector2(
            movingObject.vel.x * cos - movingObject.vel.y * sin,
            movingObject.vel.x * sin + movingObject.vel.y * cos
        ));

        object_rb.linearVelocity += movingObject.vel; // Add vel to linearVelocity
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
        foreach (ContactPoint2D contact in collision.contacts)
        {
            Vector2 wall_force = GetWallForce(contact);
            Vector2 mom_force = ProjectOnto(momentum, contact.normal);
            Vector2 vel_force = ProjectOnto(movingObject.vel, contact.normal);

            if (Vector2.Dot(wall_force - (mom_force + vel_force), contact.normal) < .001f) // opposite directions
            {
                if (testing)
                {
                    Debug.Log(
                        $"Virtually no impact. Ignoring\n"
                    );
                    if (Vector2.Dot(wall_force - (mom_force + vel_force), contact.normal) < -.1f)
                    {
                        Debug.Log($"ERROR: Object is pushing significantly incorrectly\n");
                    }
                }
            }
            else if(Vector2.Dot(wall_force, contact.normal) > Vector2.Dot(mom_force, contact.normal))
            {
                if (testing)
                {
                    Debug.Log(
                        $"Pre-Wall Collision true impact\n" +
                        $"Normal: {contact.normal}\n" +
                        $"Wall Force: {wall_force}\n" +
                        $"Vel: {movingObject.vel}\n" +
                        $"Momentum: {momentum}\n" +
                        $"Initial Total Motion: {movingObject.vel + momentum}"
                    );
                }

                movingObject.ModifyVel(ProjectOnto(movingObject.vel, Vector2.Perpendicular(contact.normal)));
                momentum = ProjectOnto(momentum, Vector2.Perpendicular(contact.normal));
                momentum += wall_force;

                if (testing)
                {
                    Debug.Log(
                        $"Post-Wall Collision true impact\n" +
                        $"Normal: {contact.normal}\n" +
                        $"Wall Force: {wall_force}\n" +
                        $"Vel: {movingObject.vel}\n" +
                        $"Momentum: {momentum}\n" +
                        $"Final Total Motion: {movingObject.vel + momentum}"
                    );
                }
            }
            else
            {
                if (testing)
                {
                    Debug.Log(
                        $"Pre-Wall Collision false impact\n" +
                        $"Normal: {contact.normal}\n" +
                        $"Wall Force: {wall_force}\n" +
                        $"Vel: {movingObject.vel}\n" +
                        $"Momentum: {momentum}\n" +
                        $"Initial Total Motion: {movingObject.vel + momentum}"
                    );
                }

                Vector2 new_vel = ProjectOnto(movingObject.vel, Vector2.Perpendicular(contact.normal)) +
                                        (wall_force - mom_force);

                if (new_vel.magnitude <= movingObject.vel.magnitude)
                {
                    movingObject.ModifyVel(new_vel);
                }
                else
                {
                    if (testing)
                    {
                        Debug.Log("ERROR: False collision preoduced new velocity greater than original");
                    }
                }

                if (testing)
                {
                    Debug.Log(
                        $"Post-Wall Collision false impact\n" +
                        $"Normal: {contact.normal}\n" +
                        $"Wall Force: {wall_force}\n" +
                        $"Vel: {movingObject.vel}\n" +
                        $"Momentum: {momentum}\n" +
                        $"Final Total Motion: {movingObject.vel + momentum}"
                    );
                }
            }
        }

    }

}
