using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private OnBoard OnBoard;
    public Ammo Ammo { get; set; }

    private void FixedUpdate()
    {
        Vector3 dir = transform.right;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, 
                                                OnBoard.momentum.normalized,
                                                OnBoard.momentum.magnitude * Time.fixedDeltaTime,
                                                hitMask);

        if (hit.collider != null)
        {
            transform.position = hit.point;

            DamageTarget(hit.collider.gameObject);
            PushTarget(hit.collider.gameObject);
            
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject, 0f);
            Destroy(this);
        }
    }

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
}
