using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    [SerializeField] private string Beat;

    private void OnEnable()
    {
        EventHandler.Instance.OnBeatChange += DestroySelf;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnBeatChange -= DestroySelf;
    }

    private void DestroySelf(string i)
    {
        if (i == Beat) Destroy(gameObject);
    }
}
