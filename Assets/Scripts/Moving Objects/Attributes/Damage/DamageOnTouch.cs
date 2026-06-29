using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

// DamageOnTouch
// Detects collisions with PlayerTriggers layer, and deals damage on touch
public class DamageOnTouch : MonoBehaviour
{
    public int damage = 10;
    
    [Tooltip("Differences in momentum multiplier")]
    [SerializeField] private float forceMultiplier = .5f;
    [SerializeField] private float Cooldown = 2.5f;

    private bool OnCooldown = false;
    private OnBoard OnBoard;
    private MovingObject MovingObject;

    private void Awake()
    {
        OnBoard = GetComponent<OnBoard>();
        if (OnBoard == null) Debug.Log(gameObject + " could not find OnBoard script for DamageOnTouch");
        MovingObject = GetComponent<MovingObject>();
        if (MovingObject == null) Debug.Log(gameObject + " could not find MovingObject script for DamageOnTouch");
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (OnCooldown) return;

        OnBoard onBoard = other.GetComponentInParent<OnBoard>();
        MovingObject movingObject = other.GetComponentInParent<MovingObject>();
        Vector2 force = Vector2.zero;
        if (onBoard == null || movingObject == null) Debug.Log("Could not find OnBoard/MovingObject script for DamageOnTouch to " + other.gameObject);
        else if (OnBoard != null && MovingObject != null) force = OnBoard.momentum + MovingObject.vel - onBoard.momentum - movingObject.vel;

        other.GetComponent<PlayerHealth>()?.TakeDamage(damage, force * forceMultiplier);
        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        OnCooldown = true;
        yield return new WaitForSeconds(Cooldown);
        OnCooldown = false;
    }
}
