using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;

// Initializes and updates "global" variables for Ship instacne
// Defines setters based on initialized variables for modifying the state of the ship.
// Note: The tilemap is never actually moving. It's movement is stored in this script
//       and accessed by any other scripts in the scene.
public class Ship : MonoBehaviour
{
    public static Ship Instance;

    public Vector2 vel { get; private set; } // Units per second
    public float spin { get; private set; } // Degrees per second

    public float max_vel = 10;
    public float max_spin = 5;


    public Vector2 center { get; private set; } // Center coordinates

    public Vector2 global_vel { get; private set; } // Units per second

    public Vector2 global_pos { get; private set; } // Ship pos in global space

    public float global_angle { get; private set; } // Ship angle in global space


    public Tilemap spaceship { get; private set; }
    public TilemapCollider2D spaceship_collider { get; private set; }

    private float vel_acc_rate;     //
    private float spin_acc_rate;    // per sec

    private Animator[] ThrustIndicators;
    private Transform CameraHolder;
    private CameraShaker Shaker;
    private bool Shifting;

    // Initialization
    protected virtual void Awake()
    {
        Instance = this;

        spaceship = GameObject.Find("Walls").GetComponent<Tilemap>();
        spaceship_collider = spaceship.GetComponent<TilemapCollider2D>();
        if (spaceship == null || spaceship_collider == null)
        {
            Debug.Log("Spaceship not set up properly");
        }

        ThrustIndicators = new Animator[] {
            transform.Find("Canvas/ThrustIndicators/Up").GetComponent<Animator>(),
            transform.Find("Canvas/ThrustIndicators/Down").GetComponent<Animator>(),
            transform.Find("Canvas/ThrustIndicators/Left").GetComponent<Animator>(),
            transform.Find("Canvas/ThrustIndicators/Right").GetComponent<Animator>()
        };

        vel = Vector2.zero;
        spin = 0;

        vel_acc_rate = .85f;
        spin_acc_rate = .15f; ;

        center = Vector2.zero;

        CameraHolder = Camera.main.transform.parent;
        Shaker = CameraHolder.GetComponent<CameraShaker>();
    }

    //////////////////////////////////////////
    // Repeatedly called via FixedUpdate
    //////////////////////////////////////////
    public void SetVel(Vector2 global_input)
    {
        float temp_epsilon = .001f;
        if (global_input.y < -temp_epsilon)
        {
            ThrustIndicators[0].SetBool("Thrusting", true);
            Shaker.Shake();
        }
        else {
            ThrustIndicators[0].SetBool("Thrusting", false);
        }

        if (global_input.y > temp_epsilon)
        {
            ThrustIndicators[1].SetBool("Thrusting", true);
            Shaker.Shake();
        }
        else
        {
            ThrustIndicators[1].SetBool("Thrusting", false);
        }

        if (global_input.x > temp_epsilon)
        {
            ThrustIndicators[2].SetBool("Thrusting", true);
            Shaker.Shake();
        }
        else
        {
            ThrustIndicators[2].SetBool("Thrusting", false);
        }

        if (global_input.x < -temp_epsilon)
        {
            ThrustIndicators[3].SetBool("Thrusting", true);
            Shaker.Shake();
        }
        else
        {
            ThrustIndicators[3].SetBool("Thrusting", false);
        }

        // Get the camera's rotation in radians. Rotating the ship's movement by
        // this will keep its movement relative to the player's camera
        float camRot = CameraHolder.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 local_input = new Vector2(
            global_input.x * Mathf.Cos(camRot) - global_input.y * Mathf.Sin(camRot),
            global_input.x * Mathf.Sin(camRot) + global_input.y * Mathf.Cos(camRot)
        );
        vel += local_input * vel_acc_rate * Time.fixedDeltaTime;

        global_vel += global_input * vel_acc_rate * Time.fixedDeltaTime;

        vel = Vector2.ClampMagnitude(vel, max_vel);
        global_vel = Vector2.ClampMagnitude(global_vel, max_vel);

    }
    public void SetSpin(float dir) // Positive is right, negative is left
    {
        spin += dir * spin_acc_rate * Time.fixedDeltaTime;
        spin = Mathf.Clamp(spin, -max_spin, max_spin);
        if (!Mathf.Approximately(0, dir)) Shaker.Shake();
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
