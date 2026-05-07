using UnityEngine;

// Health
// This is the abstract class for player and enemy health. It defines the 
// basic interface for taking damage.
public abstract class Health : MonoBehaviour
{
    public int health = 20;
    public OnBoard onBoard;

    public abstract void TakeDamage(int damage, Vector2 dir);
}
