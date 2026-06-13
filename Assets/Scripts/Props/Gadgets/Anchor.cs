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
        StartCoroutine(CooldownRoutine());
        onBoard.momentum = Ship.Instance.vel;
    }

    protected override void Disuse()
    {
        
    }


}
