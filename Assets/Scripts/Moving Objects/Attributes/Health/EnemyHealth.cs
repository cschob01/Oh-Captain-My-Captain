using UnityEngine;

public class EnemyHealth : Health
{
    public override void TakeDamage(int damage, Vector2 dir)
    {
        health -= damage;
        Debug.Log("Enemy Hit!");
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        onBoard.momentum += dir;
    }

}
