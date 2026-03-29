using UnityEngine;
using UnityEngine.InputSystem;

public class ControlPlayer : MovingObject
{

    [System.Serializable]
    public struct KeyState
    {
        public Key key;
        public bool pressed;

        public KeyState(Key key)
        {
            this.key = key;
            this.pressed = false;
        }
    }

    KeyState[] keys =
    {
        new KeyState(Key.W), // Forward
        new KeyState(Key.A), // Left
        new KeyState(Key.S), // Backward
        new KeyState(Key.D), // Right
    };

    public float acc = .04f;
    public float max_vel = 3;

    void Update()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            keys[i].pressed = Keyboard.current[keys[i].key].isPressed;
        }
    }

    // SetVel called once per FixedUpdate
    protected override void SetVel()
    {

        Vector2 input = Vector2.zero;

        if (keys[0].pressed) input += Vector2.up;  
        if (keys[1].pressed) input += Vector2.left;
        if (keys[2].pressed) input += Vector2.down;
        if (keys[3].pressed) input += Vector2.right;

        // Get the camera's rotation in radians
        float camRot = Camera.main.transform.eulerAngles.z * Mathf.Deg2Rad;

        // Rotate the input vector by the camera rotation
        Vector2 rotatedInput = new Vector2(
            input.x * Mathf.Cos(camRot) - input.y * Mathf.Sin(camRot),
            input.x * Mathf.Sin(camRot) + input.y * Mathf.Cos(camRot)
        );
        vel += acc * rotatedInput;
        vel = Vector2.ClampMagnitude(vel, max_vel);
    }
}
