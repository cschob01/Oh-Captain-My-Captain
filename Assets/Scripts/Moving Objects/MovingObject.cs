using UnityEngine;

// MovingObject
// This is the abstract class for objects containing/managing their own
// velocity vector. This is the foundation for player/enemy movement.
public abstract class MovingObject : MonoBehaviour
{
    public Vector2 vel = Vector2.zero;
    protected Rigidbody2D object_rb;
    public GameObject render;

    protected virtual void Awake()
    {
        object_rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Get vel from child class
        SetVel();

        // Apply vel so that it works in the Unity physics system
        object_rb.linearVelocity = vel;

        // If a render component is given, rotate it to face the vel
        if (render != null)
        {
            render.transform.rotation = Quaternion.FromToRotation(Vector2.right, vel);
        }
    }

    protected abstract void SetVel();
}