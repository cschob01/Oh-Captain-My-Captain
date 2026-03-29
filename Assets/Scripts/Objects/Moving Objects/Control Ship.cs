using UnityEngine;
using UnityEngine.InputSystem;

public class ControlShip : MonoBehaviour
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
        new KeyState(Key.Q), // Spin Ship Left
        new KeyState(Key.E), // Spin Ship Right
        new KeyState(Key.UpArrow), // Move Ship Up
        new KeyState(Key.LeftArrow), // Move Ship Left
        new KeyState(Key.DownArrow), // Move Ship Down
        new KeyState(Key.RightArrow), // Move Ship Right
        new KeyState(Key.Space)
    };

    void Update()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            keys[i].pressed = Keyboard.current[keys[i].key].isPressed;
        }
    }

    // SetVel called once per FixedUpdate
    private void FixedUpdate()
    {
        if (keys[0].pressed)
        {
            Ship.Instance.SetSpin(false); // Spin Left
        }
        if (keys[1].pressed)
        {
            Ship.Instance.SetSpin(true); // Spin Right
        }

        Vector2 input = Vector2.zero;
        if (keys[2].pressed) input += Vector2.up;
        if (keys[3].pressed) input += Vector2.left;
        if (keys[4].pressed) input += Vector2.down;
        if (keys[5].pressed) input += Vector2.right;

        // Get the camera's rotation in radians
        float camRot = Camera.main.transform.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 rotatedInput = new Vector2(
            input.x * Mathf.Cos(camRot) - input.y * Mathf.Sin(camRot),
            input.x * Mathf.Sin(camRot) + input.y * Mathf.Cos(camRot)
        );
        Ship.Instance.SetVel(rotatedInput);
    }
}

