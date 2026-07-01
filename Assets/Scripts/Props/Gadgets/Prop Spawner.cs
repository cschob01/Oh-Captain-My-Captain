using Unity.VisualScripting;
using UnityEngine;

public class PropSpawner : Gadget
{
    [SerializeField] private GameObject Prop;
    [SerializeField] private float ThrowForce;
    [Tooltip("<0 signifies forever")]
    [SerializeField] private float PropExistTime = -1;
    private OnBoard OnBoard;
    private MovingObject MovingObject;

    private void Awake()
    {
        OnBoard = GetComponentInParent<OnBoard>();
        MovingObject = GetComponentInParent<MovingObject>();
        if (OnBoard == null) Debug.Log("ERROR: Parent of PropSpawner does not contain OnBoard");
        if (OnBoard == null) Debug.Log("ERROR: Parent of PropSpawner does not contain movingObject");
        GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Use()
    {
        GameObject projectile = Instantiate(Prop, transform.position, transform.rotation);
        OnBoard onBoard = projectile.GetComponent<OnBoard>();
        if (onBoard == null)
        {
            Debug.Log("ERROR: Projectile prefab does not contain onBoard");
            return;
        }

        onBoard.momentum = OnBoard.momentum + InputHandler.Instance.LookReadValue().normalized * ThrowForce;
        if (MovingObject != null) onBoard.momentum += MovingObject.vel;

        if (PropExistTime > 0) Destroy(projectile, PropExistTime);
    }

    protected override void Disuse()
    {

    }
}
