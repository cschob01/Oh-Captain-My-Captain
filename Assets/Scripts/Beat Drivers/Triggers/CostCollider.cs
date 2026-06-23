using System.Reflection.Emit;
using TMPro;
using UnityEngine;

public class CostCollider : MonoBehaviour
{
    [Tooltip("Prompt follows \"binding: \"")]
    [SerializeField] private string prompt;
    [SerializeField] private int cost;
    [SerializeField] private string[] Beats;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Vector3 offset = Vector3.zero;
    [SerializeField] private bool OneTimeUse;

    private bool Active;
    private TextMeshProUGUI label;
    private string Prompt;

    private void Awake()
    {
        label = Instantiate(
            text,
            UIHandler.Instance.canvas.transform
        ).GetComponent<TextMeshProUGUI>();

        Prompt = InputHandler.Instance.GetInteractBinding() + ": " + prompt;
        label.text = Prompt;
        DisplayPrompt(false);
    }

    private void OnDestroy()
    {
        if (label != null) Destroy(label.gameObject);
    }

    private void OnEnable()
    {
        if (label != null) label.enabled = true;
    }

    private void OnDisable()
    {
        if (label != null) label.enabled = false;
    }

    private void LateUpdate()
    {
        if (label == null) return;
        if (!Active) return;

        label.rectTransform.position = Camera.main.WorldToScreenPoint(
            transform.position + offset
        );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DisplayPrompt(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        DisplayPrompt(false);
    }

    private void DisplayPrompt(bool show)
    {
        Active = show;
        LateUpdate();
        if (label != null) label.text = show? Prompt : "";
    }

    private void Update()
    {
        if (Active)
        {
            if (InputHandler.Instance.InteractWasPressedThisFrame() && CaptainHandler.Instance.Money >= cost)
            {
                CaptainHandler.Instance.SpendMoney(cost);
                for (int i = 0; i < Beats.Length; i++)
                {
                    EventHandler.Instance.BeatChange(Beats[i]);
                }
                if (OneTimeUse) Destroy(this);
            }
        }
    }
}
