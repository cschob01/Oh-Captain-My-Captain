using UnityEngine;
using Pathfinding;

// This enemy uses a BFS to search for the shortest path to the player on the map. 
// Credit to A* Pathfinding Project library for implementation of graph setup and
// search.
public class NPCPathing1 : MovingObject
{
    public Transform target;
    public LayerMask wallMask;

    Seeker seeker;
    Path path;
    int currentWaypoint = 0;

    public float waypointDistance = 0.1f;

    private bool line_of_sight = true;

    void Start()
    {
        target = CaptainHandler.Instance.transform;
        seeker = GetComponent<Seeker>();
        InvokeRepeating(nameof(HasLineOfSight), 0f, 0.5f); // Continually update if enemy sees the player
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

    // Updates line_of_sight depending on if direct path from enemy to player exists
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

    protected override Vector2 GetDir()
    {
        if (line_of_sight) // Head directly to player
        {
            Vector2 start = transform.position;
            Vector2 end = target.position;
            Vector2 dir = (end - start);
            return dir;
        }
        else // Traverse BFS graph
        {
            if (path == null || path.vectorPath == null || path.vectorPath.Count == 0)
                return Vector2.zero;

            if (currentWaypoint >= path.vectorPath.Count)
                return Vector2.zero;

            if (Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]) < waypointDistance)
            {
                currentWaypoint++;

                if (currentWaypoint >= path.vectorPath.Count)
                    return Vector2.zero;
            }

            Vector2 dir = (path.vectorPath[currentWaypoint] - (Vector3)transform.position);
            return dir;
        }
    }
}