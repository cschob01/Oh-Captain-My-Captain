using UnityEngine;
using UnityEngine.Audio;

public class EnemyHealth : Health
{
    [SerializeField] private Animator animator;

    [SerializeField] private AudioClip Hit;
    [SerializeField] private AudioClip Dead;
    private AudioSource AudioSource;

    private void Awake()
    {
        AudioSource = GetComponentInChildren<AudioSource>();
    }

    public float knockback_vulnerability = 1f;
    public override void TakeDamage(int damage, Vector2 dir)
    {
        health -= damage;
        if (animator != null) animator.SetTrigger("Hurt");

        if (health <= 0)
        {
            if (AudioSource != null)
            {
                AudioSource.clip = Dead;
                AudioSource.transform.SetParent(null);
                AudioSource.Play();
                Destroy(AudioSource.gameObject, AudioSource.clip.length);
                AudioSource = null;
            }

                CaptainHandler.Instance.MakeMoney(100);
            EventHandler.Instance.EnemyDied();
            Destroy(transform.parent.gameObject);
        }
        else
        {
            if (AudioSource != null)
            {
                AudioSource.clip = Hit;
                AudioSource.Play();
            }
        }

            onBoard.momentum += dir * knockback_vulnerability;
    }

}
