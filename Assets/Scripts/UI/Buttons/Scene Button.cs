using UnityEngine;

public class SceneButton : MonoBehaviour
{
    public void LoadScene(string SceneName)
    {
        SceneHandler.Instance.LoadScene(SceneName);
    }
}