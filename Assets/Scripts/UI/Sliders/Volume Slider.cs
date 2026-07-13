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
    }

    private void Start()
    {
        slider.SetValueWithoutNotify(GetVolume());
        slider.onValueChanged.AddListener(SetVolume);
    }

    private void SetVolume(float value)
    {
        Debug.Log("Setting valume of " + group + " to " + value);
        mixer.SetFloat(
            group,
            Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f
        );
    }

    public float GetVolume()
    {
        if (mixer != null)
        {
            mixer.GetFloat(group, out float dB);
            return Mathf.Pow(10f, dB / 20f);
        }
        return .5f;
    }
}