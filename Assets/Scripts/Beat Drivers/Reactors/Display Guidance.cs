using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections;


public class DisplayGuidance : MonoBehaviour
{
    private CanvasGroup cg;
    private TextMeshProUGUI Text;
    private Coroutine coroutine;

    [System.Serializable]
    public class Guidance
    {
        public string name;
        public string text;
        public float cooldown;
    }

    [SerializeField] private Guidance[] Guides;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        Text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }
    private void OnDestroy()
    {
        DOTween.Kill(cg);
    }
    private void OnEnable()
    {
        EventHandler.Instance.OnBeatChange += SetGuidance;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnBeatChange -= SetGuidance;
    }

    private void SetGuidance(string index)
    {
        for (int i = 0; i < Guides.Length; i++)
        {
            if (Guides[i].name == index)
            {
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                    coroutine = null;
                }

                Text.text = Guides[i].text;
                cg.DOKill();
                cg.DOFade(1f, 0.25f);
                coroutine = StartCoroutine(Cooldown(Guides[i].cooldown));
            }
        }
    }

    private IEnumerator Cooldown(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        cg.DOFade(0f, 1.25f);
    }

}
