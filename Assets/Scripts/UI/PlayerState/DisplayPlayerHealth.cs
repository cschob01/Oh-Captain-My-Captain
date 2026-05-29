using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DisplayPlayerHealth : MonoBehaviour
{
    public TextMeshProUGUI healthText;

    private void Awake()
    {
        PlayerHealth health = FindAnyObjectByType(typeof(PlayerHealth)).GetComponent<PlayerHealth>();
        if (health != null) SetHealth(health.health);
    }
    private void SetHealth(int health)
    {
        healthText.text = "X" + health;
    }
    private void OnEnable()
    {
        EventHandler.Instance.OnPlayerHealthChange += SetHealth;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnPlayerHealthChange -= SetHealth;
    }
}
