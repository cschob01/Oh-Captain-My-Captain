using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayGadget : MonoBehaviour
{
    private Image Icon;
    private Image Cooldown;

    private Gadget Gadget;

    private void Awake()
    {
        Icon = transform.Find("Icon").GetComponent<Image>();
        Cooldown = transform.Find("Cooldown").GetComponent<Image>();
    }

    private void OnEnable()
    {
        EventHandler.Instance.OnGadgetChange += SetGadget;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnGadgetChange -= SetGadget;
    }

    // Update is called once per frame
    void Update()
    {
        if (Gadget != null)
        {
            Cooldown.fillAmount = Gadget.CooldownProg / Gadget.Cooldown;
        }
        else
        {
            Cooldown.fillAmount = 0f;
        }
    }

    private void SetGadget(GameObject gadget)
    {
        if (gadget != null)
        {
            Gadget = gadget.GetComponent<Gadget>();
            Icon.sprite = gadget.GetComponent<SpriteRenderer>().sprite;
        }
        else
        {
            Icon = null;
            Gadget = null;
        }
    }
}
