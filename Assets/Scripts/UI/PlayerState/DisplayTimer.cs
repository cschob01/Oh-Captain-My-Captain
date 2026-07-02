using UnityEngine;
using TMPro;

public class DisplayTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Count;

    private void Awake()
    {
        ShowDisplay(false);
    }

    private void ShowDisplay(bool show)
    {
        Count.enabled = show;
    }

    private void UpdateDisplay()
    {
        if (Timer.Instance != null)
        {
            ShowDisplay(true);
            Count.text = Timer.Instance.TimeProg.ToString("F2");
        }
        else Count.text = "";
    }

    void Update()
    {
        UpdateDisplay();
    }
}
