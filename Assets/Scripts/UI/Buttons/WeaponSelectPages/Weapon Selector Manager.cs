using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponSelectorManager : MonoBehaviour
{
    private CanvasGroup cg;
    [SerializeField] private GameObject[] GunPrefabs; // Expects >= 3
    [SerializeField] private WeaponSelector[] Selectors; // Expects 3

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
        GameObject[] Options = GunPrefabs
            .OrderBy(x => UnityEngine.Random.value)
            .Take(3)
            .ToArray();

        for (int i = 0; i < 3; i++)
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
