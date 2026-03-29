using UnityEngine;

public class VelocityVisualizer : MonoBehaviour
{
    [Tooltip("Assign your Controls script here in the Inspector.")]
    public MovingObject movingObject;  // Drag the Controls component here in Inspector

    void Update()
    {
        if (movingObject != null)
        {
            // Set this GameObject's position based on the Controls.vel vector
            // If working in 2D, we'll preserve z = 0
            transform.localPosition = new Vector3(movingObject.vel.x, movingObject.vel.y, transform.position.z) * .35f;
        }
    }
}
