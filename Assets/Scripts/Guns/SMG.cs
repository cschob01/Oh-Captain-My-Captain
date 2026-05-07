using System.Collections;
using UnityEngine;

public class SMG : Gun
{
    public int capacity { get; private set; } = 15;
    public int chamber { get; private set; }

    public float reload_time = 5f; // Seconds per mag
    public float fire_time = .8f; // Seconds per bullet
    public float burst_time = .08f; // Seconds per burst shot

    private Coroutine reloadCoroutine;
    public bool reloading = false;
    public bool shooting = false;

    public int damage = 3;
    public float kickback = .1f;

    public DisplayAmo displayAmo;

    public OnBoard onBoard;
    public HandleGuns handleGuns;

    void Awake()
    {
        displayAmo = FindFirstObjectByType<DisplayAmo>();
        chamber = capacity;
        displayAmo.SetAmo(chamber);
        onBoard = transform.parent.GetComponent<OnBoard>(); // Parent must have this script
        handleGuns = transform.parent.GetComponent<HandleGuns>(); // Parent must have this script
    }

    public override void Fire(Vector2 dir)
    {
        // Check that fire request is valid
        if (shooting) return;
        if (chamber <= 0) { Reload(); return; }
        StopReloading();

        // Burst
        chamber -= 3;
        displayAmo.SetAmo(chamber);

        //FIRE
        StartCoroutine(BurstShot(dir));

        //Start cooldown
        StartCoroutine(FireTimer());
    }

    IEnumerator BurstShot(Vector2 dir)
    {
        // Fire thrice, each time applying kickback to player
        onBoard.momentum = onBoard.momentum - dir * kickback;
        CastRay(dir);
        yield return new WaitForSeconds(burst_time);
        onBoard.momentum = onBoard.momentum - dir * kickback;
        CastRay(handleGuns.dir);
        yield return new WaitForSeconds(burst_time);
        onBoard.momentum = onBoard.momentum - dir * kickback;
        CastRay(handleGuns.dir);
    }

    public override void Reload()
    {
        if (!reloading) reloadCoroutine = StartCoroutine(ReloadTimer());
    }

    public void StopReloading()
    {
        if (reloadCoroutine != null)
        {
            // Interupt reload
            StopCoroutine(reloadCoroutine);
            reloadCoroutine = null;
            reloading = false;
        }
    }

    IEnumerator ReloadTimer()
    {
        reloading = true;

        // Reload mag once
        yield return new WaitForSeconds(reload_time);
        chamber = capacity;
        displayAmo.SetAmo(chamber);

        reloading = false;
    }

    IEnumerator FireTimer()
    {
        shooting = true;
        yield return new WaitForSeconds(fire_time);
        shooting = false;
    }

}
