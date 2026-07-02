using UnityEngine;
using UnityEngine.InputSystem;

// ControlShip
// Gives user control over the spin and velocity of the ship
public class ControlShip : MonoBehaviour
{
    [SerializeField] float SpinMultiplier;
    [SerializeField] float ThrustMultiplier;

    private void FixedUpdate()
    {
        if (InputHandler.Instance.SpinLeftIsPressed())
        {
            Ship.Instance.SetSpin(-SpinMultiplier); // Spin Left
        }
        if (InputHandler.Instance.SpinRightIsPressed())
        {
            Ship.Instance.SetSpin(SpinMultiplier); // Spin Right
        }

        Vector2 input = InputHandler.Instance.ThrustReadValue();
        Ship.Instance.SetVel(input * ThrustMultiplier);
    }
}

