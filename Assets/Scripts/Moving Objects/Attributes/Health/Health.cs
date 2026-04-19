using UnityEngine;

public abstract class Health : MonoBehaviour
{
    public int health = 20;
    public OnBoard onBoard;

    public abstract void TakeDamage(int damage, Vector2 dir);
}
