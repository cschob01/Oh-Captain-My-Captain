using System.Collections;
using UnityEngine;

public class EnergyBar : Gadget
{
    private MovingObject MovingObject;
    private bool Boosted = false;

    [SerializeField] private float MaxVelBoost = 2f;
    [SerializeField] private float AccBoost = 2f;
    [SerializeField] private float BoostDuration = 7f;

    private void Awake()
    {
        MovingObject = GetComponentInParent<MovingObject>();
        GetComponent<SpriteRenderer>().enabled = false;
    }
    protected override void Use()
    {
        if (!Boosted)
        {
            StartCoroutine(BoostRoutine());
        }
    }

    public override void Deactivate()
    {
        
    }

    private IEnumerator BoostRoutine()
    {
        Boosted = true;
        MovingObject.acc *= AccBoost;
        MovingObject.max_vel *= MaxVelBoost;

        yield return new WaitForSeconds(BoostDuration);

        MovingObject.acc /= AccBoost;
        MovingObject.max_vel /= MaxVelBoost;
        StartCoroutine(CooldownRoutine());
        Boosted = false;
    }
}
