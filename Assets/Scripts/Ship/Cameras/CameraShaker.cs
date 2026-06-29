using System.Threading;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public bool Shaking;
    [SerializeField] private float ShakeMagnitude = 0.05f;
    [SerializeField] private float HangTime = 1f;

    private float countdown;
    private Transform cameraTransform;
    private Vector3 originalLocalPos;

    private void Awake()
    {
        cameraTransform = transform.GetChild(0);
        originalLocalPos = cameraTransform.localPosition;
        countdown = HangTime + 1;
    }

    private void LateUpdate()
    {
        if (Shaking)
        {
            Vector2 offset = Random.insideUnitCircle * ShakeMagnitude;

            cameraTransform.localPosition = originalLocalPos + new Vector3(
                offset.x,
                offset.y,
                0f
            );
        }
        else
        {
            cameraTransform.localPosition = originalLocalPos;
        }

        if (countdown < HangTime)
        {
            Shaking = true;
            countdown += Time.deltaTime;
        }
        else
        {
            Shaking = false;
        }

    }

    public void Shake()
    {
        countdown = Mathf.Min(0f, countdown);
    }

    public void Shake(float seconds)
    {
        countdown = Mathf.Min(HangTime - seconds, countdown);
    }
}
