using DG.Tweening;
using System.Collections;
using System.IO;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler Instance;

    [System.Serializable]
    public class Transition
    {
        public CanvasGroup fade_cg;
        public float fadeTime = .5f;
    }


    [SerializeField] private Transition LoadingScreen;
    [SerializeField] private Transition SpecialLoadingScreen;

    private bool isLoading;
    private AsyncOperation op;

    public void doneLoading()
    {
        isLoading = false;
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // If we are starting from Bootstrapper and not testing, 
        // start fully black so first scene can fade in
        if (SceneManager.GetActiveScene().name == "Bootstrapper") LoadingScreen.fade_cg.alpha = 1f;
    }

    private string GetNameByIndex(int index)
    {
        string path = SceneUtility.GetScenePathByBuildIndex(index);
        return Path.GetFileNameWithoutExtension(path);
    }

    public void LoadNextLevel()
    {
        if (isLoading) return;

        int index = SceneManager.GetActiveScene().buildIndex + 1;
        if (index >= SceneManager.sceneCountInBuildSettings) LoadScene("Home Page");
        else StartCoroutine(LoadRoutine(GetNameByIndex(index), SpecialLoadingScreen));
    }

    public void LoadScene(string sceneName)
    {
        if (isLoading) return;
        StartCoroutine(LoadRoutine(sceneName, LoadingScreen));
    }

    private IEnumerator LoadRoutine(string sceneName, Transition tran, bool FadeIn = true)
    {
        isLoading = true;

        if (FadeIn)
        {
            // 1. Fade OUT (hide current scene)
            tran.fade_cg.DOFade(1f, tran.fadeTime).SetUpdate(true)
                .OnComplete(() =>
                {
                    tran.fade_cg.interactable = true;
                    tran.fade_cg.blocksRaycasts = true;
                });
            yield return new WaitForSecondsRealtime(tran.fadeTime);
        }

        // 2. Lock input here if you want
        InputHandler.Instance?.EnableControls(false);
        Time.timeScale = 0f;

        // 3. Load scene in background (hidden)
        Debug.Log("Loading " + sceneName);
        op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        // 4. Wait until scene is fully loaded (0.9 barrier)
        while (op.progress < 0.9f)
        {
            yield return null;
        }

        // 5. Activate scene (still invisible due to fade)
        op.allowSceneActivation = true;

        while (!op.isDone)
            yield return null;

        // 6. Custom INIT STEP (THIS is your hook point)
        yield return StartCoroutine(SceneInit());

        // 7. Fade IN (reveal scene)
        tran.fade_cg.DOFade(0f, tran.fadeTime).SetUpdate(true);
        tran.fade_cg.interactable = false;
        tran.fade_cg.blocksRaycasts = false;

        //yield return new WaitForSecondsRealtime(fadeDuration);

        // 8. Enable input AFTER everything is ready
        InputHandler.Instance?.EnableControls(true);
        Time.timeScale = 1f;
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