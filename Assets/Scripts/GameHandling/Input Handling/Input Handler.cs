using UnityEngine;

// InputHandler
// Sets up the input controls depending on active level
// (Currently set to default state. Only one scene is active)
public class InputHandler : MonoBehaviour
{
    public InputSystem controls { get; private set; }

    void Awake()
    {
        controls = new InputSystem();
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }
}
