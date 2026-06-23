using UnityEngine;

public class CallEventButton : MonoBehaviour
{
    public void InvokePlayerDied()
    {
        EventHandler.Instance.PlayerDied();
    }

    public void InvokeEnemyDied()
    {
        EventHandler.Instance.EnemyDied();
    }

    public void InvokeRoundStart()
    {
        EventHandler.Instance.RoundStart();
    }

    public void InvokeRoundEnd()
    {
        EventHandler.Instance.RoundEnd();
    }

    public void InvokeBeatChange(string beat)
    {
        EventHandler.Instance.BeatChange(beat);
    }

    public void InvokeHealthChange(PlayerHealth Health)
    {
        EventHandler.Instance.HealthChange(Health);
    }

    public void InvokeRoundChange(int round)
    {
        EventHandler.Instance.RoundChange(round);
    }
}
