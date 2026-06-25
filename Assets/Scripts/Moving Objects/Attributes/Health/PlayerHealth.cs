using UnityEngine;
using System.Collections;

public class PlayerHealth : Health
{
    public int MaxHealth = 100;

    [SerializeField] private float HealSpeed = 5.0f;
    [SerializeField] private float HealCooldown = 5.0f;

    private float CooldownProg = 0f;

    private void Update()
    {
        if (CooldownProg < HealCooldown) {
            CooldownProg += Time.deltaTime;
            return;
        }

        health += HealSpeed * Time.deltaTime;
        health = Mathf.Clamp(health, 0, MaxHealth);
    }


    public void Start()
    {
        EventHandler.Instance.HealthChange(this);
    }

    public override void TakeDamage(int damage, Vector2 dir)
    {
        health -= damage;
        onBoard.momentum += dir;
        CooldownProg = 0;
            
        if (health <= 0)
        {
            EventHandler.Instance.PlayerDied();
            health = 0;
        }
    }
}
