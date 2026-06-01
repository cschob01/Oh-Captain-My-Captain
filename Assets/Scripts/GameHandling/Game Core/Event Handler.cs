using System;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    public static EventHandler Instance;

    public event Action OnPlayerDied;
    public event Action OnEnemyDied;
    public event Action<int> OnPlayerHealthChange;
    public event Action OnRoundStart;
    public event Action OnRoundEnd;
    public event Action<int> OnRoundChange;

    public event Action<GameObject> OnGunChange;
    public event Action<GameObject> OnGadgetChange;

    /// Beats
    public event Action<int> OnBeatChange;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void BeatChange(int i)
    {
        OnBeatChange?.Invoke(i);
    }

    public void PlayerDied()
    {
        OnPlayerDied?.Invoke();
    }

    public void EnemyDied()
    {
        OnEnemyDied?.Invoke();
    }

    public void PlayerHealthChange(int health)
    {
        OnPlayerHealthChange?.Invoke(health);
    }

    public void RoundStart()
    {
        OnRoundStart?.Invoke();
    }

    public void RoundEnd()
    {
        OnRoundEnd?.Invoke();
    }

    public void GunChange(GameObject Gun)
    {
        OnGunChange?.Invoke(Gun);
    }

    public void GadgetChange(GameObject Gadget)
    {
        OnGadgetChange?.Invoke(Gadget);
    }

    public void RoundChange(int round)
    {
        OnRoundChange?.Invoke(round);
    }
}