using UnityEngine;
using TMPro;

public class DisplayTimer : MonoBehaviour
{
    [SerializeField] private Color LowRush;
    [SerializeField] private Color HighRush;
    [SerializeField] private Color Failed;

    private TextMeshProUGUI Count;
    private TextMeshProUGUI Max;
    private Timer Timer;

    private void Awake()
    {
        Count = transform.Find("Count").GetComponent<TextMeshProUGUI>();
        Max = transform.Find("Max").GetComponent<TextMeshProUGUI>();

        ShowDisplay(false);
    }

    private void ShowDisplay(bool show)
    {
        Max.enabled = show;
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
            Max.text = Timer.MaxTime.ToString("F2");

            if (Timer.TimeProg < .7f * Timer.MaxTime)
            {
                Count.color = LowRush;
                Max.color = LowRush;
            }
            else if (Timer.TimeProg > Timer.MaxTime)
            {
                Count.color = Failed;
                Max.color = Failed;
            }
            else
            {
                Count.color = HighRush;
                Max.color = HighRush;
            }
        }
    }

    void Update()
    {
        UpdateDisplay();
    }
}
