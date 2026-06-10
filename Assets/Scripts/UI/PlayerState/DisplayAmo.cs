using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayAmmo : MonoBehaviour
{

    private TextMeshProUGUI Count;
    private Image Cooldown;
    private Image Bullet;

    private Gun Gun = null;

    private void Awake()
    {
        Count = transform.Find("Count").GetComponent<TextMeshProUGUI>();
        Bullet = transform.Find("Bullet").GetComponent<Image>();
        Cooldown = transform.Find("Cooldown").GetComponent<Image>();
        ShowDisplay(false);
    }

    private void OnEnable()
    {
        EventHandler.Instance.OnGunChange += SetGun;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnGunChange -= SetGun;
    }

    private void SetGun(GameObject gun)
    {
        if (gun != null)
        {
            Gun = gun.GetComponent<Gun>();
            ShowDisplay(true);
        }
        else
        {
            Gun = null;
            ShowDisplay(false);
        }
    }

    private void ShowDisplay(bool show)
    {
        Bullet.enabled = show;
        Count.enabled = show;
        Cooldown.enabled = show;
    }

    private void Update()
    {
        if (Gun == null)
        {
            Count.text = "";
            Cooldown.fillAmount = 0f;
        }
        else
        {
            Count.text = "X" + Gun.Chamber;
            Cooldown.fillAmount = Gun.ReloadProg / Gun.ReloadSpeed;
        }
    }


}