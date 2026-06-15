using UnityEngine;

public class Timer : MonoBehaviour
{
    public float MaxTime = 20f;
    [HideInInspector] public float TimeProg = 0;
    private bool failed = false;

    private void Start()
    {
        EventHandler.Instance.TimerChange(this);
    }


    public void FixedUpdate()
    {
        TimeProg += Time.fixedDeltaTime;
        if (TimeProg >= MaxTime && !failed)
        {
            EventHandler.Instance.LevelFailed();
            failed = true;
        }
    }

}
