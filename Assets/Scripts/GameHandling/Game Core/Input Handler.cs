using UnityEngine;

// InputHandler
// Sets up the input controls depending on active level
// (Currently set to default state. Only one scene is active)
public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;

    private InputSystem controls;

    void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        controls = new InputSystem();
    }
    void Start()
    {
        controls.Enable();
    }

    public void EnableControls(bool enable)
    {
        if (enable)
        {
            controls.Enable();
        }
        else
        {
            controls.Disable();
        }
    }

    public bool FireIsPressed() { return controls.Gun.Attack.IsPressed(); }
    public bool ReloadWasPressedThisFrame() { return controls.Gun.Reload.WasPressedThisFrame(); }
    public Vector2 LookReadValue() { return controls.Player.Look.ReadValue<Vector2>(); }

    public bool SpinLeftIsPressed() { return controls.Ship.SpinLeft.IsPressed(); }
    public bool SpinRightIsPressed() { return controls.Ship.SpinRight.IsPressed(); }
    public Vector2 ThrustReadValue() { return controls.Ship.Thrust.ReadValue<Vector2>(); }
    public Vector2 MoveReadValue() { return controls.Player.Move.ReadValue<Vector2>(); }
    public bool GadgetWasPressedThisFrame() { return controls.Gadget.Use.WasPressedThisFrame();  }

}
