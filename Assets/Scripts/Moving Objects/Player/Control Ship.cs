using UnityEngine;
using UnityEngine.InputSystem;

public class ControlShip : MonoBehaviour
{
    public InputHandler Handler;

    // SetVel called once per FixedUpdate
    private void FixedUpdate()
    {
        if (Handler.controls.Ship.SpinLeft.IsPressed())
        {
            Ship.Instance.SetSpin(false); // Spin Left
        }
        if (Handler.controls.Ship.SpinRight.IsPressed())
        {
            Ship.Instance.SetSpin(true); // Spin Right
        }

        Vector2 input = Handler.controls.Ship.Thrust.ReadValue<Vector2>();

        // Get the camera's rotation in radians
        float camRot = Camera.main.transform.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 rotatedInput = new Vector2(
            input.x * Mathf.Cos(camRot) - input.y * Mathf.Sin(camRot),
            input.x * Mathf.Sin(camRot) + input.y * Mathf.Cos(camRot)
        );
        Ship.Instance.SetVel(rotatedInput, input);
    }
}

