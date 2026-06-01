using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class WaitForBeat0 : MonoBehaviour
{
    [SerializeField] float WaitTime = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(TriggerBeat0), WaitTime);
    }

    void TriggerBeat0()
    {
        EventHandler.Instance.BeatChange(0);
    }
}
