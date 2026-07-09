using UnityEngine;
using UnityEngine.UI;

public class DisplayTutorialStats : MonoBehaviour
{
    [SerializeField] private ButtonData[] Buttons;

    [SerializeField] private Sprite completeButton;
    [SerializeField] private Sprite unlockedButton;
    [SerializeField] private Sprite lockedButton;

    [System.Serializable] public class ButtonData
    {
        public Image image;
        public Button button;
    }

    private void Update()
    {
        if (GameHandler.Instance == null) return;

        SaveData SaveData = GameHandler.Instance.GetSaveData();

        bool prevComplete = true;
        for (int i = 0; i < Mathf.Min(Buttons.Length, SaveData.tutorialData.Length); i++)
        {
            Buttons[i].image.color = Color.white; // Default
            Buttons[i].button.interactable = true; // Default

            if (SaveData.tutorialData[i].complete) Buttons[i].image.sprite = completeButton;
            else if (prevComplete) Buttons[i].image.sprite = unlockedButton;
            else
            {
                Buttons[i].button.interactable = false;
                Buttons[i].image.sprite = lockedButton;
                Buttons[i].image.color = new Color(0.2f, 0.2f, 0.2f, 1f);
            }

            prevComplete = SaveData.tutorialData[i].complete;
        }
    }
}
