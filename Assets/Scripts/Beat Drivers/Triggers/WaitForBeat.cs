using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class WaitForBeat : MonoBehaviour
{
    [SerializeField] float WaitTime = 1f;
    [SerializeField] string beat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (WaitTime == 0) TriggerBeat();
        else Invoke(nameof(TriggerBeat), WaitTime);
    }

    void TriggerBeat()
    {
        EventHandler.Instance.BeatChange(beat);
    }
}
