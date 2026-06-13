using TMPro;
using UnityEngine;
using static UnityEditor.PlayerSettings;

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

    [SerializeField] protected bool testing;

    private void OnEnable()
    {
        EventHandler.Instance.OnExplosion += OnExplosion;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnExplosion -= OnExplosion;
    }

    private void OnExplosion(Vector2 pos, float force, float radius)
    {
        Vector2 dif = (Vector2)transform.position - pos;
        float dist = dif.magnitude;
        if (dist <= radius)
        {
            if (dist == 0f)
            {
                Debug.Log("Object is positioned EXACTLY on explosion");
                return;
            }
            float power = force * (radius - dist) / radius;
            momentum += dif.normalized * power;
        }
    }

    protected void TransformDenseMass()
    {
        foreach (DenseMass mass in Ship.Instance.denseMasses)
        {
            OnExplosion(mass.transform.position, -mass.acceleration * Time.fixedDeltaTime, mass.radius);
        }
    }


    protected void TransformOnBoard() // Moves object according to ship's movement
    {
        Vector2 next_pos = transform.position;

        /////////////////////////////////////////////////////////
        //Spin: Rotate object around ship center
        /////////////////////////////////////////////////////////
        float angle = Ship.Instance.spin * Time.fixedDeltaTime;
        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);
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
        next_pos += (momentum - Ship.Instance.vel) * Time.fixedDeltaTime;

        object_rb.linearVelocity = (next_pos - object_rb.position) / Time.fixedDeltaTime;
    }

    protected Vector2 GetWallForce(ContactPoint2D contact)
    {
        // Getting force vector due to spin
        Vector2 pos = transform.position;
        Vector2 radius = pos - Ship.Instance.center;
        Vector2 tangent = new Vector2(-radius.y, radius.x);
        Vector2 spin_force = tangent * -Ship.Instance.spin;
        ////// Total momentum of bumped-into wall
        Vector2 wall_force = Ship.Instance.vel + spin_force;

        if (testing)
        {
            Debug.Log(
                $"Thrust Force: {Ship.Instance.vel}\n" +
                $"SpinForce: {spin_force}\n" +
                $"Normal: {contact.normal}\n" +
                $"Total After Projection: {ProjectOnto(wall_force, contact.normal)}"
            );
        }

        return ProjectOnto(wall_force, contact.normal);
    }

    protected Vector2 ProjectOnto(Vector2 vector, Vector2 onto)
    {
        return Vector2.Dot(vector, onto) / onto.sqrMagnitude * onto;
    }

}
