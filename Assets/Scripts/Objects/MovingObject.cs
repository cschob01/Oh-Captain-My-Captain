using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public Vector2 vel = Vector2.zero;
    protected Rigidbody2D object_rb;

    protected virtual void Awake()
    {
        object_rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        SetVel();
        object_rb.linearVelocity = vel;
    }

    protected virtual void SetVel() { }
}