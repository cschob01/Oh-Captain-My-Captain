using UnityEngine;
using UnityEngine.InputSystem;

// ControlShip
// Gives user control over the spin and velocity of the ship
public class ControlShip : MonoBehaviour
{
    private void FixedUpdate()
    {
        if (InputHandler.Instance.SpinLeftIsPressed())
        {
            Ship.Instance.SetSpin(-1f); // Spin Left
        }
        if (InputHandler.Instance.SpinRightIsPressed())
        {
            Ship.Instance.SetSpin(1f); // Spin Right
        }

        Vector2 input = InputHandler.Instance.ThrustReadValue();
        Ship.Instance.SetVel(input);
    }
}

