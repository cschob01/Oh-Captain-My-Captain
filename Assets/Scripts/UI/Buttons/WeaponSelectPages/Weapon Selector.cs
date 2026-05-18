using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponSelector : MonoBehaviour
{
    public GameObject GunPrefab;

    [SerializeField] private Image Image;
    [SerializeField] private TMP_Text Text;
    private Button Button;

    private void Awake()
    {
        Button = GetComponent<Button>();
        Button.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        EventHandler.Instance.GunChange(GunPrefab);
        EventHandler.Instance.RoundStart();
    }

    public void SetButton(GameObject Gun)
    {
        GunPrefab = Gun;
        Text.text = Gun.name;
        Image.sprite = Gun.GetComponent<SpriteRenderer>().sprite;
    }
}
