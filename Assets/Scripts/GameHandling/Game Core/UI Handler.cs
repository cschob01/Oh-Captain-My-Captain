using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public static UIHandler Instance;

    [HideInInspector]
    public Canvas canvas;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        canvas = GetComponent<Canvas>();
    }
}
