using UnityEngine;

public class WatchPerformance : MonoBehaviour
{

    public int fps_warning_marker = 50;
    void Update()
    {
        //Check that fps is above marker
        float fps = 1f / Time.deltaTime;
        if (fps < fps_warning_marker)
        {
            Debug.Log("FPS IS BELOW MARKER - FPS: "  + fps);
        }
    }
}
