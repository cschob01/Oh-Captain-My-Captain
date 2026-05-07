using System.Collections;
using UnityEngine;
using static UnityEngine.UI.Image;

public class M1919 : Gun
{
    public int capacity { get; private set; } = 100;
    public int chamber { get; private set; }

    public float reload_time = 5f; // Seconds per bullet
    public float fire_time = .1f; // Seconds per bullet

    private Coroutine reloadCoroutine;
    public bool reloading = false;
    public bool shooting = false;

    public int damage = 3;
    public float kickback = .05f;

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
        // Check that fire request is valid
        if (shooting) return;
        if (chamber <= 0) { Reload(); return; }
        StopReloading();

        chamber--;
        displayAmo.SetAmo(chamber);

        //Fire
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
            //Interupt reloading
            StopCoroutine(reloadCoroutine);
            reloadCoroutine = null;
            reloading = false;
        }
    }

    IEnumerator ReloadTimer()
    {
        reloading = true;

        // Reoad mag all at once
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
