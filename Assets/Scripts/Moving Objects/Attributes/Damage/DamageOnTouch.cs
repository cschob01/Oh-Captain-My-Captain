using UnityEngine;

// DamageOnTouch
// Detects collisions with PlayerTriggers layer, and deals damage on touch
public class DamageOnTouch : MonoBehaviour
{
    public int damage = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<PlayerHealth>()?.TakeDamage(damage, Vector2.zero);
    }

}
