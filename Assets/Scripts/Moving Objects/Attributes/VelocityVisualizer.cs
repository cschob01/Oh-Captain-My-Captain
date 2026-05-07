using UnityEngine;

public class VelocityVisualizer : MonoBehaviour
{
    public MovingObject movingObject; // Should be the parent of this object

    void FixedUpdate()
    {
        if (movingObject != null)
        {
            // Set this GameObject's position based on the Controls.vel vector
            // If working in 2D, we'll preserve z = 0
            transform.localPosition = new Vector3(movingObject.vel.x, movingObject.vel.y, transform.position.z) * .35f;
        }
    }
}
