using System.Collections;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Pistol : Gun
{
    public int capacity { get; private set; } = 6;
    public int chamber { get; private set; }

    public float reload_time = .75f; // Seconds per bullet
    public float fire_time = .3f; // Seconds per bullet

    Coroutine reloadCoroutine;
    public bool reloading = false;
    public bool shooting = false;

    public int damage = 10;

    public DisplayAmo displayAmo;

    void Awake()
    {
        displayAmo = FindFirstObjectByType<DisplayAmo>();
        chamber = capacity;
        displayAmo.SetAmo(chamber);
    }

    public override void Fire(Vector2 dir)
    {
        // Check that fire request is valid
        if (shooting) return; 
        if (chamber <= 0)  {  Reload(); return; }
        StopReloading();

        chamber--;
        displayAmo.SetAmo(chamber);

        //Fire
        CastRay(dir);

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
            // Interupt reloading
            StopCoroutine(reloadCoroutine);
            reloadCoroutine = null;
            reloading = false;
        }
    }

    IEnumerator ReloadTimer()
    {
        reloading = true;

        // Reload until chmaber is full
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
