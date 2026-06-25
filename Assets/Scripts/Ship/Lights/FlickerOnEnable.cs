using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class FlickerOnEnabled : MonoBehaviour
{
    [SerializeField] private float targetIntensity = 1f;
    [SerializeField] private float flickerDuration = 0.5f;
    [SerializeField] private float flickerInterval = 0.05f;

    private Light2D light2D;

    private void Awake()
    {
        light2D = GetComponent<Light2D>();
    }

    private void OnEnable()
    {
        StartCoroutine(FlickerRoutine());
    }

    private IEnumerator FlickerRoutine()
    {
        light2D.intensity = 0f;

        float elapsed = 0f;

        while (elapsed < flickerDuration)
        {
            // Random flicker amount, weighted toward getting brighter over time
            float progress = elapsed / flickerDuration;
            light2D.intensity = Random.Range(0f, targetIntensity * Mathf.Lerp(0.3f, 1f, progress));

            yield return new WaitForSeconds(flickerInterval);
            elapsed += flickerInterval;
        }

        light2D.intensity = targetIntensity;
    }
}