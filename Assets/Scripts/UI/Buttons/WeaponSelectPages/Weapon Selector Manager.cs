using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponSelectorManager : MonoBehaviour
{
    private CanvasGroup cg;
    [SerializeField] private Gun[] GunPrefabs; // Expects >= Number of selectors
    [SerializeField] private WeaponSelector[] Selectors;

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
    }
    private void OnEnable()
    {
        EventHandler.Instance.OnRoundStart += ClosePage;
        EventHandler.Instance.OnRoundEnd += OpenPage;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnRoundStart -= ClosePage;
        EventHandler.Instance.OnRoundEnd -= OpenPage;
    }
    private void ClosePage()
    {
        ShowPage(false);
    }
    private void OpenPage()
    {
        Debug.Log("New weapons provided");

        Gun[] Options = GunPrefabs
            .OrderBy(x => UnityEngine.Random.value)
            .Take(Selectors.Length)
            .ToArray();

        for (int i = 0; i < Selectors.Length; i++)
        {
            Selectors[i].SetButton(Options[i]);
        }


        ShowPage(true);
    }

    private void ShowPage(bool show)
    {
        cg.alpha = show? 1 : 0;
        cg.interactable = show;
        cg.blocksRaycasts = show;
    }
}
