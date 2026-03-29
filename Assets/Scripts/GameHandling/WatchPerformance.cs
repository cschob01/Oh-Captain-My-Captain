using UnityEngine;

public class WatchPerformance : MonoBehaviour
{

    public int fps_warning_marker = 50;
    void Update()
    {
        float fps = 1f / Time.deltaTime;
        if (fps < fps_warning_marker)
        {
            Debug.Log("FPS IS BELOW MARKER - FPS: "  + fps);
        }
    }
}
