using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private string group;
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(SetVolume);

        SetVolume(slider.value);
    }

    private void SetVolume(float value)
    {
        mixer.SetFloat(
            group,
            Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f
        );
    }
}