using TMPro;
using UnityEngine;

public class DisplayMoney : MonoBehaviour
{
    private TextMeshProUGUI Money;
    private int previous_count = 0;

    [Tooltip("Expects elemnt to have TextMeshProUGUI")]
    [SerializeField] private GameObject MoneyUpdateDisplay;

    void Awake()
    {
        Money = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CaptainHandler.Instance != null)
        {
            Money.text = "$" + CaptainHandler.Instance.Money.ToString();
            int UpdateMoney = CaptainHandler.Instance.Money - previous_count;
            Debug.Log("UpdateMoney: " + UpdateMoney);
            Debug.Log("MoneyUpdateDisplay: " + MoneyUpdateDisplay);
            if (UpdateMoney != 0 && MoneyUpdateDisplay != null)
            {
                Debug.Log("Spawning MoneyUpdateDisplay");
                TextMeshProUGUI UpdateText = Instantiate(MoneyUpdateDisplay, transform).GetComponent<TextMeshProUGUI>();
                UpdateText.text = "$" + UpdateMoney.ToString();
                if (UpdateMoney < 0) UpdateText.color = Color.red;
                else UpdateText.color = Color.green;
                previous_count = CaptainHandler.Instance.Money;
            }
        }
    }
}
