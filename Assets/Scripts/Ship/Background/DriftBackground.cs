using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class DriftBackground : MonoBehaviour
{
    [Header("Scroll Speed (UVs per second)")]
    [SerializeField] private Vector2 scrollSpeed = new Vector2(0.02f, -0.005f);

    private RawImage rawImage;
    private Rect uvRect;

    private void Awake()
    {
        rawImage = GetComponent<RawImage>();
        uvRect = rawImage.uvRect;
    }

    private void Update()
    {
        uvRect.x = Mathf.Repeat(uvRect.x + scrollSpeed.x * Time.deltaTime, 1f);
        uvRect.y = Mathf.Repeat(uvRect.y + scrollSpeed.y * Time.deltaTime, 1f);

        rawImage.uvRect = uvRect;
    }
}