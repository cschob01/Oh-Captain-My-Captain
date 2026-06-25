using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPlayerHealth : MonoBehaviour
{
    private Image Bar;
    private Image Border;
    private PlayerHealth Health;

    private void Awake()
    {
        Bar = transform.Find("BarMask").GetComponent<Image>();
        Border = transform.Find("Border").GetComponent<Image>();
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

        Bar.fillAmount = Mathf.Clamp01(Health.health / Health.MaxHealth);
    }

    private void ShowDisplay(bool show)
    {
        Bar.enabled = show;
        Border.enabled = show;

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
