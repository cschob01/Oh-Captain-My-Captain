using UnityEngine;

// MovingObject
// This is the abstract class for objects containing/managing their own
// velocity vector. This is the foundation for player/enemy movement.
public abstract class MovingObject : MonoBehaviour
{
    public float acc = .04f;
    public float max_vel = 3;

    public Vector2 vel { get; private set; } = Vector2.zero;
    protected Rigidbody2D object_rb;
    public GameObject render;

    protected virtual void Awake()
    {
        object_rb = GetComponent<Rigidbody2D>();
    }

    public void ModifyVel(Vector2 new_vel)
    {
        vel = Vector2.ClampMagnitude(new_vel, max_vel);
    }

    private void FixedUpdate()
    {
        // Get vel from child class
        vel += acc * GetDir().normalized;
        vel = Vector2.ClampMagnitude(vel, max_vel);

        // Apply vel so that it works in the Unity physics system
        // object_rb.linearVelocity = vel; // LEAVE TO ONBOARD

        // If a render component is given, rotate it to face the vel
        if (render != null)
        {
            render.transform.rotation = Quaternion.FromToRotation(Vector2.right, vel);
        }
    }

    protected abstract Vector2 GetDir();
}