using TMPro;
using UnityEngine;

public class DisplayAmmo : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI amoText;

    private void OnEnable()
    {
        EventHandler.Instance.OnAmmoChange += SetAmmo;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnAmmoChange -= SetAmmo;
    }

    private void SetAmmo(int ammo)
    {
        amoText.text = "X" + ammo;
    }
}