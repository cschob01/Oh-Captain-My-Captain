using Unity.VisualScripting;
using UnityEngine;

public class HandleGadgets : MonoBehaviour
{
    public GameObject StartingGadget;
    public InputHandler Handler;

    private GameObject obj = null;
    private Gadget gadget = null;

    public Vector2 dir;
    private void Start()
    {
        //Set up gadget using prefab
        GadgetChange(StartingGadget);
    }

    private void GadgetChange(GameObject Gadget)
    {
        LoseGadget();
        if (Gadget != null)
        {
            obj = Instantiate(Gadget, transform.position, Quaternion.identity, transform);
            gadget = obj.GetComponent<Gadget>();
        }
        EventHandler.Instance.GadgetChange(obj);
    }

    private void LoseGadget()
    {
        if (obj != null) Destroy(obj);
        gadget = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (obj != null && gadget != null)
        {
            // Use gadget if requested
            if (Handler.controls.Gadget.Use.WasPressedThisFrame()) gadget.Activate(); // Try to activate gadget
            if (Handler.controls.Gadget.Use.WasReleasedThisFrame()) gadget.Deactivate(); // Try to activate gadget
        }
    }
}
