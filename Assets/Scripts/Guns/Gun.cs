using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public abstract void Fire(Vector2 dir);
    public abstract void Reload();

    public GameObject laserPrefab;
    public int range = 100;
    public float knockback = .1f;
    public LayerMask hitMask;

    protected void SpawnLaser(Vector2 start, Vector2 end)
    {
        GameObject laser = Instantiate(laserPrefab);

        Vector2 dir = (end - start);
        float distance = dir.magnitude;

        // Position at start
        laser.transform.position = start;

        // Rotate to face direction
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        laser.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Scale along X axis
        laser.transform.localScale = new Vector3(distance, 1f, 1f);

        // Set parent to us
        laser.transform.parent = transform;

        // Optional: destroy after short time
        Destroy(laser, 0.05f);
    }

    protected void CastRay(Vector2 dir)
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
}