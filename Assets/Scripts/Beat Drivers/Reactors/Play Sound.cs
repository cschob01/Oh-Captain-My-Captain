using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField] private string beat;
    private AudioSource AudioSource;

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        EventHandler.Instance.OnBeatChange += OnBeatChange;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnBeatChange -= OnBeatChange;
    }

    private void OnBeatChange(string beat)
    {
        if (beat == this.beat) AudioSource.Play();
    }
}
