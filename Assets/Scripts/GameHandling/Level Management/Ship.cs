using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
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
    public float spin { get; private set; } // Radians per second

    [SerializeField] private float max_vel = 10;
    [SerializeField] private float max_spin = 50;


    public Vector2 center { get; private set; } = Vector2.zero; // Center coordinates

    public Vector2 global_vel { get; private set; } // Units per second

    public Vector2 global_pos { get; private set; } // Ship pos in global space

    public float global_angle { get; private set; } // Ship angle (radians) in global space
    public Tilemap spaceship { get; private set; }
    public TilemapCollider2D spaceship_collider { get; private set; }

    [SerializeField] private float vel_acc_rate = 1;     //
    [SerializeField] private float spin_acc_rate = 1;    // per sec

    public List<DenseMass> denseMasses { get; private set; }

    private Animator[] ThrustIndicators;
    private Transform CameraHolder;
    private CameraShaker Shaker;
    private AudioSource AudioSource;
    private bool[] Shaking = new bool[2];
    private float prog_check;

    // Initialization
    protected virtual void Awake()
    {
        Instance = this;

        denseMasses = new();

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

        CameraHolder = Camera.main.transform.parent;
        Shaker = CameraHolder.GetComponent<CameraShaker>();
        AudioSource = GetComponent<AudioSource>();
    }

    //////////////////////////////////////////
    // Repeatedly called via FIXEDUPDATE
    //////////////////////////////////////////
    public void SetVel(Vector2 global_input)
    {
        float epsilon = .1f;
        bool up = global_input.y > 0 && global_vel.y < max_vel - epsilon;
        bool down = global_input.y < 0 && global_vel.y > -max_vel + epsilon;
        bool left = global_input.x < 0 && global_vel.x > -max_vel + epsilon;
        bool right = global_input.x > 0 && global_vel.x < max_vel - epsilon;

        ThrustIndicators[1].SetBool("Thrusting", up);
        ThrustIndicators[0].SetBool("Thrusting", down);
        ThrustIndicators[3].SetBool("Thrusting", left);
        ThrustIndicators[2].SetBool("Thrusting", right);

        if (down || up || right || left)
        {
            Shaking[0] = true;
            Shaker.Shake();
        }
        else
        {
            Shaking[0] = false;
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

        vel = ClampManhattanMagnitude(vel, max_vel);
        global_vel = ClampManhattanMagnitude(global_vel, max_vel);

    }

    private Vector2 ClampManhattanMagnitude(Vector2 vector, float max)
    {
        return new Vector2(
            Mathf.Clamp(vector.x, -max, max),
            Mathf.Clamp(vector.y, -max, max)
        );
    }

    //////////////////////////////////////////
    // Repeatedly called via FIXEDUPDATE
    //////////////////////////////////////////
    public void SetSpin(float dir) // Positive is right, negative is left
    {
        spin += dir * spin_acc_rate * Time.fixedDeltaTime;
        spin = Mathf.Clamp(spin, -max_spin, max_spin);
        if ((dir > 0 && !Mathf.Approximately(spin, max_spin))
           || (dir < 0 && !Mathf.Approximately(spin, -max_spin)))
        {
            Shaker.Shake();
            Shaking[1] = true;
        }
        else
        {
            Shaking[1] = false;
        }
    }

    public void AddDenseMass(DenseMass dm)
    {
        denseMasses.Add(dm);
    }

    public void RemoveDenseMass(DenseMass dm)
    {
        denseMasses.Remove(dm);
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

        global_angle += spin * Time.fixedDeltaTime; // Update angle in global space
        global_pos += global_vel * Time.fixedDeltaTime; // Update position in global space

        if (prog_check > 2f)
        {
            prog_check = 0;
            ThrustIndicators[1].SetBool("Thrusting", false);
            ThrustIndicators[0].SetBool("Thrusting", false);
            ThrustIndicators[3].SetBool("Thrusting", false);
            ThrustIndicators[2].SetBool("Thrusting", false);
        }
        else prog_check += Time.fixedDeltaTime;

        if (AudioSource != null)
        {
            if (!AudioSource.isPlaying && (Shaking[0] || Shaking[1]))
            {
                AudioSource.Play();
            }
            if (AudioSource.isPlaying && !Shaking[0] && !Shaking[1])
            {
                AudioSource.Stop();
            }
        }
    }
}
