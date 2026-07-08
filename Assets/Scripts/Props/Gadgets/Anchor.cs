using UnityEngine;

public class Anchor : Gadget
{
    private OnBoard onBoard;

    private void Awake()
    {
        onBoard = GetComponentInParent<OnBoard>();
        GetComponent<SpriteRenderer>().enabled = false;
    }
    protected override void Use()
    {
        
    }

    protected override void Disuse()
    {
        
    }

    private void Update()
    {
        if (MidUse)
        {
            if (onBoard == null) return;

            Vector2 r = (Vector2)onBoard.transform.position - Ship.Instance.center;
            Vector2 tangentVel = new Vector2(-r.y, r.x) * Ship.Instance.spin;

            onBoard.momentum = Ship.Instance.vel - tangentVel;
        }
    }

}
