using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class WaitForBeat : MonoBehaviour
{
    [SerializeField] float WaitTime = 1f;
    [SerializeField] string beat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (WaitTime == 0) EventHandler.Instance.BeatChange(beat);
        else Invoke(nameof(TriggerBeat0), WaitTime);
    }

    void TriggerBeat0()
    {
        EventHandler.Instance.BeatChange("0");
    }
}
