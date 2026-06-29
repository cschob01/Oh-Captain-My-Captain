using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DisplayPlayerHealth : MonoBehaviour
{
    private Image Bar;
    private Image Border;
    private PlayerHealth Health;

    private int prevHealth;

    [SerializeField] private Animator WarningBackdrop;
    [SerializeField] private Animator HealingBackdrop;

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

        float healthPercent = Mathf.Clamp01(Health.health / Health.MaxHealth);
        if (prevHealth != Health.health)
        {
            DOTween.Kill(Bar);
            Bar.DOFillAmount(healthPercent, .3f);
        }

        HealingBackdrop.SetBool("Active", Health.healing);
        WarningBackdrop.SetBool("Active", !Health.healing && healthPercent < .4f);
    }

    private void ShowDisplay(bool show)
    {
        Bar.enabled = show;
        Border.enabled = show;

    }

    private void OnDestroy()
    {
        DOTween.Kill(Bar);
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
