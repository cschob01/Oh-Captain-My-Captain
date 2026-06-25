using TMPro;
using UnityEngine;

public class DisplayMoney : MonoBehaviour
{
    private TextMeshProUGUI Money;

    void Awake()
    {
        Money = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CaptainHandler.Instance != null)
        {
            Money.text = "$" + CaptainHandler.Instance.Money.ToString("F2");
        }
    }
}
