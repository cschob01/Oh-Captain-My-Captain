using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Projectile : MonoBehaviour
{
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private OnBoard_Projectile OnBoard;
    public Ammo Ammo { get; set; }
    public int BouncesLeft = 0;

    private void DamageTarget(GameObject target)
    {
        target.GetComponent<Health>()?.TakeDamage(Ammo.damage, Vector2.zero);
    }

    private void PushTarget(GameObject target)
    {
        OnBoard tar_OnBoard = target.GetComponentInParent<OnBoard>();
        if (tar_OnBoard != null)
        {
            tar_OnBoard.momentum += (OnBoard.momentum - tar_OnBoard.momentum) * Ammo.weight * .001f;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            if (BouncesLeft == 0)
            {
                Destroy(gameObject);
            }
            else
            {
                BouncesLeft--;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        DamageTarget(collider.gameObject);
        PushTarget(collider.gameObject);

        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject, 0f);
        Destroy(this);
    }

}

