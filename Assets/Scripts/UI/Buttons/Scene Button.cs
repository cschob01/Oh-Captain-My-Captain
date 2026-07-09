using UnityEngine;

public class SceneButton : MonoBehaviour
{
    public void LoadScene(string SceneName)
    {
        SceneHandler.Instance.LoadScene(SceneName);
    }

    public void LoadHomePage()
    {
        SceneHandler.Instance.LoadScene("Home Page");
    }

    public void RestartScene()
    {
        SceneHandler.Instance.RestartLevel();
    }

    public void ExitGame()
    {
        SceneHandler.Instance.ExitGame();
    }
}