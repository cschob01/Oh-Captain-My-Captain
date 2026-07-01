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
            if (onBoard != null) onBoard.momentum = Ship.Instance.vel;
        }
    }


}
