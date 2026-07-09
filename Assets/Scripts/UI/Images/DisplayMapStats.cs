using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayMapStats : MonoBehaviour
{
    [SerializeField] private Image EscapePodStar;
    [SerializeField] private Image ShipControlStar;
    [SerializeField] private TextMeshProUGUI TimeText;

    [Header("Preferences")]
    [SerializeField] private Color OffColor;
    [SerializeField] private Color OnColor;

    private void Update()
    {
        if (GameHandler.Instance == null) return;

        SaveData SaveData = GameHandler.Instance.GetSaveData();

        if (SaveData.aegisData.escapePod) EscapePodStar.color = OnColor;
        else EscapePodStar.color = OffColor;

        if (SaveData.aegisData.shipControl) ShipControlStar.color = OnColor;
        else ShipControlStar.color = OffColor;

        TimeText.text = "Longest survived: " + SaveData.aegisData.time.ToString("F2") + "s";
        if (SaveData.aegisData.time < 1f) TimeText.text = "";
    }
}
