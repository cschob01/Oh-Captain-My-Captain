using System.Collections.Generic;
using UnityEngine;

public class SettingsHandler : MonoBehaviour
{
    public static SettingsHandler Instance;

    [SerializeField] private CanvasGroup Controls;
    [SerializeField] private CanvasGroup Audio;

    private Dictionary<string, CanvasGroup> panels;
    public bool open;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        panels = new Dictionary<string, CanvasGroup>()
        {
            { "Controls", Controls },
            { "Audio", Audio }
        };

        HidePages();
    }

    public void HidePages()
    {
        ShowPage(Controls, false);
        ShowPage(Audio, false);
        open = false;
    }

    private void ShowPage(CanvasGroup cg, bool show)
    {
        cg.alpha = show? 1f : 0f;
        cg.interactable = show;
        cg.blocksRaycasts = show;
    }

    public void OpenPage(string page)
    {

        if (!panels.TryGetValue(page, out CanvasGroup panel))
        {
            Debug.LogError($"Page '{page}' not found.");
            return;
        }

        HidePages();

        ShowPage(panel, true);
        open = true;
    }

    private void Update()
    {
        if (InputHandler.Instance.PauseWasPressedThisFrame())
        {
            if (open == true)
            {
                HidePages();
            }
        }
    }
}
