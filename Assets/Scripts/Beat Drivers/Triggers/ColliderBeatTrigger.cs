using UnityEngine;
using UnityEngine.Events;

public class ColliderBeatTrigger : MonoBehaviour
{
    [SerializeField] private bool OneTimeUse = true;
    [SerializeField] private bool CompleteLevel = false;
    [SerializeField] private string Beat;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (CompleteLevel)
        {
            EventHandler.Instance.LevelCompleted();
        }
        else
        {
            EventHandler.Instance.BeatChange(Beat);
        }

        if (OneTimeUse)
        {
            Destroy(gameObject);
        }
    }
}
