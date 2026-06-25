using UnityEngine;
using TMPro;

public class DisplayTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Count;
    private Timer Timer;

    private void Awake()
    {
        ShowDisplay(false);
    }

    private void ShowDisplay(bool show)
    {
        Count.enabled = show;
    }

    private void OnEnable()
    {
        EventHandler.Instance.OnTimerChange += OnTimerChange;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnTimerChange -= OnTimerChange;
    }

    private void OnTimerChange(Timer timer)
    {
        Timer = timer;
        if (Timer == null) ShowDisplay(false);
        else {
            UpdateDisplay();
            ShowDisplay(true);
        }
    }

    private void UpdateDisplay()
    {
        if (Timer != null)
        {
            Count.text = Timer.TimeProg.ToString("F2");
        }
    }

    void Update()
    {
        UpdateDisplay();
    }
}
