using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler Instance;

    [Header("References")]
    [SerializeField] private CanvasGroup fadeCanvas; // full-screen black image
    [SerializeField] private Image loadingBar;
    [SerializeField] private float fadeDuration = 0.5f;

    private bool isLoading;
    private AsyncOperation op;

    public void doneLoading()
    {
        isLoading = false;
    }

    public void FixedUpdate()
    {
        if (isLoading && op != null) {
            if (op.progress == .9f)
            {
                loadingBar.fillAmount = 1f;

            }
            else
            {
                loadingBar.fillAmount = op.progress;
            }
        }
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // start fully black so first scene can fade in
        if (fadeCanvas != null)
            fadeCanvas.alpha = 1f;
    }

    public void LoadScene(string sceneName)
    {
        if (isLoading) return;
        StartCoroutine(LoadRoutine(sceneName));
    }

    private IEnumerator LoadRoutine(string sceneName)
    {
        isLoading = true;

        // 1. Fade OUT (hide current scene)
        fadeCanvas.DOFade(1f, fadeDuration)
            .OnComplete(() =>
            {
                fadeCanvas.interactable = true;
                fadeCanvas.blocksRaycasts = true;
            });
        yield return new WaitForSeconds(fadeDuration);

        // 2. Lock input here if you want
        InputHandler.Instance?.EnableControls(false);

        // 3. Load scene in background (hidden)
        Debug.Log("Loading " + sceneName);
        op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        // 4. Wait until scene is fully loaded (0.9 barrier)
        while (op.progress < 0.9f)
            yield return null;

        // 5. Activate scene (still invisible due to fade)
        op.allowSceneActivation = true;

        while (!op.isDone)
            yield return null;

        // 6. Custom INIT STEP (THIS is your hook point)
        yield return StartCoroutine(SceneInit());

        // 7. Fade IN (reveal scene)
        fadeCanvas.DOFade(0f, fadeDuration)
            .OnComplete(() =>
                {
                    fadeCanvas.interactable = false;
                    fadeCanvas.blocksRaycasts = false;
                });
        yield return new WaitForSeconds(fadeDuration);

        // 8. Enable input AFTER everything is ready
        InputHandler.Instance?.EnableControls(true);
    }

    private IEnumerator SceneInit()
    {
        // This is your “everything is ready but not visible” stage

        Debug.Log("Scene Init Start");

        while (isLoading)
        {
            yield return null; // allow one frame for safety
        }

        Debug.Log("Scene Init Complete");
    }
}