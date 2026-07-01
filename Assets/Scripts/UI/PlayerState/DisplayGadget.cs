using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

    private void ShowDisplay(bool show)
    {
        Icon.enabled = show;
        Cooldown1.enabled = show;
        Cooldown2.enabled = show;
    }

    void Update()
    {
        if (CaptainHandler.Instance == null) return;

        if (CaptainHandler.Instance.Gadgets.Count == 0) ShowDisplay(false);
        else
        {
            if (Gadget != CaptainHandler.Instance.Gadgets[CaptainHandler.Instance.CurrGadget])
            {
                Gadget = CaptainHandler.Instance.Gadgets[CaptainHandler.Instance.CurrGadget];
                Icon.sprite = Gadget.GetComponent<SpriteRenderer>().sprite;
            }

            if (Gadget.MidCooldown) Cooldown2.fillAmount = 1f - Gadget.CooldownProg / Gadget.Cooldown;
            else Cooldown2.fillAmount = 0f;

            if (Gadget.MidUse) Cooldown1.fillAmount = 1f - Gadget.UseProg / Gadget.UseTime;
            else Cooldown1.fillAmount = 0f;

            ShowDisplay(true);
        }
    }
}
