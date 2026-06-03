using UnityEngine;
using UnityEngine.UI;

public class ChangeOnPause : MonoBehaviour
{
    private Image Image;

    [Tooltip("Unpaused")]
    [SerializeField] private Sprite sprite1;
    [Tooltip("Paused")]
    [SerializeField] private Sprite sprite2;

    private void Awake()
    {
        Image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        EventHandler.Instance.OnGamePause += ChangeSprite;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnGamePause -= ChangeSprite;
    }

    private void ChangeSprite(bool paused)
    {
        if (paused) Image.sprite = sprite2;
        else Image.sprite = sprite1;
    }

}
