using UnityEngine;
using System.Collections;

public class Pistol : Gun
{
    public int capacity { get; private set; } = 6;
    public int chamber { get; private set; } = 6;

    private float reload_time = .75f; // Bullet per sec
    private float fire_time = .3f; // Bullet per sec
    private bool reloading = false;
    private bool shooting = false;

    void Awake()
    {
    }

    public override void Fire()
    {
        if (shooting) return; 
        if (chamber <= 0)  {  Reload(); return; }
        reloading = false;

        //Start cooldown
        StartCoroutine(FireTimer());
        chamber--;

        //FIRE SHIT
        Debug.Log("FIRED! Current: " + chamber);
    }
    public override void Reload()
    {
        if (!reloading) StartCoroutine(ReloadTimer());
    }

    IEnumerator ReloadTimer()
    {
        reloading = true;

        while ((chamber < capacity) && reloading)
        {
            yield return new WaitForSeconds(reload_time);
            if (reloading) chamber++;
            Debug.Log("Loaded one bullet. Current: " + chamber);
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
