using System.Collections;
using UnityEngine;
using static UnityEngine.UI.Image;

public class HandCannon : Gun
{
    public int capacity { get; private set; } = 1;
    public int chamber { get; private set; }

    public float reload_time = 2f; // Seconds per bullet
    public float fire_time = .3f; // Seconds per bullet 

    private Coroutine reloadCoroutine;
    public bool reloading = false;
    public bool shooting = false;

    public int damage = 30;
    public float kickback = 1.4f;

    public DisplayAmo displayAmo;

    // Allows gun to apply kickback to player
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
        // Check that request to fire is valid
        if (shooting) return;
        if (chamber <= 0) { Reload(); return; }
        StopReloading();

        chamber--;
        displayAmo.SetAmo(chamber);

        //Fire four bullets (Hand cannon is strong)
        CastRay(dir);
        CastRay(dir);
        CastRay(dir);
        CastRay(dir);

        // Apply kickback
        onBoard.momentum = onBoard.momentum - dir * kickback;

        //Start cooldown
        StartCoroutine(FireTimer());
    }

    public override void Reload()
    {
        if (!reloading) reloadCoroutine = StartCoroutine(ReloadTimer());
    }

    public void StopReloading()
    {
        if (reloadCoroutine != null)
        {
            // Interupt reloading process
            StopCoroutine(reloadCoroutine);
            reloadCoroutine = null;
            reloading = false;
        }
    }

    IEnumerator ReloadTimer()
    {
        reloading = true;

        // Add bullets until capacity is full
        while (chamber < capacity)
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
