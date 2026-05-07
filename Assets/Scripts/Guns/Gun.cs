using UnityEngine;

// Gun
// This is the abstract class defining the interface for different gun types 
public abstract class Gun : MonoBehaviour
{
    // Only two player interactions
    public abstract void Fire(Vector2 dir);
    public abstract void Reload();

    // Gun statistics
    public GameObject laserPrefab;
    public int range = 100;
    public float knockback = .1f;
    public LayerMask hitMask;
    public float lazer_time = .1f;

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

        // Destroy after short time
        Destroy(laser, lazer_time);
    }

    protected void CastRay(Vector2 dir)
    {
        // Detects which object of hitmask was been hit
        Vector3 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos, dir, range, hitMask);

        // Create appropriate line segment
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