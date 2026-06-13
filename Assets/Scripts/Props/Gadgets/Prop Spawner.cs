using Unity.VisualScripting;
using UnityEngine;

public class PropSpawner : Gadget
{
    [SerializeField] private GameObject Grenade;
    [SerializeField] private float ThrowForce;
    private OnBoard OnBoard;
    private MovingObject MovingObject;

    private void Awake()
    {
        OnBoard = GetComponentInParent<OnBoard>();
        MovingObject = GetComponentInParent<MovingObject>();
        if (OnBoard == null) Debug.Log("ERROR: Parent of shock grenade does not contain OnBoard");
        GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Use()
    {
        GameObject grenade = Instantiate(Grenade, transform.position, transform.rotation);
        OnBoard onBoard = grenade.GetComponent<OnBoard>();
        if (onBoard == null) Debug.Log("ERROR: Grenade prefab does not contain onBoard");
        else
        {
            onBoard.momentum = OnBoard.momentum + InputHandler.Instance.LookReadValue().normalized * ThrowForce;
            if (MovingObject != null) onBoard.momentum += MovingObject.vel;
            else Debug.Log("No movingObject parent detected");
        }
        StartCoroutine(CooldownRoutine());
    }

    public override void Deactivate()
    {

    }
}
