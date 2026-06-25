using UnityEngine;
using System.Collections;

// Gun
// This is the abstract class defining the interface for different gun types 
public class Gun : MonoBehaviour
{
    private LayerMask hitMask;
    private Transform Muzzle;

    private bool Shooting = false;

    [HideInInspector]
    public int Chamber;
    [HideInInspector]
    public float ReloadProg = 0f;

    private Coroutine ReloadCoroutine;

    private OnBoard_MovingObject onBoard;

    [Header("Shooting Settings")]

    [SerializeField] private int Burst = 1;

    [Tooltip("Seconds per shot")]
    [SerializeField] private float BurstSpeed;

    [Tooltip("Seconds per burst")]
    [SerializeField] private float FiringSpeed;

    [Tooltip("Meters per second^s per shot")]
    [SerializeField] private float Kickback;

    [Header("Bullet Settings")]

    [SerializeField] private int ProjectilesPerShot = 1;

    [Tooltip("[Range, Damage, Knockback] per projectile")]
    [SerializeField] private Ammo Ammo;

    [SerializeField] private float BulletSpeed = 1;

    [SerializeField] private int Bounces = 0;

    [Header("Reload Settings")]

    [SerializeField] private bool MagReload;

    [Tooltip("Seconds per bullet (or mag)")]
    [SerializeField] public float ReloadSpeed;

    [SerializeField] private int Capacity;

    [Header("Aiming")]

    [SerializeField] private float AngleRandomness = 0;

    [Header("Projectile Prefab")]

    [SerializeField] private GameObject ProjectilePrefab;

    private void OnEnable()
    {
        StartCoroutine(FireCooldown());
    }

    private void Awake()
    {
        hitMask = LayerMask.GetMask("EnemyTriggers", "Walls");
        Muzzle = transform.Find("Muzzle");
        Chamber = Capacity;
        onBoard = transform.parent.GetComponent<OnBoard_MovingObject>(); // Parent must have this script
    }

    public void Fire()
    {
        Debug.Log("Gunfire request received");

        // Check that fire request is valid
        if (Shooting) return;
        if (Chamber <= 0) { Reload(); return; }
        StopReloading();

        Debug.Log("Gunfire request validated");

        StartCoroutine(FireBurst());

        //Start cooldown
        StartCoroutine(FireCooldown());

    }

    IEnumerator FireBurst()
    {
        for(int i = 0; i < Burst; i++)
        {
            if (Chamber > 0)
            {
                FireShot();
                yield return new WaitForSeconds(BurstSpeed);
            }
        }
    }

    private void FireShot()
    {
        for(int j = 0; j < ProjectilesPerShot; j++)
        {
            FireProjectile();
        }
        onBoard.momentum = onBoard.momentum - (Vector2)transform.right * Kickback;
        Chamber--;
    }

    private void FireProjectile()
    {
        float angle = transform.eulerAngles.z + Random.Range(-AngleRandomness, AngleRandomness);
        GameObject obj =
           Instantiate(ProjectilePrefab,
                       Muzzle.transform.position,
                       Quaternion.Euler(0f, 0f, angle));

        Projectile projectile_script = obj.GetComponent<Projectile>();
        projectile_script.Ammo = Ammo;
        projectile_script.BouncesLeft = Bounces;
        OnBoard proj_OnBoard = obj.GetComponent<OnBoard>();

        Vector2 dir = new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad)
        ).normalized;

        obj.GetComponent<OnBoard>().momentum = onBoard.movingObject? 
                                                onBoard.momentum + onBoard.movingObject.vel + dir * BulletSpeed :
                                                onBoard.momentum + dir * BulletSpeed;
    }

    IEnumerator FireCooldown()
    {
        Shooting = true;
        yield return new WaitForSeconds(FiringSpeed);
        Shooting = false;
    }

    public void Reload()
    {
        if (ReloadCoroutine == null) ReloadCoroutine = StartCoroutine(ReloadTimer());
    }

    private void StopReloading()
    {
        if (ReloadCoroutine != null)
        {
            // Interupt reloading
            StopCoroutine(ReloadCoroutine);
            ReloadCoroutine = null;
            ReloadProg = 0f;
        }
    }

    IEnumerator ReloadTimer()
    {

        // Reload mag once
        if (MagReload)
        {
            while (ReloadProg <= ReloadSpeed)
            {
                yield return null;
                ReloadProg += Time.deltaTime;
            }
            Chamber = Capacity;
        }
        else
        {
            while (Chamber < Capacity)
            {
                while (ReloadProg <= ReloadSpeed)
                {
                    yield return null;
                    ReloadProg += Time.deltaTime;
                }
                Chamber++;
                ReloadProg = 0f;
            }
        }

        ReloadProg = 0f;
        ReloadCoroutine = null;
    }
}