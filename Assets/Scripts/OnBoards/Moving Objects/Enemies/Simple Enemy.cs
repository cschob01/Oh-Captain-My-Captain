using UnityEngine;

// This enemy simply moves towards the player at all times, ignorant
// of walls and map layout
public class SimpleEnemy : MovingObject
{
    public GameObject objective;

    // Called once every fixedUpdate
    protected override Vector2 GetDir()
    {
        Vector2 pos = transform.position;
        Vector2 target = objective.transform.position;
        Vector2 dir = (target - pos);
        return dir;
    }
}
