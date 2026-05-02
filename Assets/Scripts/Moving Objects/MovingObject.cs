using UnityEngine;

public class MovingObject : MonoBehaviour
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
        SetVel();
        object_rb.linearVelocity = vel;
        if (render != null)
        {
            render.transform.rotation = Quaternion.FromToRotation(Vector2.right, vel);
        }
    }

    protected virtual void SetVel() { }
}