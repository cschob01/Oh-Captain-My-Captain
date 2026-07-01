using System;
using System.Collections.Generic;
using UnityEngine;

public class CaptainHandler : MonoBehaviour
{
    public static CaptainHandler Instance;

    [Header("Starting Setup")]

    [SerializeField] private Gun StartingGun;
    [SerializeField] private Gadget StartingGadget;
    [SerializeField] private int StartingMoney;

    [Header("Mid-Game")]

    [SerializeField] public Health Health { get; private set; }
    [SerializeField, Range(1, 3)] private int MaxGadgets;
    [HideInInspector] public List<Gadget> Gadgets { get; private set; } = new();
    [HideInInspector] public int CurrGadget { get; private set; }

    [SerializeField, Range(1, 3)] private int MaxGuns;
    [HideInInspector] public List<Gun> Guns { get; private set; } = new();
    [HideInInspector] public int CurrGun { get; private set; }
    [HideInInspector] public int Money { get; private set; }


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        ResetPlayer();

    }

    void Update()
    {
        if (GunEquipped())
        {
            // Fire/Reload gun if requested
            if (InputHandler.Instance.FireIsPressed())
            {
                Debug.Log("Requesting gunfire");
                Guns[CurrGun].Fire();
            }
            if (InputHandler.Instance.ReloadWasPressedThisFrame()) Guns[CurrGun].Reload();
        }
           
        if (GadgetEquipped())
        {
            // Use gadget if requested
            if (InputHandler.Instance.GadgetWasPressedThisFrame()) Gadgets[CurrGadget].Activate(); // Try to activate gadget
            if (InputHandler.Instance.GadgetWasReleasedThisFrame()) Gadgets[CurrGadget].Deactivate(); // Try to activate gadget
        }

        SwitchGun(InputHandler.Instance.GunSwitchWasPressedThisFrame());
        SwitchGadget(InputHandler.Instance.GadgetSwitchWasPressedThisFrame());

        AimGun();
    }

    private void SwitchGadget(bool move)
    {
        CurrGadget += move ? 1 : 0;
        UpdateGadgets();
    }

    private void SwitchGun(int index)
    {
        if (index > 1)
        {
            int newIndex = index - 2;
            if (newIndex <= Guns.Count - 1) CurrGun = newIndex;
        }
        else
        {
            CurrGun += index;
        }
        UpdateGuns();
    }
    private void AimGun()
    {
        if (GunEquipped())
        {
            Vector2 dir = InputHandler.Instance.LookReadValue();
            Guns[CurrGun].gameObject.transform.localPosition = dir * .1f;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Guns[CurrGun].transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void MakeMoney(int money)
    {
        Money += money;
    }

    public void SpendMoney(int money)
    {
        Money -= money;
    }

    public void ResetPlayer()
    {
        for(int i = 0; i < MaxGuns; i++)
        {
            RemoveGun();
        }
        AddGun(StartingGun);
        for (int i = 0; i < MaxGadgets; i++)
        {
            RemoveGadget();
        }
        AddGadget(StartingGadget);

        Money = StartingMoney;
    }

    private bool GunEquipped()
    {
        if (Guns.Count == 0) return false;
        else if (Guns[CurrGun] == null) Debug.Log("ERROR: Null is loaded into Gun array, inex " + CurrGun);
        return true;
    }

    private void UpdateGuns()
    {
        if (Guns.Count <= CurrGun) CurrGun = 0;
        if (0 > CurrGun) CurrGun = Guns.Count - 1;

        for (int i = 0; i < Guns.Count; i++)
        {
            if (i != CurrGun) Guns[i].gameObject.SetActive(false);
            else Guns[CurrGun].gameObject.SetActive(true);
        }
    }

    public void FlipGun()
    {
        CurrGun++;
        UpdateGuns();
    }
    public void RemoveGun()
    {
        if (Guns.Count == 0) return;

        if (Guns[CurrGun] != null) Destroy(Guns[CurrGun].gameObject);
        Guns.RemoveAt(CurrGun);
        UpdateGuns();
    }

    public void AddGun(Gun gun)
    {
        if (gun == null) return;

        if (Guns.Count == MaxGuns)
        {
            if (Guns[CurrGun] != null) Destroy(Guns[CurrGun].gameObject);
            Guns.RemoveAt(CurrGun);
        }
        Guns.Insert(CurrGun, Instantiate(gun, transform.position, Quaternion.identity, transform));

        UpdateGuns();
    }

    private bool GadgetEquipped()
    {
        if (Gadgets.Count == 0) return false;
        else if (Gadgets[CurrGadget] == null) Debug.Log("ERROR: Null is loaded into Gadget array, inex " + CurrGadget);
        return true;
    }

    private void UpdateGadgets()
    {
        if (Gadgets.Count <= CurrGadget) CurrGadget = 0;
        if (0 > CurrGadget) CurrGadget =  Gadgets.Count - 1;
    }

    public void FlipGadget()
    {
        CurrGadget++;
        UpdateGadgets();
    }
    public void RemoveGadget()
    {
        if (Gadgets.Count == 0) return;

        if (Gadgets[CurrGadget] != null) Destroy(Gadgets[CurrGadget].gameObject);
        Gadgets.RemoveAt(CurrGadget);
        UpdateGadgets();
    }

    public void AddGadget(Gadget gadget)
    {
        if (gadget == null) return;

        if (Gadgets.Count == MaxGadgets)
        {
            if (Gadgets[CurrGadget] != null) Destroy(Gadgets[CurrGadget].gameObject);
            Gadgets.RemoveAt(CurrGadget);
        }
        Gadgets.Insert(CurrGadget, Instantiate(gadget, transform.position, Quaternion.identity, transform));

        UpdateGadgets();
    }
}
