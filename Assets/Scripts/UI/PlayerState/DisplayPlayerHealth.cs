using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPlayerHealth : MonoBehaviour
{
    private TextMeshProUGUI healthText;
    private Image Image;
    private PlayerHealth Health;

    private void Awake()
    {
        healthText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        Image = transform.Find("Image").GetComponent<Image>();
        ShowDisplay(false);
    }
    private void SetHealth(PlayerHealth health)
    {
        Health = health;
        if (health != null)
        {
            ShowDisplay(true);
        }
        else
        {
            ShowDisplay(false);
        }
    }

    private void Update()
    {
        if (Health == null) return;

        healthText.text = "X " + Health.health.ToString();
    }

    private void ShowDisplay(bool show)
    {
        Image.enabled = show;
        healthText.enabled = show;

    }
    private void OnEnable()
    {
        EventHandler.Instance.OnHealthChange += SetHealth;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnHealthChange -= SetHealth;
    }
}
