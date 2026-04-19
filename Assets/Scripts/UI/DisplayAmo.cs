using TMPro;
using UnityEngine;

public class DisplayAmo : MonoBehaviour
{

    public TextMeshProUGUI amoText;

    public void SetAmo(int amo)
    {
        amoText.text = "X" + amo;
    }
}