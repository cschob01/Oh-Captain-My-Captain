using UnityEngine;

public class Grounder : Gadget
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
        onBoard.momentum = Vector2.zero;
    }

    public override void Deactivate()
    {
        
    }
}
