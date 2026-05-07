using System.Collections;
using UnityEngine;
using static UnityEngine.UI.Image;

public class HandCannon : Gun
{
    public int capacity { get; private set; } = 1;
    public int chamber { get; private set; }

    public float reload_time = 2f; // Bullet per sec
    public float fire_time = .3f; // Bullet per sec

    private Coroutine reloadCoroutine;
    public bool reloading = false;
    public bool shooting = false;

    public int damage = 30;
    public float kickback = 1.4f;

    public DisplayAmo displayAmo;

    public OnBoard onBoard;

    void Awake()
    {
        displayAmo = FindFirstObjectByType<DisplayAmo>();
        chamber = capacity;
        displayAmo.SetAmo(chamber);
        onBoard = transform.parent.GetComponent<OnBoard>(); // Parent must have this script
    }

    public override void Fire(Vector2 dir)
    {
        if (shooting) return;
        if (chamber <= 0) { Reload(); return; }
        StopReloading();

        chamber--;
        displayAmo.SetAmo(chamber);

        //FIRE SHIT
        CastRay(dir);
        CastRay(dir);
        CastRay(dir);
        CastRay(dir);

        onBoard.momentum = onBoard.momentum - dir * kickback;

        //Start cooldown
        StartCoroutine(FireTimer());

        //Debug.Log("FIRED! Current: " + chamber);
    }

    public void CastRay(Vector2 dir)
    {
        Vector3 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos, dir, range, hitMask);
        Vector2 endPoint;

        if (hit.collider != null)
        {
            hit.collider.GetComponent<Health>()?.TakeDamage(10, dir * knockback);
            endPoint = hit.point;
        }
        else
        {
            endPoint = (Vector2)pos + dir * range;
        }

        SpawnLaser(pos, endPoint);
    }

    public override void Reload()
    {
        if (!reloading) reloadCoroutine = StartCoroutine(ReloadTimer());
    }

    public void StopReloading()
    {
        if (reloadCoroutine != null)
        {
            Debug.Log("STOPPING COROUTINE");
            StopCoroutine(reloadCoroutine);
            reloadCoroutine = null;
            reloading = false;
        }
        else
        {
            Debug.Log("Nothing to stop");
        }
    }

    IEnumerator ReloadTimer()
    {
        reloading = true;

        while ((chamber < capacity) && reloading)
        {
            yield return new WaitForSeconds(reload_time);
            chamber++;
            displayAmo.SetAmo(chamber);
        }

        reloading = false;
    }

    IEnumerator FireTimer()
    {
        shooting = true;
        yield return new WaitForSeconds(fire_time);
        shooting = false;
    }

}
