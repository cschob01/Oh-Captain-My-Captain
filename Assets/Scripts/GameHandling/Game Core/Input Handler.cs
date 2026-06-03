using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance; // Make sigleton
    private PlayerInput playerInput;

    public PlayerInput PlayerInput => playerInput;

    private InputAction attack;
    private InputAction reload;
    private InputAction look;
    private InputAction spinLeft;
    private InputAction spinRight;
    private InputAction thrust;
    private InputAction moveUp;
    private InputAction moveLeft;
    private InputAction moveDown;
    private InputAction moveRight;
    private InputAction gadget;
    private InputAction pause;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        playerInput = GetComponent<PlayerInput>();
        Instance = this;

        attack = GetAction("Attack");
        reload = GetAction("Reload");
        look = GetAction("Look");
        spinLeft = GetAction("SpinLeft");
        spinRight = GetAction("SpinRight");
        thrust = GetAction("Thrust");
        moveUp = GetAction("MoveUp");
        moveLeft = GetAction("MoveLeft");
        moveDown = GetAction("MoveDown");
        moveRight = GetAction("MoveRight");
        gadget = GetAction("Gadget/Use");
        pause = GetAction("Game/Pause");
    }

    private InputAction GetAction(string actionName)
    {
        InputAction action = playerInput.actions.FindAction(actionName);

        if (action == null)
        {
            Debug.LogError($"Input Action '{actionName}' was not found in the Input Actions asset.");
        }

        return action;
    }

    public void EnableControls(bool enable)
    {
        if (enable)
            playerInput.actions.Enable();
        else
            playerInput.actions.Disable();
    }

    public bool FireIsPressed() => attack.IsPressed();
    public bool ReloadWasPressedThisFrame() => reload.WasPressedThisFrame();
    public Vector2 LookReadValue() => look.ReadValue<Vector2>();

    public bool SpinLeftIsPressed() => spinLeft.IsPressed();
    public bool SpinRightIsPressed() => spinRight.IsPressed();
    public Vector2 ThrustReadValue() => thrust.ReadValue<Vector2>();
    public Vector2 MoveReadValue()
    {
        float x = 0f;
        float y = 0f;

        if (moveRight.IsPressed()) x += 1f;
        if (moveLeft.IsPressed()) x -= 1f;
        if (moveUp.IsPressed()) y += 1f;
        if (moveDown.IsPressed()) y -= 1f;

        Vector2 result = new Vector2(x, y);

        return result.sqrMagnitude > 1f ? result.normalized : result;
    }
    public bool GadgetWasPressedThisFrame() => gadget.WasPressedThisFrame();

    public bool PauseWasPressedThisFrame() => pause.WasPressedThisFrame();

    public string CurrentControlScheme => playerInput.currentControlScheme;
}