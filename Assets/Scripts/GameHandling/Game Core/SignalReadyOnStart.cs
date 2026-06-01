using UnityEngine;

public class SignalReadyOnStart : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SceneHandler.Instance.doneLoading();
    }
}
