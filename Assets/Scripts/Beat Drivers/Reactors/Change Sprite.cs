using UnityEngine;

public class ChangeSprite : MonoBehaviour
{
    [SerializeField] string beat;
    [SerializeField] Sprite sprite;
    [SerializeField] bool OneTimeUse;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) Debug.Log("ERROR: Did not find sprite renderer component for ChangeSprite");
    }

    private void OnEnable()
    {
        EventHandler.Instance.OnBeatChange += changeSprite;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnBeatChange -= changeSprite;
    }

    private void changeSprite(string beat)
    {
        if (this.beat == beat)
        {
            spriteRenderer.sprite = sprite;
            if (OneTimeUse) Destroy(this);
        }
    }
}
