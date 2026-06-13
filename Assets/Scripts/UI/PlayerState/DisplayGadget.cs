using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayGadget : MonoBehaviour
{
    private Image Icon;
    private Image Cooldown1;
    private Image Cooldown2;

    private Gadget Gadget;

    private void Awake()
    {
        Icon = transform.Find("Icon").GetComponent<Image>();
        Cooldown1 = transform.Find("Cooldown1").GetComponent<Image>();
        Cooldown2 = transform.Find("Cooldown2").GetComponent<Image>();
        ShowDisplay(false);
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
            if (Gadget.MidCooldown) Cooldown2.fillAmount = 1f - Gadget.CooldownProg / Gadget.Cooldown;
            else Cooldown2.fillAmount = 0f;

            if (Gadget.MidUse) Cooldown1.fillAmount = 1f - Gadget.UseProg / Gadget.UseTime;
            else Cooldown1.fillAmount = 0f;
        }
        else
        {
            Cooldown2.fillAmount = 0f;
            Cooldown1.fillAmount = 0f;
        }
    }

    private void ShowDisplay(bool show)
    {
        Icon.enabled = show;
        Cooldown1.enabled = show;
        Cooldown2.enabled = show;
    }

    private void SetGadget(GameObject gadget)
    {
        if (gadget != null)
        {
            Gadget = gadget.GetComponent<Gadget>();
            Icon.sprite = gadget.GetComponent<SpriteRenderer>().sprite;
            ShowDisplay(true);
        }
        else
        {
            ShowDisplay(false);
        }
    }
}
