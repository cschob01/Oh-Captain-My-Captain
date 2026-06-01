using UnityEngine;

// Expects to be called in Bootstrap
public class DoNotDestroyOnLoad : MonoBehaviour
{
    [SerializeField] private bool ImmediatelyLoadNewScene = false;
    [SerializeField] private string SceneName = "";

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (ImmediatelyLoadNewScene) SceneHandler.Instance.LoadScene(SceneName);
    }
}