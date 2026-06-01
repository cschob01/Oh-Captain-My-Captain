using UnityEngine;
using UnityEngine.InputSystem;

// ControlPlayer
// Controls Player velocity using user inputs
public class ControlPlayer : MovingObject
{
    private Transform CameraHolder;

    protected override void Awake()
    {
        object_rb = GetComponent<Rigidbody2D>();
        CameraHolder = Camera.main.transform.parent;
    }

    // SetVel called once per FixedUpdate
    protected override void SetVel()
    {
        Vector2 input = InputHandler.Instance.MoveReadValue();

        // Rotate relative to camera so that it matches what user expects
        float camRot = CameraHolder.eulerAngles.z * Mathf.Deg2Rad;

        Vector2 rotatedInput = new Vector2(
            input.x * Mathf.Cos(camRot) - input.y * Mathf.Sin(camRot),
            input.x * Mathf.Sin(camRot) + input.y * Mathf.Cos(camRot)
        );

        vel += acc * rotatedInput;
        vel = Vector2.ClampMagnitude(vel, max_vel);
    }
}
