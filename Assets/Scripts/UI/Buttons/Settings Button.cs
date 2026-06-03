using UnityEngine;

public class SettingsButton : MonoBehaviour
{
    public void LoadSetting(string SettingName)
    {
        SettingsHandler.Instance.OpenPage(SettingName);
    }
}
