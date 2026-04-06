using UnityEngine;
using UnityEngine.Tilemaps;

// Initializes "global" variables
// Defines setters based on initialized variables
public class Ship : MonoBehaviour
{
    public static Ship Instance;

    public Vector2 vel { get; private set; } // Units per second
    public float spin { get; private set; } // Degrees per second
    public float grav_dir { get; private set; } // Direction in degrees
    public float grav_force { get; private set; } // Direction in degrees
    public Vector2 center { get; private set; } // Center coordinates
    public Tilemap spaceship { get; private set; }
    public TilemapCollider2D spaceship_collider { get; private set; }
    public Tilemap doors { get; private set; }
    public Tilemap airlocks { get; private set; }

    private float vel_acc_rate;     //
    private float spin_acc_rate;    // per sec
    private float grav_acc_rate;    //
    private float grav_force_standard;

    private bool vel_active;        //
    private bool spin_active;       // Temporary states handled by 
    private bool grav_active;       // individual ships

    // Initialization
    protected virtual void Awake()
    {
        Instance = this;

        spaceship = transform.Find("Walls").GetComponent<Tilemap>();
        spaceship_collider = spaceship.GetComponent<TilemapCollider2D>();

        vel = Vector2.zero;
        spin = 0;
        grav_force = 0;

        vel_active = true;
        spin_active = true;
        grav_active = true;

        vel_acc_rate = .85f;
        spin_acc_rate = .15f; ;
        grav_acc_rate = .15f; ;

        grav_force = 0;
        grav_force_standard = 1;

        center = Vector2.zero;
    }

    //////////////////////////////////////////
    // Repeatedly called via FixedUpdate
    //////////////////////////////////////////
    public void SetVel(Vector2 input)
    {
        if (vel_active)
        {
            vel += input * vel_acc_rate * Time.fixedDeltaTime;
        }
    }
    public void SetSpin(bool dir)
    {
        if (spin_active)
        {
            spin += (dir ? 1 : -1) * spin_acc_rate * Time.fixedDeltaTime;
        }
    }
    public void SetGrav(bool dir, bool on)
    {
        if (grav_active)
        {
            grav_dir += (dir ? 1 : -1) * grav_acc_rate * Time.fixedDeltaTime;
            if (!on) grav_force = 0;
            else grav_force = grav_force_standard;
        }
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
    }
}
