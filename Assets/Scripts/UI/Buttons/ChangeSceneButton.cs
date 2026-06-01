using UnityEngine;

public class ChangeSceneButton : MonoBehaviour
{
    public void LoadScene(string SceneName)
    {
        SceneHandler.Instance.LoadScene(SceneName);
    }
}