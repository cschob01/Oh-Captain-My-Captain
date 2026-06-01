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

    public void InvokeBeatChange(int beat)
    {
        EventHandler.Instance.BeatChange(beat);
    }

    public void InvokePlayerHealthChange(int health)
    {
        EventHandler.Instance.PlayerHealthChange(health);
    }

    public void InvokeRoundChange(int round)
    {
        EventHandler.Instance.RoundChange(round);
    }

    public void InvokeGunChange(GameObject gun)
    {
        EventHandler.Instance.GunChange(gun);
    }

    public void InvokeGadgetChange(GameObject gadget)
    {
        EventHandler.Instance.GadgetChange(gadget);
    }
}
