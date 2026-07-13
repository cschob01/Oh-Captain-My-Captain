using System.Collections;
using UnityEngine;

public class ShockGrenade : MonoBehaviour
{
    [SerializeField] private float Radius;
    [SerializeField] private float Force;
    [SerializeField] private float time;

    private Animator animator;
    private AudioSource Explosion;

    private void Awake()
    {
        if (time == 0) Debug.Log("ERROR: time for ShockGrenade cannot be equal to 0");
        animator = GetComponent<Animator>();
        if (animator == null) Debug.Log("ERROR: No animator set up for ShockGrenade");
        StartCoroutine(ExplodeAfter());

        Explosion = GetComponent<AudioSource>();
    }

    private IEnumerator ExplodeAfter()
    {
        float timeLeft = 0;
        while(timeLeft < time)
        {
            timeLeft += Time.deltaTime;
            animator.speed = 1 + (timeLeft / time) * 6;
            yield return null;
        }

        Explosion?.Play();
        EventHandler.Instance.Explosion(transform.position, Force, Radius);
        animator.speed = 1f;
        animator.SetTrigger("Explode");
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
