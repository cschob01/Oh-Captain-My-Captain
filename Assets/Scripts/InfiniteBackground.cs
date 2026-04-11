using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    public Transform cam;
    public float scrollScale = 0.1f;

    private Renderer rend;
    private Vector2 offset;

    //public float follow_speed = 1f;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void FixedUpdate()
    {
        offset.x = Ship.Instance.global_pos.x * scrollScale;
        offset.y = Ship.Instance.global_pos.y * scrollScale;
        transform.rotation = Quaternion.Euler(0, 0, Ship.Instance.global_angle);
        rend.material.mainTextureOffset = offset;
        //Debug.Log(offset);

    }
}