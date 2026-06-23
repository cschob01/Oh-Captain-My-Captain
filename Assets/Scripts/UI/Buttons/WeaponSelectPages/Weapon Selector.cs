using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponSelector : MonoBehaviour
{
    private Gun GunPrefab;
    private Image Image;
    private TMP_Text Text;
    private Button Button;

    private void Awake()
    {
        Image = transform.Find("Image").GetComponent<Image>();
        Text = transform.Find("Text").GetComponent<TMP_Text>();

        Button = GetComponent<Button>();
        Button.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        CaptainHandler.Instance.AddGun(GunPrefab);
        EventHandler.Instance.RoundStart();
    }

    public void SetButton(Gun gun)
    {
        GunPrefab = gun;
        Text.text = gun.name;
        Image.sprite = gun.GetComponent<SpriteRenderer>().sprite;
    }
}
