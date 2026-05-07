using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    // Scroll changes the speed at which the backround moves to the ships 
    // velocity. Smaller scroll scale will make the planets look further away.
    public float scrollScale = 0.1f;

    private Renderer rend;
    private Vector2 offset;
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void FixedUpdate()
    {
        // Offset to where ship is in space
        offset.x = Ship.Instance.global_pos.x * scrollScale;
        offset.y = Ship.Instance.global_pos.y * scrollScale;
        rend.material.mainTextureOffset = offset;

        //Follow ship's rotation
        transform.rotation = Quaternion.Euler(0, 0, Ship.Instance.global_angle);
    }
}