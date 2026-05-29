using System.Collections;
using UnityEngine;

public abstract class Gadget : MonoBehaviour
{
    [Header("Use Settings")]

    [SerializeField] public float Cooldown = 1f;
    [HideInInspector] public float CooldownProg = 0f;
    protected bool MidCooldown = false;

    public void Activate()
    {
        if (!MidCooldown) Use();
    }

    protected abstract void Use();

    public abstract void Deactivate();

    protected IEnumerator CooldownRoutine()
    {
        MidCooldown = true;
        while (CooldownProg < Cooldown)
        {
            yield return null;
            CooldownProg += Time.deltaTime;
        }
        CooldownProg = 0f;
        MidCooldown = false;
    }

}
