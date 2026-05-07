using UnityEngine;

// DamageOnTouch
// Detects collisions with PlayerTriggers layer, and deals damage on touch
public class DamageOnTouch : MonoBehaviour
{
    public int damage = 10;
    public string PlayerTriggers;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(PlayerTriggers))
        {
            if (other.TryGetComponent<PlayerHealth>(out var player_health))
            {
                player_health.TakeDamage(damage, Vector2.zero);
            }
        }
    }

}
