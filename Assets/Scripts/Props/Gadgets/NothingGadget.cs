using UnityEngine;

public class NothingGadget : Gadget
{

    private void Awake()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
    protected override void Use()
    {

    }

    protected override void Disuse()
    {
        
    }
}
