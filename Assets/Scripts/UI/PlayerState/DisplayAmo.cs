using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayAmmo : MonoBehaviour
{

    private TextMeshProUGUI Count;
    private Image Cooldown;
    private Image Bullet;

    private void Awake()
    {
        Count = transform.Find("Count").GetComponent<TextMeshProUGUI>();
        Bullet = transform.Find("Bullet").GetComponent<Image>();
        Cooldown = transform.Find("Cooldown").GetComponent<Image>();
        ShowDisplay(false);
    }

    private void ShowDisplay(bool show)
    {
        Bullet.enabled = show;
        Count.enabled = show;
        Cooldown.enabled = show;
    }

    private void Update()
    {
        if (CaptainHandler.Instance == null) return;

        if (CaptainHandler.Instance.Guns.Count == 0) ShowDisplay(false);
        else
        {
            ShowDisplay(true);

            Gun gun = CaptainHandler.Instance.Guns[CaptainHandler.Instance.CurrGun];
            Count.text = "X" + gun.Chamber;
            Cooldown.fillAmount = gun.ReloadProg / gun.ReloadSpeed;
        }
    }
}