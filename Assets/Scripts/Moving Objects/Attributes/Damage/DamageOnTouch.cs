using System.Collections;
using UnityEngine;

// DamageOnTouch
// Detects collisions with PlayerTriggers layer, and deals damage on touch
public class DamageOnTouch : MonoBehaviour
{
    public int damage = 10;
    [SerializeField] private float Cooldown = 2.5f;

    private bool OnCooldown = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (OnCooldown) return;

        other.GetComponent<PlayerHealth>()?.TakeDamage(damage, Vector2.zero);
        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        OnCooldown = true;
        yield return new WaitForSeconds(Cooldown);
        OnCooldown = false;
    }
}
