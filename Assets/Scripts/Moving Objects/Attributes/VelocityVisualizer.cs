using UnityEngine;

public class VelocityVisualizer : MonoBehaviour
{
    private MovingObject movingObject;
    [SerializeField] private float DistanceFactor = .35f;

    private void Awake()
    {
        movingObject = GetComponentInParent<MovingObject>();
    }

    void FixedUpdate()
    {
        if (movingObject != null)
        {
            // Set this GameObject's position based on the Controls.vel vector
            // If working in 2D, we'll preserve z = 0
            transform.localPosition = new Vector3(movingObject.vel.x, movingObject.vel.y, transform.position.z) * DistanceFactor;
        }
    }
}
