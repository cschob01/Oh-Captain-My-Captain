using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance;

    public bool TimeCap = false; 
    public float MaxTime = 20f;
    [HideInInspector] public float TimeProg = 0;
    private bool failed = false;
    private bool Stopped;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    private void OnEnable()
    {
        EventHandler.Instance.OnPlayerDied += OnPlayerDied;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnPlayerDied -= OnPlayerDied;
    }


    public void FixedUpdate()
    {
        if (Stopped) return;

        TimeProg += Time.fixedDeltaTime;
        if (TimeProg >= MaxTime && !failed && TimeCap)
        {
            EventHandler.Instance.LevelFailed();
            failed = true;
        }
    }

    private void OnPlayerDied()
    {
        Stopped = true;
    }

}
