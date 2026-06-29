using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerHealth : Health
{
    public int MaxHealth = 100;

    [SerializeField] private float HealSpeed = 5.0f;
    [SerializeField] private float HealCooldown = 5.0f;
    [SerializeField] private CameraShaker camShaker;
    [SerializeField] private Animator animator;

    private float CooldownProg = 0f;
    [HideInInspector] public bool healing;

    private bool dead;

    private void Update()
    {
        if (CooldownProg < HealCooldown) {
            CooldownProg += Time.deltaTime;
            healing = false;
            return;
        }
        if (health == MaxHealth)
        {
            healing = false;
            return;
        }

        if (dead) return;

        healing = true;
        health += HealSpeed * Time.deltaTime;
        health = Mathf.Clamp(health, 0, MaxHealth);
    }


    public void Start()
    {
        EventHandler.Instance.HealthChange(this);
    }

    public override void TakeDamage(int damage, Vector2 dir)
    {
        if (dead) return;

        bool ArmorActive = health / MaxHealth > .8f; // Shouldn't die in one hit if above 80%

        health -= damage;
        onBoard.momentum += dir;
        CooldownProg = 0;

        if (camShaker != null) camShaker.Shake(.1f);
            
        if (health <= 0)
        {
            if (ArmorActive) health = 10;
            else {
                health = 0;
                PlayerDies();
                return;
            }
        }
        if (animator != null) animator.SetTrigger("Hurt");
    }

    private void PlayerDies()
    {
        dead = true;
        EventHandler.Instance.PlayerDied();
    }
}
