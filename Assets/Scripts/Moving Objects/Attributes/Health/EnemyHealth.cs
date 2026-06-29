using UnityEngine;

public class EnemyHealth : Health
{
    [SerializeField] private Animator animator;

    public float knockback_vulnerability = 1f;
    public override void TakeDamage(int damage, Vector2 dir)
    {
        health -= damage;
        if (animator != null) animator.SetTrigger("Hurt");

        if (health <= 0)
        {
            CaptainHandler.Instance.MakeMoney(100);
            EventHandler.Instance.EnemyDied();
            Destroy(transform.parent.gameObject);
        }

        onBoard.momentum += dir * knockback_vulnerability;
    }

}
