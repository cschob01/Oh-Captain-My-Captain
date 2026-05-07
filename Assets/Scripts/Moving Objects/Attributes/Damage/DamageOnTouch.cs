using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    public int damage = 10;
    public string PlayerTriggers;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(PlayerTriggers))
        {
            Debug.Log("Collided with player!");

            if (other.TryGetComponent<PlayerHealth>(out var player_health))
            {
                player_health.TakeDamage(damage, Vector2.zero);
            }
        }
        else
        {
            Debug.Log("Collided with non-player!");
        }
    }

}
