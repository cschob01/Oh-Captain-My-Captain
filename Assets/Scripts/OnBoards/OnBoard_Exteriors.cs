using UnityEngine;

// OnBoard is the foundation for on-ship physics
// Rather than moving the spaceship, objects inside that are given this script
// will move relative to the ship to create the illusion of the ship's
// movement. This avoids working with 2 moving colliders in Unity physics.
public class OnBoard_Exteriors : OnBoard
{
    private void Awake()
    {
        object_rb = GetComponent<Rigidbody2D>();
    }

    protected void FixedUpdate()
    {
        TransformOnBoard();
    }
}
