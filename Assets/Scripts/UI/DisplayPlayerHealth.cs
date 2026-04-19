using TMPro;
using UnityEngine;

public class DisplayPlayerHealth : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public void SetHealth(int health)
    {
        healthText.text = "X" + health;
    }
}
