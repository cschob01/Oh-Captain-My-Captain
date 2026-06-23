using UnityEngine;
using UnityEngine.Tilemaps;

public class AndGate : MonoBehaviour
{
    [SerializeField] private string[] Conditions;
    [SerializeField] private string Beat;

    [Tooltip("Continues firing when an above event is called after each has been called at least once.")]
    [SerializeField] private bool Continuous;

    private bool[] active;

    private void Awake()
    {
        active = new bool[Conditions.Length];
    }

    private void OnEnable()
    {
        EventHandler.Instance.OnBeatChange += OnBeat;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnBeatChange -= OnBeat;
    }

    private void OnBeat(string beat)
    {
        bool found = false;
        for (int i = 0; i < Conditions.Length; i++)
        {
            if (Conditions[i] == beat)
            {
                active[i] = true;
                found = true;
            }
        }

        if (!found) return;

        bool AllTrue = true;
        for (int i = 0; i < Conditions.Length; i++)
        {
            if (!active[i]) AllTrue = false;
        }
        if (AllTrue) {
            EventHandler.Instance.BeatChange(Beat);
            if (!Continuous) Destroy(this);
        }
    }

}
