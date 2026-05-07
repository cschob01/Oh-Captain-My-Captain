using UnityEngine;

public class EnemyHealth : Health
{
    public float knockback_vulnerability = 1f;
    public override void TakeDamage(int damage, Vector2 dir)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(transform.parent.gameObject);
        }

        onBoard.momentum += dir * knockback_vulnerability;
    }

}
