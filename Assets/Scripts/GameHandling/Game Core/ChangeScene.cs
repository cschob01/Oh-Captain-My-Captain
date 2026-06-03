using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private string scene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SceneHandler.Instance.LoadScene(scene);
    }
}
