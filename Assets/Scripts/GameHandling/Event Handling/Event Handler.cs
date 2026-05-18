using System;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    public static EventHandler Instance;

    public event Action OnPlayerDied;
    public event Action OnEnemyDied;
    public event Action<int> OnPlayerHealthChange;
    public event Action<int> OnAmmoChange;
    public event Action OnRoundStart;
    public event Action OnRoundEnd;
    public event Action<int> OnRoundChange;

    public event Action<GameObject> OnGunChange;

    private void Awake()
    {
        Instance = this;
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

    public void AmmoChange(int ammo)
    {
        OnAmmoChange?.Invoke(ammo);
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

    public void RoundChange(int round)
    {
        OnRoundChange?.Invoke(round);
    }
}