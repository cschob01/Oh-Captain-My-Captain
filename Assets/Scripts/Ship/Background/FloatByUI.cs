using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
[RequireComponent(typeof(RectTransform))]
public class FloatByUI : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float minDelay = 15f;
    [SerializeField] private float maxDelay = 45f;

    [Header("Movement")]
    [SerializeField] private float minSpeed = 20f;
    [SerializeField] private float maxSpeed = 40f;

    [Header("Rotation")]
    [SerializeField] private float minRotationSpeed = -5f;
    [SerializeField] private float maxRotationSpeed = 5f;

    [Header("Spawn Padding")]
    [SerializeField] private float offscreenPadding = 200f;

    [SerializeField, Range(0f, 1f)]
    private float centerRegion = 0.5f;

    private RectTransform rect;
    private RectTransform canvasRect;
    private RawImage image;

    private Vector2 velocity;
    private float rotationSpeed;
    private float delayTimer;
    private bool activeFlight;



    private void Awake()
    {
        image = GetComponent<RawImage>();
        rect = GetComponent<RectTransform>();
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        image.enabled = false;
        ScheduleNextFlight();
    }

    private void Update()
    {
        if (!activeFlight)
        {
            delayTimer -= Time.deltaTime;

            if (delayTimer <= 0f)
            {
                image.enabled = true;
                BeginFlight();
            }

            return;
        }

        rect.anchoredPosition += velocity * Time.deltaTime;
        rect.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

        float halfWidth = canvasRect.rect.width * 0.5f;
        float halfHeight = canvasRect.rect.height * 0.5f;

        if (rect.anchoredPosition.x < -halfWidth - offscreenPadding ||
            rect.anchoredPosition.x > halfWidth + offscreenPadding ||
            rect.anchoredPosition.y < -halfHeight - offscreenPadding ||
            rect.anchoredPosition.y > halfHeight + offscreenPadding)
        {
            image.enabled = false;
            activeFlight = false;
            ScheduleNextFlight();
        }
    }

    private void ScheduleNextFlight()
    {
        delayTimer = Random.Range(minDelay, maxDelay);
    }

    private void BeginFlight()
    {
        activeFlight = true;

        float halfWidth = canvasRect.rect.width * 0.5f;
        float halfHeight = canvasRect.rect.height * 0.5f;

        // Spawn just outside a random edge.
        Vector2 start = GetRandomBorderPoint(halfWidth, halfHeight);

        // Pick a destination in the middle 50% of the screen.
        float xRange = halfWidth * centerRegion;
        float yRange = halfHeight * centerRegion;
        Vector2 target = new Vector2(
            Random.Range(-xRange, xRange),
            Random.Range(-yRange, yRange));

        rect.anchoredPosition = start;

        float speed = Random.Range(minSpeed, maxSpeed);
        velocity = (target - start).normalized * speed;

        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);

        rect.localRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
    }

    private Vector2 GetRandomBorderPoint(float halfWidth, float halfHeight)
    {
        switch (Random.Range(0, 4))
        {
            case 0: // Left
                return new Vector2(
                    -halfWidth - offscreenPadding,
                    Random.Range(-halfHeight, halfHeight));

            case 1: // Right
                return new Vector2(
                    halfWidth + offscreenPadding,
                    Random.Range(-halfHeight, halfHeight));

            case 2: // Top
                return new Vector2(
                    Random.Range(-halfWidth, halfWidth),
                    halfHeight + offscreenPadding);

            default: // Bottom
                return new Vector2(
                    Random.Range(-halfWidth, halfWidth),
                    -halfHeight - offscreenPadding);
        }
    }
}