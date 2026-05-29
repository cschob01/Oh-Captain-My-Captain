using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayAmmo : MonoBehaviour
{

    private TextMeshProUGUI Count;
    private Image Cooldown;

    private Gun Gun = null;

    private void Awake()
    {
        Count = transform.Find("Count").GetComponent<TextMeshProUGUI>();
        Cooldown = transform.Find("Cooldown").GetComponent<Image>();
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
        }
        else
        {
            Gun = null;
        }
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