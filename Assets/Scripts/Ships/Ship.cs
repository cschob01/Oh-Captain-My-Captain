using UnityEngine;
using UnityEngine.Tilemaps;

// Initializes and updates "global" variables for Ship instacne
// Defines setters based on initialized variables for modifying the state of the ship.
// Note: The tilemap is never actually moving. It's movement is stored in this script
//       and accessed by any other scripts in the scene.
public class Ship : MonoBehaviour
{
    public static Ship Instance;

    public Vector2 vel { get; private set; } // Units per second
    public float spin { get; private set; } // Degrees per second


    public Vector2 center { get; private set; } // Center coordinates

    public Vector2 global_vel { get; private set; } // Units per second

    public Vector2 global_pos { get; private set; } // Ship pos in global space

    public float global_angle { get; private set; } // Ship angle in global space


    public Tilemap spaceship { get; private set; }
    public TilemapCollider2D spaceship_collider { get; private set; }

    private float vel_acc_rate;     //
    private float spin_acc_rate;    // per sec

    // Initialization
    protected virtual void Awake()
    {
        Instance = this;

        spaceship = transform.Find("Walls").GetComponent<Tilemap>();
        spaceship_collider = spaceship.GetComponent<TilemapCollider2D>();

        vel = Vector2.zero;
        spin = 0;

        vel_acc_rate = .85f;
        spin_acc_rate = .15f; ;

        center = Vector2.zero;
    }

    //////////////////////////////////////////
    // Repeatedly called via FixedUpdate
    //////////////////////////////////////////
    public void SetVel(Vector2 input, Vector2 global_input)
    {
        vel += input * vel_acc_rate * Time.fixedDeltaTime;
        global_vel += global_input * vel_acc_rate * Time.fixedDeltaTime;
    }
    public void SetSpin(float dir) // Positive is right, negative is left
    {
        spin += dir * spin_acc_rate * Time.fixedDeltaTime;
    }

    private void FixedUpdate()
    {
        // Rotate vel to compensate for spin
        float cos = Mathf.Cos(spin * Time.fixedDeltaTime);
        float sin = Mathf.Sin(spin * Time.fixedDeltaTime);

        vel = new Vector2(
            vel.x * cos - vel.y * sin,
            vel.x * sin + vel.y * cos
        );

        global_angle += spin * Mathf.Rad2Deg * Time.fixedDeltaTime; // Update angle in global space
        global_pos += global_vel * Time.fixedDeltaTime; // Update position in global space
    }
}
