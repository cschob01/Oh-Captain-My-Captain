using UnityEngine;

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
