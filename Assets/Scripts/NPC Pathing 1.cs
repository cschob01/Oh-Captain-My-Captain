using UnityEngine;
using Pathfinding;

public class NPCPathing1 : MovingObject
{
    public Transform target;
    public LayerMask wallMask;

    public float acc = .04f;
    public float max_vel = 3;

    Seeker seeker;
    Path path;
    int currentWaypoint = 0;

    public float waypointDistance = 0.1f;

    private bool line_of_sight = true;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        InvokeRepeating(nameof(HasLineOfSight), 0f, 0.5f);
    }

    void UpdatePath()
    {
        if (target != null)
            seeker.StartPath(transform.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    void HasLineOfSight()
    {
        Vector2 start = transform.position;
        Vector2 end = target.position;

        RaycastHit2D hit = Physics2D.Linecast(start, end, wallMask);

        bool res = hit.collider == null;

        if (!res) // Doesn't have line of sight
        {
            if (line_of_sight) // Previously had line of sight
            {
                InvokeRepeating(nameof(UpdatePath), 0f, 0.5f);
                //Debug.Log("Invoking");
            }
        }
        else
        {
            CancelInvoke(nameof(UpdatePath));
            //Debug.Log("Cancel Invoking");
        }
            line_of_sight = res;
    }

    protected override void SetVel()
    {
        if (line_of_sight)
        {
            //Debug.Log("LINE OF SIGHT");
            Vector2 start = transform.position;
            Vector2 end = target.position;
            Vector2 dir = (end - start).normalized;
            vel += acc * dir;

            ////Clamp velocity
            vel = Vector2.ClampMagnitude(vel, max_vel);
        }
        else
        {
            //Debug.Log("NO LINE OF SIGHT");
            if (path == null || currentWaypoint >= path.vectorPath.Count) return;

            Vector2 dir = ((Vector2)path.vectorPath[currentWaypoint] - (Vector2)transform.position).normalized;
            vel += acc * dir;

            if (Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]) < waypointDistance)
                currentWaypoint++;
        }
    }
}