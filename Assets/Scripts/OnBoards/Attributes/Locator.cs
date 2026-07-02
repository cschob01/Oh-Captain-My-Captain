using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Locator : MonoBehaviour
{
    [SerializeField] private RectTransform imageWorldLabel;
    [SerializeField] private bool DisplayDistance;
    [SerializeField] private bool OnScreenEdge;
    [SerializeField] private float Margin = .45f;

    private RectTransform indicator;
    private TextMeshProUGUI Text = null;
    private RectTransform Image;

    private void Awake()
    {
        indicator = Instantiate(
            imageWorldLabel,
            UIHandler.Instance.canvas.transform
        ).GetComponent<RectTransform>();

        ShowIndicator(false);

        Image = indicator.GetComponentInChildren<Image>().GetComponent<RectTransform>();
        Text = indicator.GetComponentInChildren<TextMeshProUGUI>();

        if (!DisplayDistance && Text != null)
        {
            Text.text = "";
            Text = null;
        }
    }

    private void OnDestroy()
    {
        if (indicator != null) Destroy(indicator.gameObject);
    }

    private void OnEnable()
    {
        if (indicator != null) indicator.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        if (indicator != null) indicator.gameObject.SetActive(false);
    }

    private void ShowIndicator(bool show)
    {
        if (indicator != null) indicator.gameObject.SetActive(show);
    }

    void Update()
    {
        Vector3 vp = Camera.main.WorldToViewportPoint(transform.position);

        bool offscreen =
            vp.x < 0 || vp.x > 1 ||
            vp.y < 0 || vp.y > 1 ||
            vp.z < 0;

        if (!offscreen)
        {
            ShowIndicator(false);
            return;
        }

        ShowIndicator(true);

        Vector2 center = new Vector2(0.5f, 0.5f);

        //Get normalization
        Vector2 dir = (Vector2)vp - center;
        Vector2 clamped = Vector2.zero;

        if (OnScreenEdge)
        {
            float chebyshev = Mathf.Max(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
            if (chebyshev > 0f)
                dir /= chebyshev;

            clamped = center + dir * Margin;
        }
        else
        {
            dir = ((Vector2)vp - center).normalized;
            clamped = center + dir * Margin;
        }

            Vector2 screenPos = new Vector2(
                clamped.x * Screen.width,
                clamped.y * Screen.height
            );

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (indicator != null)
            indicator.position = screenPos;

        if (Image != null)
            Image.rotation = Quaternion.Euler(0f, 0f, angle);

        if (Text != null)
        {
            float dist = Vector2.Distance(
                transform.position,
                Camera.main.transform.position
            );

            Text.text = dist.ToString("F0") + "m";
        }
    }
}