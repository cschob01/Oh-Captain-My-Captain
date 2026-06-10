using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup cg;
    private bool Paused;

    private void OnEnable()
    {
        EventHandler.Instance.OnLevelCompleted += DisablePauseMenu;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnLevelCompleted -= DisablePauseMenu;
    }

    private void DisablePauseMenu()
    {
        Pause(false);
        gameObject.SetActive(false);
    }

    private void Start()
    {
        Pause(false);
    }

    private void Update()
    {
        if (InputHandler.Instance.PauseWasPressedThisFrame() && !SettingsHandler.Instance.open)
        {
            ChangePauseState();
        }
    }

    public void ChangePauseState()
    {
        Pause(!Paused);
    }

    public void Pause(bool pause)
    {
        if (pause != Paused)
        {
            EventHandler.Instance.GamePause(pause);
            Paused = pause;
        }

        if (!pause) SettingsHandler.Instance.HidePages();
        Time.timeScale = pause ? 0f : 1f;
        cg.alpha = pause ? 1f : 0f;
        cg.interactable = pause;
        cg.blocksRaycasts = pause;
    }
}
