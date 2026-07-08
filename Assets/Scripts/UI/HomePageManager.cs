using System.Collections;
using UnityEngine;

public class HomePageManager : MonoBehaviour
{
    [SerializeField] private Animator TitleScreen;
    [SerializeField] private Animator Tutorials;
    [SerializeField] private Animator Maps;
    [SerializeField] private float animation_wait_time;

    private Animator Active;
    private bool switching;

    private void Awake()
    {
        ShowTitleScreen();
    }

    public void ShowTitleScreen()
    {
        Debug.Log("Moving to Title Screen");
        if (!switching) StartCoroutine(ShowPage(TitleScreen));
    }

    public void ShowTutorials()
    {
        Debug.Log("Moving to Tutorials");
        if (!switching) StartCoroutine(ShowPage(Tutorials));
    }

    public void ShowMaps()
    {
        Debug.Log("Moving to Maps");
        if (!switching) StartCoroutine(ShowPage(Maps));
    }

    private IEnumerator ShowPage(Animator anim)
    {
        if (Active == anim) yield break;


        switching = true;
        if (Active != null) Active.SetTrigger("Exit");
        yield return new WaitForSeconds(animation_wait_time);
        if (anim != null) anim.SetTrigger("Enter");
        Active = anim;
        yield return new WaitForSeconds(animation_wait_time);
        switching = false;
    }

}
