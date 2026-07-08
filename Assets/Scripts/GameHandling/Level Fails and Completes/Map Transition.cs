using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.UI;

public class MapTransition : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private Sprite CompletePage;
    [SerializeField] private Sprite FailPage;

    [Header("UI Bindings")]
    [SerializeField] private CanvasGroup Holder;
    [SerializeField] private Image Page;
    [SerializeField] private TextMeshProUGUI EndCondition;
    [SerializeField] private TextMeshProUGUI TimeSurvived;
    [SerializeField] private TextMeshProUGUI Kills;
    [SerializeField] private TextMeshProUGUI Points;
    [SerializeField] private TextMeshProUGUI FavoriteGun;
    [SerializeField] private TextMeshProUGUI FavoriteGadget;

    private bool Tracking = true;
    //private bool Displaying = false;

    //Tracking
    private float time;
    private int PrevMoney;
    private int MoneyCounter;
    private Dictionary<string, float> WeaponList = new Dictionary<string, float>();
    private Dictionary<string, float> GadgetList = new Dictionary<string, float>();
    private int KillCount;

    //Displaying
    private int DisplayPoints;
    private int DisplayKills;
    private float DisplayTimeSurvived;
    private string DisplayFavoriteGadget;
    private string DisplayFavoriteGun;

    private void Awake()
    {
        Holder.alpha = 0f;
        Holder.blocksRaycasts = false;
        Holder.interactable = false;
    }

    private void OnLevelComplete(int index)
    {
        Time.timeScale = 0f;
        Page.sprite = CompletePage;
        DisplayPage(index);
    }

    private void OnLevelFailed()
    {
        Time.timeScale = 0f;
        Page.sprite = FailPage;
        DisplayPage(-1);
    }

    private IEnumerator UpdateDisplayStats()
    {
        float TweenTime = 2f;

        DOTween.To(() => DisplayTimeSurvived, x => DisplayTimeSurvived = x, time, TweenTime).SetUpdate(true).SetTarget(this);
        yield return new WaitForSecondsRealtime(TweenTime);

        DOTween.To(() => DisplayKills, x => DisplayKills = x, KillCount, TweenTime).SetUpdate(true).SetTarget(this);
        yield return new WaitForSecondsRealtime(TweenTime);

        DOTween.To(() => DisplayPoints, x => DisplayPoints = x, MoneyCounter, TweenTime).SetUpdate(true).SetTarget(this);
        yield return new WaitForSecondsRealtime(TweenTime);

        float stringTime = 1f;

        yield return new WaitForSecondsRealtime(stringTime);
        DisplayFavoriteGadget = GetMaxString(GadgetList).Replace("(Clone)", "");

        yield return new WaitForSecondsRealtime(stringTime);
        DisplayFavoriteGun = GetMaxString(WeaponList).Replace("(Clone)", "");
    }

    private IEnumerator UpdateDisplayRoutine()
    {
        while(true){
            yield return null;
            UpdateDisplay();
        }
    }

    private string GetMaxString(Dictionary<string, float> dict)
    {
        float maxValue = float.MinValue;
        string keyValue = "None";

        foreach (var pair in dict)
        {
            if (pair.Value > maxValue)
            {
                maxValue = pair.Value;
                keyValue = pair.Key;
            }
        }

        return keyValue;
    }

    private void DisplayPage(int EndCondition) // -1 = fail, 0 = escape pods, 1 = ship control
    {
        //Displaying = true;

        Holder.DOFade(1f, 2f).SetUpdate(true);
        Holder.blocksRaycasts = true; 
        Holder.interactable = true;

        if (EndCondition == -1) this.EndCondition.text = "You Died";
        else if (EndCondition == 0) this.EndCondition.text = "You Escaped";
        else if (EndCondition == 1) this.EndCondition.text = "You Made it Home";

        StartCoroutine(UpdateDisplayStats());
        StartCoroutine(UpdateDisplayRoutine());
    }

    private void OnDestroy()
    {
        DOTween.Kill(this);
    }

    private void Update()
    {
        if (Tracking) Track();

    }

    private void UpdateDisplay()
    {
        TimeSurvived.text = "Time Survived: " + DisplayTimeSurvived.ToString("F2");
        Kills.text = "Kills: " + DisplayKills;
        Points.text = "Score: " + DisplayPoints;
        FavoriteGadget.text = "Favorite Gadget: " + DisplayFavoriteGadget;
        FavoriteGun.text = "Favorite Gun: " + DisplayFavoriteGun;
    }

    private void Track()
    {
        //Tracking Favorite Gun
        if (CaptainHandler.Instance.Guns.Count != 0)
        {
            string CurrGunName = CaptainHandler.Instance.Guns[CaptainHandler.Instance.CurrGun].name;
            WeaponList.TryAdd(CurrGunName, 0);
            WeaponList[CurrGunName] += Time.deltaTime;
        }

        //Tracking Favorite Gadget
        if (CaptainHandler.Instance.Gadgets.Count != 0)
        {
            string CurrGadgetName = CaptainHandler.Instance.Gadgets[CaptainHandler.Instance.CurrGadget].name;
            GadgetList.TryAdd(CurrGadgetName, 0);
            GadgetList[CurrGadgetName] += Time.deltaTime;
        }

        //Update Points
        int CaptainMoney = CaptainHandler.Instance.Money;
        if (PrevMoney < CaptainMoney)
        {
            MoneyCounter += CaptainMoney - PrevMoney;
        }
        PrevMoney = CaptainMoney;

        //Track TIme
        if (Timer.Instance != null) time = Timer.Instance.TimeProg;
        else time = 0;
    }

    private void OnEnable()
    {
        EventHandler.Instance.OnPlayerDied += EndTracking;
        EventHandler.Instance.OnEnemyDied += OnEnemyDeath;
        EventHandler.Instance.OnLevelCompleted += OnLevelComplete;
        EventHandler.Instance.OnLevelFailed += OnLevelFailed;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnPlayerDied -= EndTracking;
        EventHandler.Instance.OnEnemyDied -= OnEnemyDeath;
        EventHandler.Instance.OnLevelCompleted -= OnLevelComplete;
        EventHandler.Instance.OnLevelFailed -= OnLevelFailed;
    }

    private void EndTracking()
    {
        Tracking = false;
    }

    private void OnEnemyDeath()
    {
        if (!Tracking) return;
        KillCount++;
    }

}
