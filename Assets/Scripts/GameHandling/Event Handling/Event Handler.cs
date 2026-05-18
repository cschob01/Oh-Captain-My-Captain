using System;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    public static EventHandler Instance;

    public event Action OnPlayerDied;
    public event Action<int> OnPlayerHealthChange;
    public event Action<int> OnAmmoChange;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayerDied()
    {
        OnPlayerDied?.Invoke();
    }

    public void PlayerHealthChange(int health)
    {
        OnPlayerHealthChange?.Invoke(health);
    }

    public void AmmoChange(int ammo)
    {
        OnAmmoChange?.Invoke(ammo);
    }
}