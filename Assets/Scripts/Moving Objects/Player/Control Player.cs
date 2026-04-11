using UnityEngine;
using UnityEngine.InputSystem;

public class ControlPlayer : MovingObject
{

    public InputHandler Handler;

    public float acc = .04f;
    public float max_vel = 3;

    // SetVel called once per FixedUpdate
    protected override void SetVel()
    {
        Vector2 input = Handler.controls.Player.Move.ReadValue<Vector2>();

        // Rotate relative to camera
        float camRot = Camera.main.transform.eulerAngles.z * Mathf.Deg2Rad;

        Vector2 rotatedInput = new Vector2(
            input.x * Mathf.Cos(camRot) - input.y * Mathf.Sin(camRot),
            input.x * Mathf.Sin(camRot) + input.y * Mathf.Cos(camRot)
        );

        vel += acc * rotatedInput;
        vel = Vector2.ClampMagnitude(vel, max_vel);
    }
}
