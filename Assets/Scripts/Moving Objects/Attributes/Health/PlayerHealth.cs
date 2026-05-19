using UnityEngine;
using System.Collections;

public class PlayerHealth : Health
{
    public bool damage_cool_down = false;

    public float cool_down_time = 3f; // Cannot be damaged twice in this amount of time

    public void Awake()
    {
        EventHandler.Instance.PlayerHealthChange(health);
    }

    public override void TakeDamage(int damage, Vector2 dir)
    {
        if (!damage_cool_down)
        {
            StartCoroutine(CoolDown());

            health -= damage;
            EventHandler.Instance.PlayerHealthChange(health);
            if (health <= 0)
            {
                Destroy(transform.parent.gameObject);
            }

            onBoard.momentum += dir;
        }
    }

    // Gives player time to recover
    IEnumerator CoolDown()
    {
        damage_cool_down = true;

        yield return new WaitForSeconds(cool_down_time);

        damage_cool_down = false;
    }

}
