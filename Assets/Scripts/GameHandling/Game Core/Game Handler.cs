using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance;

    [SerializeField] private int TutorialCount = 5;

    private const string SAVE_KEY = "SaveData";
    private SaveData SaveData;

    [SerializeField] private float autoSaveInterval = 10f;
    private float autoSaveTimer;

    [SerializeField] private AudioMixer mixer;

    public SaveData GetSaveData()
    {
        EnsureSaveDataFormat();
        return SaveData;
    }

    public void SetSaveData(SaveData SaveData)
    {
        this.SaveData = SaveData;
        EnsureSaveDataFormat();
    }

    private void Update()
    {
        autoSaveTimer += Time.unscaledDeltaTime;

        if (autoSaveTimer >= autoSaveInterval)
        {
            autoSaveTimer = 0f;
            UpdateData();
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Load();
    }

    private void UpdateData()
    {
        SaveData.audioData.masterVolume = GetVolume("MasterVolume");
        SaveData.audioData.sfxVolume = GetVolume("SFXVolume");
        SaveData.audioData.musicVolume = GetVolume("MusicVolume");

        SaveData.bindingOverrides = InputHandler.Instance.PlayerInput.actions.SaveBindingOverridesAsJson();

        Save();
    }
    public float GetVolume(string group)
    {
        if (mixer != null)
        {
            mixer.GetFloat(group, out float dB);
            return Mathf.Pow(10f, dB / 20f);
        }
        return .5f;
    }

    public void Load()
    {
        string json = PlayerPrefs.GetString(SAVE_KEY, "");
        Debug.Log("Loaded Data:\n" + json);

        if (string.IsNullOrEmpty(json))
        {
            // First launch
            SaveData = new SaveData();
            Debug.Log("No Existing data detected. Saving blank slate");
        }
        else
        {
            SaveData = JsonUtility.FromJson<SaveData>(json);
        }
        EnsureSaveDataFormat();
        ApplySaveData();

        PrintSaveData("After Loading: ");
    }

    private void ApplySaveData()
    {
        // Audio
        SetVolume("MasterVolume", SaveData.audioData.masterVolume);
        SetVolume("MusicVolume", SaveData.audioData.musicVolume);
        SetVolume("SFXVolume", SaveData.audioData.sfxVolume);

        // Controls
        if (!string.IsNullOrEmpty(SaveData.bindingOverrides))
        {
            InputHandler.Instance.PlayerInput.actions.LoadBindingOverridesFromJson(
                SaveData.bindingOverrides);
        }
    }

    private void SetVolume(string group, float value)
    {
        if (!mixer.SetFloat(
            group,
            Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f
        ))
        {
            Debug.Log("Failed to set " + group);
        }
        else
        {
            Debug.Log("Set " + group + " to " + value + " (" + Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f + "db)");
        }
    }

    public void Save()
    {
        EnsureSaveDataFormat();

        string json = JsonUtility.ToJson(SaveData, true);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();

        PrintSaveData("Saved: ");
    }

    public void EnsureSaveDataFormat()
    {
        if (SaveData == null) SaveData = new SaveData();

        SaveData.tutorialData = ResizeArray(SaveData.tutorialData, TutorialCount);
        EnsureArrayEntries(SaveData.tutorialData);

        if (SaveData.audioData == null) SaveData.audioData = new AudioData();
        if (SaveData.aegisData == null) SaveData.aegisData = new AegisData();
    }

    private T[] ResizeArray<T>(T[] array, int newSize)
    {
        if (array == null)
            return new T[newSize];

        if (array.Length == newSize)
            return array;

        T[] resized = new T[newSize];
        System.Array.Copy(array, resized, Mathf.Min(array.Length, newSize));
        return resized;
    }

    private void EnsureArrayEntries<T>(T[] array) where T : class, new()
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == null)
                array[i] = new T();
        }
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteKey(SAVE_KEY);
        Load();
    }

    private void OnApplicationQuit()
    {
        UpdateData();
        Save();
    }

    private void PrintSaveData(string key = "")
    {
        string json = JsonUtility.ToJson(SaveData, true);
        Debug.Log(key + "Current Save Data:\n" + json);
    }
}

[System.Serializable]
public class SaveData
{
    // Audio
    public AudioData audioData;

    // Tutorial Progress
    public TutorialData[] tutorialData;

    // Map Progress
    public AegisData aegisData;

    // Bindings
    public string bindingOverrides = "";
}

[System.Serializable]
public class AegisData
{
    public float time;
    public bool escapePod;
    public bool shipControl;
}

[System.Serializable]
public class TutorialData
{
    public bool complete;
}

[System.Serializable]
public class AudioData
{
    // Audio
    public float masterVolume = .8f;
    public float musicVolume = .7f;
    public float sfxVolume = .7f;
}
