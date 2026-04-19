using UnityEngine;

public class PlayerHealth : Health
{
    public DisplayPlayerHealth displayHealth;

    public void Awake()
    {
        health = 100;
        displayHealth = FindFirstObjectByType<DisplayPlayerHealth>();
        displayHealth.SetHealth(health);
    }

    public override void TakeDamage(int damage, Vector2 dir)
    {
        health -= damage;
        displayHealth.SetHealth(health);
        Debug.Log("Enemy Hit!");
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        onBoard.momentum += dir;
    }
}
