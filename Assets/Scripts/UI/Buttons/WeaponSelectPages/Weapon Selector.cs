using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponSelector : MonoBehaviour
{
    public HandleGuns Player;

    private GameObject GunPrefab;
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
        Player.GunChange(GunPrefab);
        EventHandler.Instance.RoundStart();
    }

    public void SetButton(GameObject Gun)
    {
        GunPrefab = Gun;
        Text.text = Gun.name;
        Image.sprite = Gun.GetComponent<SpriteRenderer>().sprite;
    }
}
