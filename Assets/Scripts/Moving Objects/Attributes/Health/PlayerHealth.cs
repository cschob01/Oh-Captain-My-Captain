using UnityEngine;
using System.Collections;

public class PlayerHealth : Health
{
    public DisplayPlayerHealth displayHealth;
    public bool damage_cool_down = false;

    public float cool_down_time = 3f;

    public void Awake()
    {
        health = 100;
        displayHealth = FindFirstObjectByType<DisplayPlayerHealth>();
        displayHealth.SetHealth(health);
    }

    public override void TakeDamage(int damage, Vector2 dir)
    {
        if (!damage_cool_down)
        {
            StartCoroutine(CoolDown());


            health -= damage;
            displayHealth.SetHealth(health);
            Debug.Log("Player Hit!");
            if (health <= 0)
            {
                Destroy(gameObject);
            }

            onBoard.momentum += dir;
        }
        else
        {
            Debug.Log("Player On Cooldown!");
        }
    }

    IEnumerator CoolDown()
    {
        damage_cool_down = true;

        yield return new WaitForSeconds(cool_down_time);

        damage_cool_down = false;
    }

}
