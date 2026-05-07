using UnityEngine;
using UnityEngine.InputSystem;

// ControlShip
// Gives user control over the spin and velocity of the ship
public class ControlShip : MonoBehaviour
{
    public InputHandler Handler;

    private void FixedUpdate()
    {
        if (Handler.controls.Ship.SpinLeft.IsPressed())
        {
            Ship.Instance.SetSpin(-1f); // Spin Left
        }
        if (Handler.controls.Ship.SpinRight.IsPressed())
        {
            Ship.Instance.SetSpin(1f); // Spin Right
        }

        Vector2 input = Handler.controls.Ship.Thrust.ReadValue<Vector2>();

        // Rotate to camera so velocity change matches user's expectations
        float camRot = Camera.main.transform.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 rotatedInput = new Vector2(
            input.x * Mathf.Cos(camRot) - input.y * Mathf.Sin(camRot),
            input.x * Mathf.Sin(camRot) + input.y * Mathf.Cos(camRot)
        );
        Ship.Instance.SetVel(rotatedInput, input);
    }
}

