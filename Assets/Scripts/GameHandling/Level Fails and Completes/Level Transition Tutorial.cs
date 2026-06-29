using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Runtime.CompilerServices;

public class LevelTransitionTutorial : MonoBehaviour
{
    private void OnEnable()
    {
        EventHandler.Instance.OnLevelCompleted += Complete;
        EventHandler.Instance.OnLevelFailed += Fail;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnLevelCompleted -= Complete;
        EventHandler.Instance.OnLevelFailed -= Fail;
    }

    private void Complete()
    {
        SceneHandler.Instance.LoadNextLevel();
    }

    private void Fail()
    {
        StartCoroutine(FailRoutine());
    }

    private IEnumerator FailRoutine()
    {
        yield return new WaitForSecondsRealtime(0f);
        SceneHandler.Instance.RestartLevel();
    }
}
