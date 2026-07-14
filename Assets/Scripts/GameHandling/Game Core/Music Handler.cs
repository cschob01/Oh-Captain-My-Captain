using DG.Tweening;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicHandler : MonoBehaviour
{
    public static MusicHandler Instance;

    [SerializeField] private AudioClip[] Songs;
    [SerializeField] private float FadeDuration;
    [SerializeField] private float TimeBetweenSongs;

    private AudioSource AudioSource;

    private AudioClip ActiveSong;
    private Coroutine Transition;
    private float TimeProg;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        AudioSource = GetComponent<AudioSource>();

        PlaySong(GetNewRandomSong());
    }

    private void Update()
    {


        if (ClipEnded())
        {
            TimeProg += Time.unscaledDeltaTime;
            if (TimeProg > TimeBetweenSongs)
                PlaySong(GetNewRandomSong());
        }
        else
        {
            TimeProg = 0;
        }
    }

    private AudioClip GetNewRandomSong()
    {
        AudioClip Chosen = ActiveSong;

        int attempts = 0;
        while (Chosen == ActiveSong)
        {
            Chosen = Songs[Random.Range(0, Songs.Length)];
            attempts++;
            if (attempts > 10) return Chosen;
        }
        return Chosen;
    }

    private void PlaySong(AudioClip song)
    {
        EndTranstion();
        ActiveSong = song;
        Transition = StartCoroutine(SongTransition(song));
    }

    private void EndTranstion()
    {
        if (Transition == null) return;
        else StopCoroutine(Transition);
        Transition = null;
    }

    private IEnumerator SongTransition(AudioClip song)
    {
        AudioSource.DOFade(0f, FadeDuration).SetUpdate(true); ;
        yield return new WaitForSecondsRealtime(FadeDuration);
        AudioSource.Stop();
        AudioSource.clip = song;
        AudioSource.Play();
        AudioSource.DOFade(1f, FadeDuration).SetUpdate(true); ;
        yield return new WaitForSecondsRealtime(FadeDuration);

        //Shut itself off
        Transition = null;
    } 

    private bool ClipEnded()
    {
        return AudioSource.clip != null && !AudioSource.isPlaying && Transition == null;
    }

}
