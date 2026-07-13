using System.Collections;
using UnityEngine;

public class EnergyBar : Gadget
{
    private MovingObject MovingObject;

    [SerializeField] private float MaxVelBoost = 2f;
    [SerializeField] private float AccBoost = 2f;

    private float MaxVel;
    private float Acc;

    private AudioSource Crunch;

    private void Awake()
    {
        MovingObject = GetComponentInParent<MovingObject>();
        GetComponent<SpriteRenderer>().enabled = false;

        if (MovingObject == null) Debug.Log("ERROR: No moving object in parent of energy bar");
        else
        {
            MaxVel = MovingObject.max_vel;
            Acc = MovingObject.acc;
        }

        Crunch = GetComponent<AudioSource>();
    }
    protected override void Use()
    {
        Crunch?.Play();

        MovingObject.max_vel = MaxVel * MaxVelBoost;
        MovingObject.acc = Acc * AccBoost;
    }

    protected override void Disuse()
    {
        MovingObject.max_vel = MaxVel;
        MovingObject.acc = Acc;
    }

    private void OnDisable()
    {
        MovingObject.max_vel = MaxVel;
        MovingObject.acc = Acc;
    }
}
