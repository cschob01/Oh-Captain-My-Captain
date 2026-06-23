using System;
using UnityEngine;
using UnityEngine.Android;

public class EventHandler : MonoBehaviour
{
    public static EventHandler Instance;

    public event Action OnPlayerDied;
    public event Action OnEnemyDied;
    public event Action OnRoundStart;
    public event Action OnRoundEnd;
    public event Action<int> OnRoundChange;

    public event Action<Timer> OnTimerChange;
    public event Action<PlayerHealth> OnHealthChange;

    /// Beats
    public event Action<string> OnBeatChange;
    public event Action OnLevelCompleted;
    public event Action OnLevelFailed;

    public event Action<bool> OnGamePause;

    public event Action<Vector2, float, float> OnExplosion;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void TimerChange(Timer timer)
    {
        OnTimerChange?.Invoke(timer);
    }

    public void LevelFailed()
    {
        OnLevelFailed?.Invoke();
    }

    public void Explosion(Vector2 Coordinates, float Force, float Radius)
    {
        OnExplosion?.Invoke(Coordinates, Force, Radius);
    }

    public void HealthChange(PlayerHealth Health)
    {
        OnHealthChange?.Invoke(Health);
    }

    public void LevelCompleted()
    {
        OnLevelCompleted?.Invoke();
    }

    public void GamePause(bool pause)
    {
        OnGamePause?.Invoke(pause);
    }

    public void BeatChange(string beat)
    {
        Debug.Log(beat + " called");
        OnBeatChange?.Invoke(beat);
    }

    public void PlayerDied()
    {
        OnPlayerDied?.Invoke();
    }

    public void EnemyDied()
    {
        OnEnemyDied?.Invoke();
    }

    public void RoundStart()
    {
        OnRoundStart?.Invoke();
    }

    public void RoundEnd()
    {
        OnRoundEnd?.Invoke();
    }

    public void RoundChange(int round)
    {
        OnRoundChange?.Invoke(round);
    }
}