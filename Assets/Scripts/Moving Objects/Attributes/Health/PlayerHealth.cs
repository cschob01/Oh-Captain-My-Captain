using UnityEngine;
using System.Collections;

public class PlayerHealth : Health
{
    public bool damage_cool_down = false;

    public float cool_down_time = 1f; // Cannot be damaged twice in this amount of time

    public void Start()
    {
        EventHandler.Instance.HealthChange(this);
    }

    public override void TakeDamage(int damage, Vector2 dir)
    {
        if (!damage_cool_down)
        {
            StartCoroutine(CoolDown());

            health -= damage;
            onBoard.momentum += dir;

            if (health <= 0)
            {
                EventHandler.Instance.PlayerDied();
            }
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
