using System.Collections;
using UnityEngine;

public abstract class Gadget : MonoBehaviour
{
    [Header("Use Settings")]

    [SerializeField] public float Cooldown = 1f;
    [HideInInspector] public float CooldownProg = 0f;

    [SerializeField] public float UseTime = 0f;
    [HideInInspector] public float UseProg = 0f;

    public bool MidCooldown = false;
    public bool MidUse = false;

    private Coroutine useRoutine = null;

    public void Activate()
    {
        if (MidCooldown || MidUse)
            return;

        Use();

        if (UseTime > 0f)
            useRoutine = StartCoroutine(UseRoutine());
        else
            StartCoroutine(CooldownRoutine());
    }

    protected abstract void Use();

    public void Deactivate()
    {
        if (!MidUse)
            return;

        MidUse = false;

        EndUseRoutine();
        Disuse();

        StartCoroutine(CooldownRoutine());
    }

    protected abstract void Disuse();

    protected void EndUseRoutine()
    {
        if (useRoutine != null)
        {
            StopCoroutine(useRoutine);
            useRoutine = null;
        }
    }

    protected IEnumerator CooldownRoutine()
    {
        MidCooldown = true;
        CooldownProg = 0f;

        while (CooldownProg < Cooldown)
        {
            CooldownProg += Time.deltaTime;
            yield return null;
        }

        CooldownProg = 0f;
        MidCooldown = false;
    }

    protected IEnumerator UseRoutine()
    {
        MidUse = true;
        UseProg = 0f;

        while (UseProg < UseTime)
        {
            UseProg += Time.deltaTime;
            yield return null;
        }

        UseProg = 0f;
        MidUse = false;
        useRoutine = null;

        Disuse();

        StartCoroutine(CooldownRoutine());
    }
}