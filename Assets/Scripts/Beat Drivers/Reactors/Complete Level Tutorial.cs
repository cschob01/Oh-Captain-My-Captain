using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Runtime.CompilerServices;

public class CompleteLevelTutorial : MonoBehaviour
{
    private void OnEnable()
    {
        EventHandler.Instance.OnLevelCompleted += Complete;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnLevelCompleted -= Complete;
    }

    private void Complete()
    {
        SceneHandler.Instance.LoadNextLevel();
    }
}
